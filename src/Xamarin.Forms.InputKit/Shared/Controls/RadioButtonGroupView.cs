using Plugin.InputKit.Shared.Abstraction;
using Plugin.InputKit.Shared.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using RadioButton = Plugin.InputKit.Shared.Controls.RadioButton;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// Groups radiobuttons, Inherited StackLayout.
    /// </summary>
    public partial class RadioButtonGroupView : StatefulStackLayout, IValidatable
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Default constructor of RadioButtonGroupView
        /// </summary>
        public RadioButtonGroupView()
        {
            this.ChildAdded += OnChildAdded;
            this.ChildrenReordered += RadioButtonGroupView_ChildrenReordered;
        }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Invokes when tapped on RadioButon
        /// </summary>
        public event EventHandler SelectedItemChanged;
        /// <summary>
        /// Implementation of IValidatable, Triggered when value changed.
        /// </summary>
        public event EventHandler ValidationChanged;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Executes when tapped on RadioButton
        /// </summary>
        public ICommand SelectedItemChangedCommand
        {
            get => (ICommand)GetValue(SelectedItemChangedCommandProperty);
            set => SetValue(SelectedItemChangedCommandProperty,value);
        }
        /// <summary>
        /// Command Parameter will be sent in SelectedItemChangedCommand
        /// </summary>
        public object CommandParameter 
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value); 
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// this will be added later
        /// </summary>
        public async void DisplayValidation()
        {
            this.BackgroundColor = Color.Red;
            await Task.Delay(500);
            this.BackgroundColor = Color.Transparent;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Returns selected radio button's index from inside of this.
        /// </summary>
        public int SelectedIndex
        {
            get => (int)GetValue(RadioButtonGroupView.SelectedIndexProperty);
            set => SetValue(RadioButtonGroupView.SelectedIndexProperty, value);
        }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Returns selected radio button's Value from inside of this.
        /// You can change the selectedItem too by sending a Value which matches ones of radio button's value
        /// </summary>
        public object SelectedItem
        {
            get => GetValue(RadioButtonGroupView.SelectedItemProperty);
            set => SetValue(RadioButtonGroupView.SelectedItemProperty, value);
        }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// It will be added later
        /// </summary>
        public bool IsRequired { get; set; }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// It will be added later
        /// </summary>
        public bool IsValidated { get => !this.IsRequired || this.SelectedIndex >= 0; }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// It will be added later
        /// </summary>
        public string ValidationMessage { get; set; }

        #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(RadioButtonGroupView), null, propertyChanged: (bo, ov, nv) => (bo as RadioButtonGroupView).UpdateToSelectedItem());
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(RadioButtonGroupView), -1, BindingMode.TwoWay, propertyChanged: UpdateSelectedIndex);

        private static void UpdateSelectedIndex(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadioButtonGroupView rg)
            {
                rg.UpdateToSelectedIndex();
                rg.SelectedIndex = (int)newValue;
            }
        }

        public static readonly BindableProperty SelectedItemChangedCommandProperty = BindableProperty.Create(nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(RadioButtonGroupView), null, propertyChanged: (bo, ov, nv) => (bo as RadioButtonGroupView).SelectedItemChangedCommand = (ICommand)nv);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(RadioButtonGroupView), null, propertyChanged: OnCommandParameterChanged,defaultBindingMode:BindingMode.TwoWay);

        private static void OnCommandParameterChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadioButtonGroupView rg)
            {
                rg.CommandParameter = newValue;
            }
        }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion

        #region Methods
        private void RadioButtonGroupView_ChildrenReordered(object sender, EventArgs e)
        {
            UpdateAllEvent();
        }
        private void UpdateAllEvent()
        {
            foreach (var item in GetChildRadioButtons(this))
            {
                if (item is RadioButton rb)
                {
                    rb.Checked -= UpdateSelected;
                    rb.Checked += UpdateSelected;
                }
            }
        }
        private void OnChildAdded(object sender, ElementEventArgs e)
        {
            if (e.Element is RadioButton rb)
            {
                rb.Checked -= UpdateSelected;
                rb.Checked += UpdateSelected;
            }
            else if (e.Element is Layout<View> la)
            {
                la.ChildAdded -= OnChildAdded;
                la.ChildAdded += OnChildAdded;
                foreach (var radioButton in GetChildRadioButtons(la))
                {
                    radioButton.Checked -= UpdateSelected;
                    radioButton.Checked += UpdateSelected;
                }
            }
        }

        void UpdateToSelectedItem()
        {
            foreach (var rb in GetChildRadioButtons(this))
            {
                rb.IsChecked = rb.Value?.Equals(SelectedItem) ?? false;
            }
        }

        void UpdateToSelectedIndex()
        {
            int index = 0;
            foreach (var rb in GetChildRadioButtons(this))
            {
                rb.IsChecked = index == SelectedIndex;
                index++;
            }
        }

        void UpdateSelected(object selected, EventArgs e)
        {
            var asRadioButton = (RadioButton)selected;

            // if the selected item is checked, uncheck all others
            if (asRadioButton.IsChecked)
            {
                foreach (var rb in GetChildRadioButtons(this))
                {
                    if (rb != asRadioButton)
                    {
                        rb.IsChecked = false;
                    }
                }
                SetValue(SelectedItemProperty, asRadioButton.Value);
                var index = GetChildRadioButtons(this).ToList().IndexOf(asRadioButton);
                SetValue(SelectedIndexProperty, index);

                SelectedItemChanged?.Invoke(this, new EventArgs());
                SelectedItemChangedCommand?.Execute(CommandParameter);
            }
            ValidationChanged?.Invoke(this, new EventArgs());
        }
        private IEnumerable<RadioButton> GetChildRadioButtons(Layout<View> layout)
        {
            foreach (var view in layout.Children)
            {
                if (view is RadioButton)
                {
                    yield return view as RadioButton;
                }
                else if (view is Layout<View> la)
                {
                    foreach (var chk in GetChildRadioButtons(la))
                        yield return chk;
                }
            }
        }
        #endregion
    }
}
