using System.ComponentModel;
using System.Text.RegularExpressions;
using WhiteDot.Exceptions;
using WhiteDot.YamlRoot;

namespace WhiteDot.Validation;

internal class SelectValidator: IValidator
{
    private readonly Dictionary<string, SelectDefinition> _definitions;
    
    public SelectValidator(Dictionary<string, SelectDefinition> definitions)
    {
        this._definitions = definitions;
    }
    
    public void Validate()
    {
        foreach (var (key, value) in this._definitions)
        {
            var sqlStatement = this._definitions[key];
            
            if (string.IsNullOrWhiteSpace(sqlStatement.Sql))
            {
                throw new InvalidConfigException("Invalid config. Expected key 'sql' is missing");
            }
            
            if (sqlStatement.Properties is null)
            {
                throw new InvalidConfigException("Invalid config. Key 'properties' cannot be empty");
            }
            
            if (sqlStatement.Properties.Count == 0)
            {
                throw new InvalidConfigException("Invalid config. Key 'properties' cannot be empty");
            }
        }
    }
}