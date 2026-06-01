using WhiteDot.YamlRoot;

namespace WhiteDot.Representation;

internal class RepresentationFactory
{
    private Dictionary<string,
            Dictionary<string, SelectDefinition>> _representations;
    
    public RepresentationFactory(
        Dictionary<string,
            Dictionary<string, SelectDefinition>> representations)
    {
        this._representations = representations;
    }

    public Dictionary<string, SelectRepresentation> CreateSelectRepresentations(Dictionary<string, List<string>> parameters)
    {
        var representations = new Dictionary<string, SelectRepresentation>();
        
        if (this._representations.ContainsKey("select"))
        {
            var select = this._representations["select"];

            foreach (var (key, value) in select)
            {
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