using System.Data.Common;
using Microsoft.VisualBasic;
using WhiteDot.Exceptions;
using WhiteDot.Representation;

namespace WhiteDot.Reflection;

internal class Reflection
{
    private SelectRepresentation _representation;
    private DbDataReader _reader;
    
    public Reflection(SelectRepresentation representation, DbDataReader reader)
    {
        this._representation = representation;
        this._reader = reader;
    }
    
    public object CreateSingleInstance()
    {
        string className = $@"{this._representation.Nmspace}, {this._representation.Assembly}";
        Type? type = Type.GetType(className);
        
        if (type == null)
            throw new TypeNotFoundException($"Type '{className}' not found.");
            
        object instance = Activator.CreateInstance(type)!;
        if (instance == null)
            throw new TypeNotFoundException($"Type '{className}' could not be created into an instance.");

        foreach (var prop in this._representation.Properties)
        {
            var from = this._reader[prop.From];
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
                            throw new TypeException($@"Invalid date for {prop.From}. DateOnly instance only accepts year/month/day format");
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
                            throw new TypeException($@"Invalid date for {prop.From}. DateOnly instance only accepts year/month/day format");
                        }                    }
                }
                else
                {
                    Console.WriteLine($@"Enters for {reflectedProperty.Name}");
                    reflectedProperty.SetValue(
                        instance,
                        Convert.ChangeType(from, reflectedProperty.PropertyType)
                    );
                }
            }
            else
            {
                throw new TypeException($@"Property {prop.To} on {type.Name}");
            }
        }

        return instance;
    }
}