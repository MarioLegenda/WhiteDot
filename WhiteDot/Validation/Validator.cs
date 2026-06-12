using WhiteDot.YamlRoot;

namespace WhiteDot.Validation;

internal class Validator
{
    public static void Validate(Root data)
    {
        if (data.Select != null)
        {
            SelectValidator simpleValidator = new SelectValidator(data.Select);
                
            simpleValidator.Validate();
        }

        if (data.Insert != null)
        {
            InsertValidator insertValidator = new InsertValidator(data.Insert);
                
            insertValidator.Validate();
        }
    }
}