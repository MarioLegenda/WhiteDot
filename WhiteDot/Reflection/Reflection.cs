using System.Data.Common;
using Microsoft.VisualBasic;
using WhiteDot.Exceptions;
using WhiteDot.Repository;
using WhiteDot.Representation;

namespace WhiteDot.Reflection;

internal class Reflection
{
    private SelectRepresentation _representation;
    private SelectRepository _repository;

    public Reflection(SelectRepresentation representation, SelectRepository repository)
    {
        this._representation = representation;
        this._repository = repository;
    }

    public async Task<object> CreateInstance<T>()
    {
        Type? type = typeof(T);
        if (type == null)
            throw new TypeNotFoundException($"Type {nameof(T)} not found.");

        if (type.Name == "List`1")
        {
            await using DbDataReader multipleReader = await this._repository.SelectMultiple();
            
            Type genericArg = type.GetGenericArguments()[0];
            if (genericArg == null)
                throw new TypeNotFoundException($"Type {nameof(genericArg)} not found.");

            Type listType = typeof(List<>).MakeGenericType(genericArg);
            if (listType == null)
                throw new TypeNotFoundException($"Type {nameof(listType)} not found.");

            object listInstance = Activator.CreateInstance(listType)!;

            if (listInstance == null)
                throw new TypeNotFoundException($"Type could not be created into an instance.");

            var addMethod = listType.GetMethod("Add")!;

            object genericArgInstance = Activator.CreateInstance(genericArg)!;
            if (genericArgInstance == null)
                throw new TypeNotFoundException($"Type could not be created into an instance.");

            while (await multipleReader.ReadAsync())
            {
                this.AddToProperties(genericArg, genericArgInstance, multipleReader);
                
                addMethod.Invoke(listInstance, new[] { genericArgInstance });
            }
            
            return listInstance;
        }

        var (singleReader, exists) = await this._repository.SelectSingle();
        if (!exists)
        {
            return null;
        }

        object instance = Activator.CreateInstance(type)!;
        if (instance == null)
            throw new TypeNotFoundException($"Type could not be created into an instance.");
        
        this.AddToProperties(type, instance, singleReader);

        return instance;
    }

    private void AddToProperties(Type type, object instance, DbDataReader reader)
    {
        foreach (var prop in this._representation.Properties)
        {
            var from = reader[prop.From];
            var to = prop.To;

            var reflectedProperty = type.GetProperty(to);
            if (reflectedProperty is not null)
            {
                if (from == DBNull.Value)
                {
                    reflectedProperty.SetValue(instance, null);
                }
                else if (reflectedProperty.PropertyType.Name == "DateOnly")
                {
                    string? text = Convert.ToString(from);
                    if (text is null)
                    {
                        throw new TypeException(@"Cannot convert {to} to a string to make it a DateOnly instance");
                    }

                    var backlashSplit = Strings.Split(text, "/");
                    if (backlashSplit.Length == 3)
                    {
                        int day = 0;
                        var month = 0;
                        var year = 0;
                        Int32.TryParse(backlashSplit[0], out month);
                        Int32.TryParse(backlashSplit[1], out day);
                        Int32.TryParse(backlashSplit[2], out year);

                        try
                        {
                            reflectedProperty.SetValue(instance, new DateOnly(year, month, day));
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            throw new TypeException(
                                $@"Invalid date for {prop.From}. DateOnly instance only accepts year/month/day format");
                        }
                    }

                    var dotSplit = Strings.Split(text, ".");
                    if (dotSplit.Length == 3)
                    {
                        int day = 0;
                        var month = 0;
                        var year = 0;
                        Int32.TryParse(dotSplit[0], out month);
                        Int32.TryParse(dotSplit[1], out day);
                        Int32.TryParse(dotSplit[2], out year);

                        try
                        {
                            reflectedProperty.SetValue(instance, new DateOnly(year, month, day));
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            throw new TypeException(
                                $@"Invalid date for {prop.From}. DateOnly instance only accepts year/month/day format");
                        }
                    }

                    else
                    {
                        reflectedProperty.SetValue(
                            instance,
                            Convert.ChangeType(from, reflectedProperty.PropertyType)
                        );
                    }
                }
                else
                {
                    reflectedProperty.SetValue(
                        instance,
                        Convert.ChangeType(from, reflectedProperty.PropertyType)
                    );
                }
            }
        }
    }

}