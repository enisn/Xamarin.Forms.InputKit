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
        RegexPattern
    }

    public enum LabelPosition
    {
        Before,
        After
    }
}
