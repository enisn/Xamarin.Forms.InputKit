using InputKit.Shared.Validations;
using System;

namespace InputKit.Shared.Abstraction;

public interface IValidatable
{
    public List<IValidation> Validations { get; }

    bool IsValid { get; }

    void DisplayValidation();
}
