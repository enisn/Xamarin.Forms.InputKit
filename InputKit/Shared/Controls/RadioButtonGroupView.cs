using Plugin.InputKit.Shared.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// Groups radiobuttons, Inherited StackLayout.
    /// </summary>
    public partial class RadioButtonGroupView : StackLayout, IValidatable
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Default constructor of RadioButtonGroupView
        /// </summary>
        public RadioButtonGroupView()
        {
            this.ChildAdded += RadioButtonGroupView_ChildAdded;
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
        public ICommand SelectedItemChangedCommand { get; set; }
        /// <summary>
        /// Command Parameter will be sent in SelectedItemChangedCommand
        /// </summary>
        public object CommandParameter { get; set; }
        private void RadioButtonGroupView_ChildrenReordered(object sender, EventArgs e)
        {
            UpdateAllEvent();
        }
        private void UpdateAllEvent()
        {
            foreach (var item in this.Children)
            {
                if (item is RadioButton)
                {
                    (item as RadioButton).Clicked -= UpdateSelected;
                    (item as RadioButton).Clicked += UpdateSelected;
                }
            }
        }
        private void RadioButtonGroupView_ChildAdded(object sender, ElementEventArgs e)
        {
            if (e.Element is RadioButton)
            {
                (e.Element as RadioButton).Clicked -= UpdateSelected;
                (e.Element as RadioButton).Clicked += UpdateSelected;
            }
        }
        void UpdateSelected(object selected, EventArgs e)
        {
            foreach (var item in this.Children)
            {
                if (item is RadioButton)
                    (item as RadioButton).IsChecked = item.Equals(selected);
            }

            SetValue(SelectedItemProperty, this.SelectedItem);
            OnPropertyChanged(nameof(SelectedItem));
            SetValue(SelectedIndexProperty, this.SelectedIndex);
            OnPropertyChanged(nameof(SelectedIndex));
            SelectedItemChanged?.Invoke(this, new EventArgs());
            if (SelectedItemChangedCommand?.CanExecute(CommandParameter ?? this) ?? false)
                SelectedItemChangedCommand?.Execute(CommandParameter ?? this);
            ValidationChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// this will be added later
        /// </summary>
        public async void DisplayValidation()
        {
            this.BackgroundColor = Color.Red;
            await Task.Delay(500);
            this.BackgroundColor = Color.Transparent;
        }

        /// <summary>
        /// Returns selected radio button's index from inside of this.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                int index = 0;
                foreach (var item in this.Children)
                {
                    if (item is RadioButton)
                    {
                        if ((item as RadioButton).IsChecked)
                            return index;
                        index++;
                    }
                }
                return -1;
            }
            set
            {
                int index = 0;
                foreach (var item in this.Children)
                {
                    if (item is RadioButton)
                    {
                        (item as RadioButton).IsChecked = index == value;
                        index++;
                    }
                }
            }
        }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Returns selected radio button's Value from inside of this.
        /// You can change the selectedItem too by sending a Value which matches ones of radio button's value
        /// </summary>
        public object SelectedItem
        {
            get
            {
                foreach (var item in this.Children)
                {
                    if (item is RadioButton && (item as RadioButton).IsChecked)
                        return (item as RadioButton).Value;
                }
                return null;
            }
            set
            {
                foreach (var item in this.Children)
                {
                    if (item is RadioButton)
                        (item as RadioButton).IsChecked = (item as RadioButton).Value.Equals(value);
                }
            }
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
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(RadioButtonGroupView), null, propertyChanged: (bo, ov, nv) => (bo as RadioButtonGroupView).SelectedItem = nv);
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(RadioButtonGroupView), -1, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as RadioButtonGroupView).SelectedIndex = (int)nv);
        public static readonly BindableProperty SelectedItemChangedCommandProperty = BindableProperty.Create(nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(RadioButtonGroupView), null, propertyChanged: (bo, ov, nv) => (bo as RadioButtonGroupView).SelectedItemChangedCommand = (ICommand)nv);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion
    }
}
