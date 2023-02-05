namespace Plugin.InputKit.Shared.Validations
{
    public class MaxLengthValidation : IValidation
    {
        private string message;
        public string Message { get => message ?? $"The field should contain maxium {MaxLength} character."; set => message = value; }
        public int MaxLength { get; set; }

        public bool Validate(object value)
        {
            if (value is string text)
            {
                return text.Length <= MaxLength;
            }

            return true;
        }
    }
}