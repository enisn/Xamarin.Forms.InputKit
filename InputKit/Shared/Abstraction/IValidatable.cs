using System;

namespace Plugin.InputKit.Shared.Abstraction
{
    interface IValidatable
    {
        bool IsRequired { get; set; }
        bool IsValidated { get; }
        string ValidationMessage { get; set; }
        void DisplayValidation();
    }
}
