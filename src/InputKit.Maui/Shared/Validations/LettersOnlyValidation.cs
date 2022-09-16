namespace InputKit.Shared.Validations;
public class LettersOnlyValidation : IValidation
{
    public string Message { get; set; } = "The field should contain only letters.";

    public bool Validate(object value)
    {
        if (value is string text)
        {
            return text.All(char.IsLetter);
        }

        return false;
    }
}
