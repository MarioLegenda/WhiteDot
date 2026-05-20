using WhiteDot.YamlRoot;

namespace WhiteDot.Validation;

internal class Validator
{
    public static void Validate(
        Dictionary<string, Dictionary<string, Dictionary<string, SimpleDefinition>>> data,
        Dictionary<string, List<string>> parameters)
    {
        if (data.ContainsKey("simple"))
        {
            var simple = data["simple"];
            if (simple.ContainsKey("select"))
            {
                SelectValidator simpleValidator = new SelectValidator(simple["select"], parameters);
                
                simpleValidator.Validate();
            }

            if (simple.ContainsKey("insert"))
            {
                InsertValidator insertValidator = new InsertValidator(simple["insert"], parameters);
                
                insertValidator.Validate();
            }


        }
    }
}