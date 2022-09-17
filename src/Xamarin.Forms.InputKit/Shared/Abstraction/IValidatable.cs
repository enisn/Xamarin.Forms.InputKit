using Plugin.InputKit.Shared.Validations;
using System.Collections.Generic;
using System.ComponentModel;

namespace Plugin.InputKit.Shared.Abstraction
{
    public interface IValidatable : INotifyPropertyChanged
    {
        List<IValidation> Validations { get; }

        bool IsValid { get; }

        void DisplayValidation();
    }
}
