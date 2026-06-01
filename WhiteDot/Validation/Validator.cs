using WhiteDot.YamlRoot;

namespace WhiteDot.Validation;

internal class Validator
{
    public static void Validate(
        Dictionary<string, Dictionary<string, SimpleDefinition>> data,
        Dictionary<string, List<string>> parameters)
    {
        if (data.ContainsKey("select"))
        {
            SelectValidator simpleValidator = new SelectValidator(data["select"], parameters);
                
            simpleValidator.Validate();
        }
        
        if (data.ContainsKey("insert"))
        {
            InsertValidator insertValidator = new InsertValidator(data["insert"], parameters);
                
            insertValidator.Validate();
        }
    }
}