using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using WhiteDot;
using WhiteDot.Exceptions;
using WhiteDot.YamlRoot;

internal class Validator
{
    public static void Validate(
        Dictionary<string, Dictionary<string, Dictionary<string, SimpleDefinition>>> data,
        Dictionary<string, List<string>> parameters)
    {
        if (data.ContainsKey("simple"))
        {
            var simple = data["simple"];
            if (!simple.ContainsKey("select"))
            {
                throw new InvalidConfigException($"Invalid config. Expected 'select', got {simple.First().Key}");
            }

            foreach (var (key, value) in simple["select"])
            {
                var sqlStatement = simple["select"][key];

                if (string.IsNullOrWhiteSpace(sqlStatement.Sql))
                {
                    throw new InvalidConfigException("Invalid config. Expected key 'sql' is missing");
                }
                
                var sqlParameters = new List<string>();
                foreach (Match match in Regex.Matches(sqlStatement.Sql, @":\w+", RegexOptions.IgnoreCase))
                {
                    sqlParameters.Add(match.Value.TrimStart(':'));
                }

                parameters[key] = sqlParameters;

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
}