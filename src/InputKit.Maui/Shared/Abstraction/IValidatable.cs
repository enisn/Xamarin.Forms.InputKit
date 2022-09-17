using InputKit.Shared.Validations;
using System;
using System.ComponentModel;

namespace InputKit.Shared.Abstraction;

public interface IValidatable : INotifyPropertyChanged
{
    public List<IValidation> Validations { get; }

    bool IsValid { get; }

    void DisplayValidation();
}
