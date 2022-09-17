namespace Plugin.InputKit.Shared.Validations
{
    public interface IValidation
    {
        string Message { get; }
        bool Validate(object value);
    }
}