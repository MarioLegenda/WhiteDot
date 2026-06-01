using System.ComponentModel;
using System.Text.RegularExpressions;
using WhiteDot.Exceptions;
using WhiteDot.YamlRoot;

namespace WhiteDot.Validation;

internal class SelectValidator: IValidator
{
    private Dictionary<string, SelectDefinition> _definitions;
    private Dictionary<string, List<string>> _parameters;

    public SelectValidator(Dictionary<string, SelectDefinition> definitions, Dictionary<string, List<string>> parameters)
    {
        this._definitions = definitions;
        this._parameters = parameters;
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
                
            var sqlParameters = new List<string>();
            foreach (Match match in Regex.Matches(sqlStatement.Sql, @":\w+", RegexOptions.IgnoreCase))
            {
                sqlParameters.Add(match.Value.TrimStart(':'));
            }

            this._parameters[key] = sqlParameters;

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