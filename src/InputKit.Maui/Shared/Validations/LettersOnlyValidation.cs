namespace InputKit.Shared.Validations;
public class LettersOnlyValidation : BindableObject, IValidation
{
    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(string),
        typeof(LettersOnlyValidation),
        "The field should contain only letters.");
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly BindableProperty AllowSpacesProperty = BindableProperty.Create(
        nameof(AllowSpaces),
        typeof(bool),
        typeof(LettersOnlyValidation),
        true);

    public bool AllowSpaces
    {
        get => (bool)GetValue(AllowSpacesProperty);
        set => SetValue(AllowSpacesProperty, value);
    }

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
