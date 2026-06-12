using System.Text.RegularExpressions;
using WhiteDot.Exceptions;
using WhiteDot.YamlRoot;

namespace WhiteDot.Validation;

internal class InsertValidator: IValidator
{
    private readonly Dictionary<string, InsertDefinition> _definitions;

    public InsertValidator(Dictionary<string, InsertDefinition> definitions)
    {
        this._definitions = definitions;
    }
    
    public void Validate()
    {
        foreach (var (key, value) in this._definitions)
        {
            var sqlStatement = this._definitions[key];

            if (sqlStatement is null)
            {
                throw new InvalidConfigException("Invalid config. Expected key 'sql' is missing");
            }

            if (string.IsNullOrWhiteSpace(sqlStatement.Sql))
            {
                throw new InvalidConfigException("Invalid config. Expected key 'sql' is missing");
            }
        }
    }
}