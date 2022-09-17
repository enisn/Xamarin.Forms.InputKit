namespace InputKit.Shared.Validations;
public class LettersOnlyValidation : IValidation
{
    public string Message { get; set; } = "The field should contain only letters.";

    public bool AllowSpaces { get; set; } = true;

    public bool Validate(object value)
    {
        if (value is string text)
        {
            if (AllowSpaces)
            {
                return text.All(x => char.IsLetter(x) || char.IsWhiteSpace(x));
            }

            return text.All(char.IsLetter);
        }

        return false;
    }
}
