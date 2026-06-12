using System.Text.RegularExpressions;
using WhiteDot.YamlRoot;

namespace WhiteDot.Representation;

internal class RepresentationFactory
{
    private Root _representations;
    
    public RepresentationFactory(
        Root representations)
    {
        this._representations = representations;
    }

    public Dictionary<string, SelectRepresentation> CreateSelectRepresentations()
    {
        var representations = new Dictionary<string, SelectRepresentation>();

        if (this._representations.Select != null)
        {
            var select = this._representations.Select;
            foreach (var (key, value) in select)
            {
                var parameters = new Dictionary<string, List<string>>();
                var sqlParameters = new List<string>();
                foreach (Match match in Regex.Matches(value.Sql, @":\w+", RegexOptions.IgnoreCase))
                {
                    sqlParameters.Add(match.Value.TrimStart(':'));
                }
                parameters[key] = sqlParameters;
                
                var properties = new List<Property>();
                foreach (var prop in value.Properties)
                {
                    var parts = prop.Split(" to ");

                    var from = parts[0];
                    var to = parts[1];
                    var createdProperty = new Property(from, to);
                    
                    properties.Add(createdProperty);
                }

                representations[key] = new SelectRepresentation(
                    value.Sql,
                    properties,
                    parameters[key]
                );
            }
        }

        return representations;
    }
}