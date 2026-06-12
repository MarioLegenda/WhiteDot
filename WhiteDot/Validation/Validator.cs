using WhiteDot.YamlRoot;

namespace WhiteDot.Validation;

internal class Validator
{
    public static void Validate(
        Root data,
        Dictionary<string, List<string>> parameters)
    {
        if (data.Select != null)
        {
            SelectValidator simpleValidator = new SelectValidator(data.Select, parameters);
                
            simpleValidator.Validate();
        }

        if (data.Insert != null)
        {
            InsertValidator insertValidator = new InsertValidator(data.Insert, parameters);
                
            insertValidator.Validate();
        }
    }
}