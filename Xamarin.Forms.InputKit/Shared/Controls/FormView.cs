using Microsoft.Maui.Controls;
using Plugin.InputKit.Shared.Abstraction;
using System;
using System.Collections.Generic;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// Quickly gets last result of all IValidatable elements inside of this
    /// </summary>
    public partial class FormView : Microsoft.Maui.Controls.StackLayout
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public FormView()
        {
            DeclareEvents();
            this.ChildAdded += FormView_ChildAdded;
            this.ChildRemoved += FormView_ChildRemoved;
        }

        private void FormView_ChildRemoved(object sender, ElementEventArgs e)
        {
            if (e is IValidatable validatable)
                validatable.ValidationChanged -= FormView_ValidationChanged;
        }

        private void FormView_ChildAdded(object s, ElementEventArgs e)
        {
            if (e.Element is IValidatable validatable)
            {
                SetEvent(validatable);
            }
            else if (e.Element is Layout layout)
            {
                layout.ChildAdded -= FormView_ChildAdded;
                layout.ChildAdded += FormView_ChildAdded;
                layout.ChildRemoved -= FormView_ChildRemoved;
                layout.ChildRemoved += FormView_ChildRemoved;
                foreach (var child in GetChildIValidatables(layout))
                    SetEvent(child);
            }
        }

        void DeclareEvents()
        {
            foreach (var item in GetChildIValidatables(this))
                SetEvent(item);
        }

        void SetEvent(IValidatable view)
        {
            if (view == null) return;
            view.ValidationChanged -= FormView_ValidationChanged;
            view.ValidationChanged += FormView_ValidationChanged;
        }
        private void FormView_ValidationChanged(object sender, EventArgs e)
        {
            if (!(sender as IValidatable).IsValidated) SetValue(IsValidatedProperty, false);
            else SetValue(IsValidatedProperty, CheckValidation(this));
        }
        /// <summary>
        /// Shows if all elements inside of this are validated or not
        /// </summary>
        public bool IsValidated
        {
            get
            {
                return (bool)GetValue(IsValidatedProperty);
            }

            set { /* To make visible in XAML pages */ }
        }
        /// <summary>
        /// Checks and element is validated or not
        /// </summary>
        /// <param name="view">A view which IValidated implemented</param>
        /// <returns></returns>
        public static bool CheckValidation(Layout view)
        {
            foreach (var item in GetChildIValidatables(view))
                if (!item.IsValidated)
                    return false;
            return true;
        }

        static IEnumerable<IValidatable> GetChildIValidatables(Layout layout)
        {
            foreach (var item in layout.Children)
            {
                if (item is IValidatable validatable)
                    yield return validatable;

                else if (item is Layout la)
                    foreach (var child in GetChildIValidatables(la))
                        yield return child;
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty IsValidatedProperty = BindableProperty.Create(nameof(IsValidated), typeof(bool), typeof(FormView), false, BindingMode.OneWayToSource);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}
