using Plugin.InputKit.Shared.Abstraction;
using Plugin.InputKit.Shared.Configuration;
using Plugin.InputKit.Shared.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// a custom list for selections,
    /// </summary>
    public class SelectionView : Grid
    {
        /// <summary>
        /// Manages default values of selectionview
        /// </summary>
        public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
        {
            Color = Color.Accent,
            BackgroundColor = (Color)Button.BackgroundColorProperty.DefaultValue,
            BorderColor = (Color)Button.BorderColorProperty.DefaultValue,
            CornerRadius = 20,
            FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Button)),
            Size = -1,
            TextColor = (Color)Button.TextColorProperty.DefaultValue,
        };

        private IList _itemSource;
        private SelectionType _selectionType = SelectionType.Button;
        private IList _disabledSource;
        private int _columnNumber = 2;
        private Color _color = GlobalSetting.Color;
        private BindingBase _itemDisplayBinding;

        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SelectionView()
        {
            this.RowSpacing = 0;
            this.ColumnSpacing = 0;
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Selection Type, More types will be added later
        /// </summary>
        public SelectionType SelectionType { get => _selectionType; set { _selectionType = value; UpdateView(); } }
        ///----------------------------------------------------------
        /// <summary>
        /// Added later
        /// </summary>
        public string IsDisabledPropertyName { get; set; }
        ///----------------------------------------------------------
        /// <summary>
        /// Column of this view
        /// </summary>
        public int ColumnNumber { get => _columnNumber; set { _columnNumber = value; UpdateView(); } }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Disables these options. They can not be choosen
        /// </summary>
        public IList DisabledSource { get => _disabledSource; set { _disabledSource = value; UpdateView(); } }

        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Color of selections
        /// </summary>
        public Color Color { get => _color; set { _color = value; UpdateColor(); OnPropertyChanged(); } }

        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Items Source of selections
        /// </summary>
        public IList ItemsSource
        {
            get => _itemSource;
            set
            {
                _itemSource = value;
                UpdateEvents(value);
                UpdateView();
            }
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Sets or Gets SelectedItem of SelectionView
        /// </summary>
        public object SelectedItem
        {
            get
            {
                foreach (var item in this.Children)
                    if (item is ISelection && (item as ISelection).IsSelected)
                        return (item as ISelection).Value;
                return null;
            }
            set
            {
                foreach (var item in this.Children)
                    if (item is ISelection && !(item as ISelection).IsDisabled)
                        (item as ISelection).IsSelected = (item as ISelection).Value == value;
            }
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Sets or Gets SelectedItem of SelectionView
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                for (int i = 0; i < this.Children.Count; i++)
                {
                    if ((this.Children[i] as ISelection)?.IsSelected ?? false)
                        return i;
                }
                return -1;
            }
            set
            {
                for (int i = 0; i < this.Children.Count; i++)
                {
                    if (!(this.Children[i] as ISelection)?.IsDisabled ?? false)
                        (this.Children[i] as ISelection).IsSelected = i == value;
                }
            }
        }

        public IEnumerable<int> SelectedIndexes
        {
            get
            {
                for (int i = 0; i < this.Children.Count; i++)
                {
                    if (this.Children[i] is ISelection && (this.Children[i] as ISelection).IsSelected)
                        yield return i;
                }
            }
            set
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    if (this.Children[i] is ISelection)
                        (this.Children[i] as ISelection).IsSelected = value.Contains(i);
                }
            }
        }

        ///-----------------------------------------------------------------------------
        /// <summary>
        ///Selected Items for the multiple selections, 
        /// </summary>
        public IList SelectedItems
        {
            get
            {
                return this.Children.Where(w => (w is ISelection) && (w as ISelection).IsSelected)?.ToList();
            }
            set
            {
                foreach (var item in this.Children)
                    if (item is ISelection)
                        (item as ISelection).IsSelected = value.Contains((item as ISelection).Value);
            }
        }
        /// <summary>
        /// Changes all <see cref="SelectableButton.UnselectedColor"/> in <see cref="SelectionView"/>
        /// </summary>
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set
            {
                foreach (var item in this.Children)
                    if (item is SelectableButton)
                        (item as SelectableButton).UnselectedColor = value;
            }
        }
        private void UpdateEvents(IList value)
        {
            if (value is INotifyCollectionChanged)
            {
                (value as INotifyCollectionChanged).CollectionChanged -= MultiSelectionView_CollectionChanged;
                (value as INotifyCollectionChanged).CollectionChanged += MultiSelectionView_CollectionChanged;
            }
        }
        private void MultiSelectionView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateView();
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Updates views when data changed
        /// </summary>
        private void UpdateView()
        {
            if (ItemsSource == null) return;

            this.Children.Clear();
            SetValue(SelectedItemProperty, null);
            foreach (var item in ItemsSource)
            {
                try
                {
                    var _View = GetInstance(item);

                    SetTextBinding(_View);

                    (_View as ISelection).Clicked -= Element_Clicked;
                    (_View as ISelection).Clicked += Element_Clicked;

                    if (!String.IsNullOrEmpty(IsDisabledPropertyName)) //Sets if property Disabled
                        (_View as ISelection).IsDisabled = Convert.ToBoolean(item.GetType().GetProperty(IsDisabledPropertyName)?.GetValue(item) ?? false);
                    if (DisabledSource?.Contains(item) ?? false)
                        (_View as ISelection).IsDisabled = true;

                    this.Children.Add(_View, this.Children.Count % ColumnNumber, this.Children.Count / ColumnNumber);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }

            ChooseFirstIndex();
        }
        /// <summary>
        /// Finds first undisabled selection and sets selected
        /// </summary>
        private void ChooseFirstIndex()
        {
            for (int i = 0; i < this.Children.Count; i++)
            {
                if (!(this.Children[i] as ISelection)?.IsDisabled ?? false)
                {
                    this.SelectedIndex = i;
                    return;
                }
            }
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Updates colors of inside, when color property changed on runtime
        /// </summary>
        private void UpdateColor()
        {
            foreach (var item in this.Children)
            {
                SetInstanceColor(item, this.Color);
            }
        }
        private void Element_Clicked(object sender, EventArgs e)
        {
            if ((int)this.SelectionType % 2 == 0)
            {
                SetValue(SelectedItemsProperty, SelectedItems);
                SetValue(SelectedIndexesProperty, SelectedIndexes);
            }
            else
            {
                SelectedItem = (sender as ISelection).Value;
                SetValue(SelectedItemProperty, SelectedItem);
                SetValue(SelectedIndexProperty, SelectedIndex);
            }
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        ///         Gets or sets a binding that selects the property that will be displayed for each
        ///object in the list of items.
        /// </summary>
        public BindingBase ItemDisplayBinding { get => _itemDisplayBinding; set { _itemDisplayBinding = value; SetTextBindings(); } }

        private View GetInstance(object obj)
        {
            switch (SelectionType)
            {
                case SelectionType.Button:
                    var btn =new SelectableButton(obj, this.Color);
                    btn.UnselectedColor = this.BackgroundColor;
                    return btn;
                case SelectionType.RadioButton:
                    return new SelectableRadioButton(obj, this.Color);
                case SelectionType.CheckBox:
                    return new SelectableCheckBox(obj, this.Color);
            }
            return null;
        }

        private void SetTextBinding(View control)
        {
            if (ItemDisplayBinding == null) return;

            BindingBase _binding = new Binding((ItemDisplayBinding as Binding)?.Path, source: (control as ISelection)?.Value);
            switch (SelectionType)
            {
                case SelectionType.Button:
                    (control as SelectableButton).SetBinding(SelectableButton.TextProperty, _binding);
                    break;
                case SelectionType.RadioButton:
                    (control as SelectableRadioButton).SetBinding(SelectableRadioButton.TextProperty, _binding);
                    break;
                case SelectionType.CheckBox:
                    (control as SelectableCheckBox).SetBinding(SelectableCheckBox.TextProperty, _binding);
                    break;
            }
        }
        private void SetTextBindings()
        {
            if (ItemDisplayBinding == null) return;

            foreach (var item in this.Children)
            {
                SetTextBinding(item);
            }
        }

        private void SetInstanceColor(View view, Color color)
        {
            switch (SelectionType)
            {
                case SelectionType.Button:
                    {
                        if (view is Button)
                        {
                            (view as Button).BackgroundColor = color;
                        }
                    }
                    break;
                case SelectionType.RadioButton:
                    {
                        if (view is SelectableRadioButton)
                            (view as SelectableRadioButton).Color = color;
                    }
                    break;
                case SelectionType.CheckBox:
                    {
                        if (view is SelectableCheckBox)
                            (view as SelectableCheckBox).Color = color;
                    }
                    break;
            }
        }
        #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(SelectionView), null, propertyChanged: (bo, ov, nv) => (bo as SelectionView).ItemsSource = (IList)nv);
        public static readonly BindableProperty DisabledSourceProperty = BindableProperty.Create(nameof(DisabledSource), typeof(IList), typeof(SelectionView), null, propertyChanged: (bo, ov, nv) => (bo as SelectionView).DisabledSource = (IList)nv);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(SelectionView), null, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as SelectionView).SelectedItem = nv);
        public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems), typeof(IList), typeof(SelectionView), null, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as SelectionView).SelectedItems = (IList)nv);
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(SelectionView), -1, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as SelectionView).SelectedIndex = (int)nv);
        public static readonly BindableProperty SelectedIndexesProperty = BindableProperty.Create(nameof(SelectedIndexes), typeof(IEnumerable<int>), typeof(SelectionView), new int[0], BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as SelectionView).SelectedIndexes = (IEnumerable<int>)nv);
        public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SelectionView), SelectionView.GlobalSetting.BackgroundColor, propertyChanged: (bo, ov, nv) => (bo as SelectionView).BackgroundColor = (Color)nv);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion
    }

    /// <summary>
    /// Types of selectionlist
    /// </summary>
    public enum SelectionType
    {
        Button = 1,
        RadioButton = 3,
        CheckBox = 2,
    }

    /// <summary>
    /// A Button which ISelection Implemented
    /// </summary>
    internal class SelectableButton : Button, ISelection
    {
        private bool _isSelected = false;
        private object _value;
        private Color _selectionColor = Color.Accent;
        private Color _unselectedColor;

        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Default constructor
        /// </summary>
        public SelectableButton()
        {
            //this.Margin = new Thickness(0);
            UpdateColors();
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Generates with its value
        /// </summary>
        /// <param name="value">Value to keep</param>
        public SelectableButton(object value) : this()
        {
            this.Value = value;
            this.FontFamily = SelectionView.GlobalSetting.FontFamily;
            this.TextColor = SelectionView.GlobalSetting.TextColor;
            this.FontSize = SelectionView.GlobalSetting.FontSize;
            this.CornerRadius = (int)SelectionView.GlobalSetting.CornerRadius;
            this.BorderColor = SelectionView.GlobalSetting.BorderColor;
            this.UnselectedColor = SelectionView.GlobalSetting.BackgroundColor;
            this.BorderWidth = 2;
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Colored Constructor
        /// </summary>
        /// <param name="value"></param>
        /// <param name="selectionColor">Color of selected situation</param>
        public SelectableButton(object value, Color selectionColor) : this(value)
        {
            this.SelectedColor = selectionColor;
        }
        public Color UnselectedColor { get => _unselectedColor; set { _unselectedColor = value; UpdateColors(); } }
        public Color SelectedColor
        {
            get => _selectionColor;
            set
            {
                _selectionColor = value;
                UpdateColors();
            }
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// This button is selected or not
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; UpdateColors(); }
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Updates colors, Triggered when color property changed
        /// </summary>
        private void UpdateColors()
        {
            if (IsSelected)
            {
                this.BackgroundColor = SelectedColor;
                this.TextColor = SelectedColor.ToSurfaceColor();
            }
            else
            {
                this.BackgroundColor = UnselectedColor;
                this.TextColor = (Color)SelectionView.GlobalSetting.TextColor;
            }
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Value is stored on this control
        /// </summary>
        public object Value { get => _value; set { _value = value; this.Text = value?.ToString(); } }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// This button is disabled or not. Disabled buttons(if it's true) can not be choosen.
        /// </summary>
        public bool IsDisabled { get; set; } = false;
    }

    /// <summary>
    /// A Radio Button which ISelection Implemented
    /// </summary>
    internal class SelectableRadioButton : RadioButton, ISelection
    {
        private bool _isDisabled;
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SelectableRadioButton() { }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Constructor with value
        /// </summary>
        /// <param name="value">Value to keep</param>
        public SelectableRadioButton(object value)
        {
            this.Value = value;
            this.Text = value?.ToString();
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Colored Constructor
        /// </summary>
        public SelectableRadioButton(object value, Color color) : this(value)
        {
            this.Color = color;
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// ISelection interface property
        /// </summary>
        public bool IsSelected { get => this.IsChecked; set => this.IsChecked = value; }
    }

    /// <summary>
    /// A CheckBox which ISelection Implemented
    /// </summary>
    internal class SelectableCheckBox : CheckBox, ISelection
    {
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SelectableCheckBox()
        {
            this.Type = CheckType.Check;
            this.CheckChanged += (s, e) => this.Clicked?.Invoke(s, e);
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Constructor with Value
        /// </summary>
        /// <param name="value">Parameter too keep</param>
        public SelectableCheckBox(object value) : this()
        {
            this.Value = value;
            this.Text = value?.ToString();
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Constructor with Value
        /// </summary>
        /// <param name="value">Parameter too keep</param>
        /// <param name="color">Color of control</param>
        public SelectableCheckBox(object value, Color color) : this(value)
        {
            this.Color = color;
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Capsulated IsChecked
        /// </summary>
        public bool IsSelected { get => this.IsChecked; set => this.IsChecked = value; }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Parameter to keep
        /// </summary>
        public object Value { get; set; }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Triggers when CheckChanged
        /// </summary>
        public event EventHandler Clicked;
    }
}
