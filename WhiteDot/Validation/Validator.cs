using WhiteDot.YamlRoot;

namespace WhiteDot.Validation;

internal class Validator
{
    public static void Validate(Root data)
    {
        if (data.Select is not null)
        {
            SelectValidator simpleValidator = new SelectValidator(data.Select);
                
            simpleValidator.Validate();
        }

        if (data.Insert is not null)
        {
            InsertValidator insertValidator = new InsertValidator(data.Insert);
                
            insertValidator.Validate();
        }
    }
}