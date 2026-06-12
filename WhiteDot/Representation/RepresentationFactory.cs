using System.Text.RegularExpressions;
using WhiteDot.YamlRoot;

namespace WhiteDot.Representation;

internal class RepresentationFactory
{
    private readonly Root _representations;
    
    public RepresentationFactory(Root representations)
    {
        this._representations = representations;
    }

    public Dictionary<string, SelectRepresentation> CreateSelectRepresentations()
    {
        var representations = new Dictionary<string, SelectRepresentation>();

        if (this._representations.Select is not null)
        {
            foreach (var (key, value) in this._representations.Select)
            {
                representations[key] = new SelectRepresentation(
                    value.Sql,
                    this.createProperties(value.Properties),
                    this.createParameters(value.Sql)
                );
            }
        }

        return representations;
    }

    private List<string> createParameters(string sql)
    {
        var sqlParameters = new List<string>();
        foreach (Match match in Regex.Matches(sql, @":\w+", RegexOptions.IgnoreCase))
        {
            sqlParameters.Add(match.Value.TrimStart(':'));
        }

        return sqlParameters;
    }

    private List<Property> createProperties(List<string> props)
    {
        var properties = new List<Property>();
        foreach (var prop in props)
        {
            var parts = prop.Split(" to ");

            var from = parts[0];
            var to = parts[1];
            var createdProperty = new Property(from, to);
                    
            properties.Add(createdProperty);
        }

        return properties;
    }
}