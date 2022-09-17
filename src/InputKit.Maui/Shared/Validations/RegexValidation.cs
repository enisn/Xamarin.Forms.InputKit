using System.Text.RegularExpressions;

namespace InputKit.Shared.Validations;
public class RegexValidation : IValidation
{
    public string Pattern { get; set; }
    public string Message { get; set; } = "The field isn't valid.";

    public bool Validate(object value)
    {
        if (value is string text)
        {
            var result = Regex.Match(text, Pattern);

            return result.Success;
        }

        return false;
    }
}
