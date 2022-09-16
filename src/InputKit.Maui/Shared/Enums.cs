using System;
using System.Collections.Generic;
using System.Text;

namespace InputKit.Shared;

public enum LabelPosition
{
    After,
    Before
}

/// <summary>
/// Types of selectionlist
/// </summary>
public enum SelectionType
{
    Button = 1,
    RadioButton = 3,
    CheckBox = 2,
    MultipleButton = 4,
    SingleCheckBox = 5,
    MultipleRadioButton = 6,
}
