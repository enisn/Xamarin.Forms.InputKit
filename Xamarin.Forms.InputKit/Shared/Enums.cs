using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.InputKit.Shared
{
    /// <summary>
    /// Enum of Annotations. Detail will be added later.
    /// </summary>
    public enum AnnotationType
    {
        /// <summary>
        /// None of check. Allows all inputs and returns IsValidated as true.
        /// </summary>
        None,
        /// <summary>
        /// Letter chars only
        /// </summary>
        LettersOnly,
        /// <summary>
        /// Digits characters only. Also that means 'Integer' numbers.
        /// </summary>
        DigitsOnly,
        /// <summary>
        /// NonDigits characters only.
        /// </summary>
        NonDigitsOnly,
        /// <summary>
        /// Standard decimal check with coma or dot (depends on culture).
        /// </summary>
        Decimal,
        /// <summary>
        /// Standard email check.
        /// </summary>
        Email,
        /// <summary>
        /// Standard password check (at least a number and at least a char). MinLength should be set seperately.
        /// </summary>
        Password,
        /// <summary>
        /// Standard phone number check. Also you should use MinLength property with this type.
        /// </summary>
        Phone,
        /// <summary>
        /// You need to set RegexPattern property as your regex query to use this.
        /// </summary>
        RegexPattern,
        /// <summary>
        /// Short Type only
        /// </summary>
        ShortType,
        /// <summary>
        /// Int Type only
        /// </summary>
        IntType,
        /// <summary>
        /// Long Type only
        /// </summary>
        LongType,
        /// <summary>
        /// Float Type only
        /// </summary>
        FloatType,
        /// <summary>
        /// Double Type only
        /// </summary>
        DoubleType,
        /// <summary>
        /// Decimal Type only
        /// </summary>
        DecimalType,
        /// <summary>
        /// Byte Type only
        /// </summary>
        ByteType,
        /// <summary>
        /// SByte Type only
        /// </summary>
        SByteType,
        /// <summary>
        /// Char Type only
        /// </summary>
        CharType,
        /// <summary>
        /// UInt Type only
        /// </summary>
        UIntType,
        /// <summary>
        /// ULong Type only
        /// </summary>
        ULongType,
        /// <summary>
        /// UShort Type only
        /// </summary>
        UShortType
    }

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
}
