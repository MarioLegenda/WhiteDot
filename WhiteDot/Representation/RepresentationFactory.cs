using WhiteDot.YamlRoot;

namespace WhiteDot.Representation;

internal class RepresentationFactory
{
    private Dictionary<string,
        Dictionary<string,
            Dictionary<string, SimpleDefinition>>> _representations;
    
    public RepresentationFactory(Dictionary<string,
        Dictionary<string,
            Dictionary<string, SimpleDefinition>>> representations)
    {
        this._representations = representations;
    }

    public Dictionary<string, SelectRepresentation> CreateSelectRepresentations(Dictionary<string, List<string>> parameters)
    {
        var representations = new Dictionary<string, SelectRepresentation>();
        
        if (this._representations.ContainsKey("simple") && this._representations["simple"].ContainsKey("select"))
        {
            var select = this._representations["simple"]["select"];

            foreach (var (key, value) in select)
            {
                representations[key] = new SelectRepresentation(
                    value.Sql, 
                    value.Namespace, 
                    value.Assembly,
                    parameters[key]
                );
            }
        }

        return representations;
    }
}