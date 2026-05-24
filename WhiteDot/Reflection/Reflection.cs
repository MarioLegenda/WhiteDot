using System.Data.Common;
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
                throw new TypeException($@"Property {prop.To} on {type.Name}");
            }
        }

        return instance;
    }
}