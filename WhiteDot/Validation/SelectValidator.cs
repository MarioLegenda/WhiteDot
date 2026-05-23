using System.Text.RegularExpressions;
using WhiteDot.Exceptions;
using WhiteDot.YamlRoot;

namespace WhiteDot.Validation;

internal class SelectValidator: IValidator
{
    private Dictionary<string, SimpleDefinition> _definitions;
    private Dictionary<string, List<string>> _parameters;

    public SelectValidator(Dictionary<string, SimpleDefinition> definitions, Dictionary<string, List<string>> parameters)
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

            if (string.IsNullOrWhiteSpace(sqlStatement.Namespace))
            {
                throw new InvalidConfigException("Invalid config. Expected key 'namespace' is missing");
            }
                
            if (string.IsNullOrWhiteSpace(sqlStatement.Assembly))
            {
                throw new InvalidConfigException("Invalid config. Expected key 'assembly' is missing");
            }
            
            
        }
    }
}