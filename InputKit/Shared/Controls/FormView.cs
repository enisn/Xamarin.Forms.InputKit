using Plugin.InputKit.Shared.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// Quickly gets last result of all IValidatable elements inside of this
    /// </summary>
    public partial class FormView : StackLayout
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public FormView()
        {
            DeclareEvents();
            this.ChildAdded += (s, args) =>
            {
                if (args.Element is IValidatable)
                    SetEvent(args.Element as IValidatable);
            };
            this.ChildRemoved += (s, args) => { if (args is IValidatable) (args as IValidatable).ValidationChanged -= FormView_ValidationChanged; };
        }
        void DeclareEvents()
        {
            foreach (var item in this.Children)
            {
                if (item is IValidatable)
                    SetEvent(item as IValidatable);
            }
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

            set { }
        }
        /// <summary>
        /// Checks and element is validated or not
        /// </summary>
        /// <param name="view">A view which IValidated implemented</param>
        /// <returns></returns>
        public static bool CheckValidation(Layout<View> view)
        {
            foreach (var item in view.Children)
                if (item is IValidatable)
                    if (!(item as IValidatable).IsValidated)
                        return false;
            return true;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty IsValidatedProperty = BindableProperty.Create(nameof(IsValidated), typeof(bool), typeof(FormView), false, BindingMode.OneWayToSource);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}
