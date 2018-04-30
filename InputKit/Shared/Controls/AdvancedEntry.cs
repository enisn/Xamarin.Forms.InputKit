using Plugin.InputKit.Shared.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// This Entry contains validation and some stuffs inside
    /// </summary>
    public class AdvancedEntry : Entry, IValidatable
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AdvancedEntry()
        {
            this.TextChanged += AdvancedEntry_TextChanged;
        }

        private void AdvancedEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Text.Length > this.MaxLength)
                this.Text = this.Text.Substring(0, MaxLength);

            if (!IsValidated)
            {

                switch (this.Validation)
                {
                    case AnnotationType.Letter:
                        this.Text = this.Text.Where(t => Char.IsLetter(t)).Aggregate("", (current, t) => current + t);
                        break;
                    case AnnotationType.Integer:
                        this.Text = this.Text.Where(t => Char.IsNumber(t)).Aggregate("", (current, t) => current + t);
                        break;
                    case AnnotationType.Text:
                        this.Text = this.Text.Where(t => !(Char.IsSeparator(t) || Char.IsSymbol(t) || Char.IsSurrogate(t) || Char.IsControl(t) || Char.IsPunctuation(t))).Aggregate("", (current, t) => current + t);
                        break;
                    case AnnotationType.Number:
                    case AnnotationType.Money:
                        this.Text = this.Text.Where(t => (Char.IsNumber(t) || t == '.')).Aggregate("", (current, t) => current + t);
                        break;
                }
            }
        }

        public string ValidationMessage { get; set; }
        public short MaxLength { get; set; }

        public bool IsValidated
        {
            get
            {
                switch (Validation)
                {
                    case AnnotationType.Letter:
                        return !this.Text.Any(a => !Char.IsLetter(a));
                    case AnnotationType.Integer:
                        return !this.Text.Any(a => !Char.IsNumber(a));
                    case AnnotationType.Text:
                        return !this.Text.Any(a => Char.IsSeparator(a) || Char.IsSymbol(a) || Char.IsSurrogate(a) || Char.IsControl(a) || Char.IsPunctuation(a));
                    case AnnotationType.Number:
                    case AnnotationType.Money:
                        return !this.Text.Any(a=>!Char.IsNumber(a));
                    case AnnotationType.Email:
                        return this.Text.Contains("@") && this.Text.Contains(".") && this.Text.Substring(this.Text.IndexOf('@'),this.Text.Length - this.Text.IndexOf('@')-1).Length >=2;
                    case AnnotationType.Password:
                        //Will be added
                        return true;
                }
                return true;
            }
        }
        public bool IsRequired { get; set; }
        public AnnotationType Validation { get; set; }

        void UpdateKeyboard(AnnotationType annotation)
        {
            switch (annotation)
            {
                case AnnotationType.Letter:
                case AnnotationType.Text:
                    this.Keyboard = Keyboard.Chat;
                    break;
                case AnnotationType.Integer:
                case AnnotationType.Number:
                case AnnotationType.Money:
                    this.Keyboard = Keyboard.Numeric;
                    break;
                case AnnotationType.Email:
                    this.Keyboard = Keyboard.Email;
                    break;
                case AnnotationType.Phone:
                    this.Keyboard = Keyboard.Telephone;
                    break;
                case AnnotationType.Password:
                    this.Keyboard = Keyboard.Plain;
                    this.IsPassword = true;
                    break;
                default:
                    this.Keyboard = Keyboard.Default;
                    break;
            }
        }

        public enum AnnotationType
        {
            None = 0,
            Letter = 1 << 1,
            Integer = 1 << 2,
            Number = 1 << 3,
            Text = 1 << 4,
            Money = 1 << 16,
            Email = 1 << 32,
            Password = 99,
            Phone = 100
        }
    }
}
