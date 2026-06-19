using System.Text.RegularExpressions;
using WhiteDot.YamlRoot;

namespace WhiteDot.Representation;

internal class RepresentationFactory
{
    private Dictionary<string, SelectRepresentation> _selectRepresentations;
    private Dictionary<string, WriteRepresentation> _writeRepresentations;

    public RepresentationFactory(
        Dictionary<string, SelectRepresentation> selectRepresentations, 
        Dictionary<string, WriteRepresentation> writeRepresentations
    )
    {
        this._selectRepresentations = selectRepresentations;
        this._writeRepresentations = writeRepresentations;
    }

    public void AddToSelectRepresentation(Root data)
    {
        if (data.Select is not null)
        {
            foreach (var (key, value) in data.Select)
            {
                IfExistsRepresentation ifExistsRepresentation = null!;
                if (value.IfExists is not null)
                {
                    ifExistsRepresentation =
                        new IfExistsRepresentation(value.IfExists.Sql, this.createParameters(value.IfExists.Sql));
                }
                
                this._selectRepresentations[key] = new SelectRepresentation(
                    value.Sql,
                    this.createProperties(value.Properties),
                    this.createParameters(value.Sql),
                    ifExistsRepresentation
                );
            }
        }
    }

    public void AddToWriteRepresentation(Root data)
    {
        if (data.Write is not null)
        {
            foreach (var (key, value) in data.Write)
            {
                this._writeRepresentations[key] = new WriteRepresentation(
                    value.Sql, 
                    this.createParameters(value.Sql)
                );
            }
        }
    }

    private List<string> createParameters(string sql)
    {
        var sqlParameters = new List<string>();
        foreach (Match match in Regex.Matches(sql, @"@\w+", RegexOptions.IgnoreCase))
        {
            sqlParameters.Add(match.Value.TrimStart('@'));
        }

        return sqlParameters;
    }

    private List<Property> createProperties(List<string> props)
    {
        var properties = new List<Property>();
        foreach (var prop in props)
        {
            var parts = prop.Split(":");

            var from = parts[0];
            var to = parts[1];
            var createdProperty = new Property(from, to);
                    
            properties.Add(createdProperty);
        }

        return properties;
    }
}