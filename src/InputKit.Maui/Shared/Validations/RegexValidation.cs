using System.Text.RegularExpressions;

namespace InputKit.Shared.Validations;
public class RegexValidation : BindableObject, IValidation
{
    public string Pattern
    {
        get => (string)GetValue(PatternProperty);
        set => SetValue(PatternProperty, value);
    }

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly BindableProperty PatternProperty = BindableProperty.Create(
        nameof(Pattern),
        typeof(string),
        typeof(RegexValidation),
        string.Empty);

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(string),
        typeof(RegexValidation),
        "The field isn't valid.");

    public bool Validate(object value)
    {
        if (value is string text)
        {
            var result = Regex.Match(text, Pattern);

            return result.Success;
        }

        return true;
    }
}
