using InputKit.Shared.Abstraction;
using InputKit.Shared.Configuration;
using InputKit.Shared.Helpers;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace InputKit.Shared.Controls;

/// <summary>
/// a custom list for selections,
/// </summary>
public partial class SelectionView : Grid
{
    /// <summary>
    /// Manages default values of selectionview
    /// </summary>
    public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
    {
        Color = InputKitOptions.GetAccentColor(),
        BackgroundColor = Colors.LightGray,
        BorderColor = (Color)Button.BorderColorProperty.DefaultValue,
        CornerRadius = 20,
        FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Button)),
        Size = -1,
        TextColor = (Color)Label.TextColorProperty.DefaultValue,
        LabelPosition = LabelPosition.After
    };

    #region Fields
    private SelectionType _selectionType = SelectionType.Button;
    private int _columnNumber = 2;
    private BindingBase _itemDisplayBinding;
    private int _selectedIndex = 0;
    #endregion

    /// <summary>
    /// Default Constructor
    /// </summary>
    public SelectionView()
    {
        RowSpacing = 0;
        ColumnSpacing = 0;
    }

    #region Properties

    /// <summary>
    /// Selection Type, More types will be added later
    /// </summary>
    public SelectionType SelectionType { get => _selectionType; set { _selectionType = value; UpdateView(); } }

    /// <summary>
    /// Added later
    /// </summary>
    public string IsDisabledPropertyName { get; set; } = "IsDisabled";

    /// <summary>
    /// Column of this view
    /// </summary>
    public int ColumnNumber { get => _columnNumber; set { _columnNumber = value; UpdateView(); } }

    /// <summary>
    /// Disables these options. They can not be choosen
    /// </summary>
    public IList DisabledSource { get => (IList)GetValue(DisabledSourceProperty); set => SetValue(DisabledSourceProperty, value); }

    /// <summary>
    /// Color of selections
    /// </summary>
    public Color Color { get => (Color)GetValue(ColorProperty); set { SetValue(ColorProperty, value); OnPropertyChanged(); } }

    /// <summary>
    /// Items Source of selections
    /// </summary>
    public IList ItemsSource { get => (IList)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }

    /// <summary>
    /// Sets or Gets SelectedItem of SelectionView
    /// </summary>
    public object SelectedItem { get => GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }

    /// <summary>
    /// Sets or Gets SelectedItem of SelectionView
    /// </summary>
    public int SelectedIndex
    {
        get
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if ((Children[i] as ISelection)?.IsSelected ?? false)
                    return i;
            }
            return -1;
        }
        set
        {
            _selectedIndex = value;
            for (int i = 0; i < Children.Count; i++)
            {
                if (!(Children[i] as ISelection)?.IsDisabled ?? false)
                    (Children[i] as ISelection).IsSelected = i == value;
            }
        }
    }

    /// <summary>
    /// Gets and Sets selected items with indexes.
    /// </summary>
    public IEnumerable<int> SelectedIndexes
    {
        get
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i] is ISelection && (Children[i] as ISelection).IsSelected)
                    yield return i;
            }
        }
        set
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i] is ISelection)
                    (Children[i] as ISelection).IsSelected = value.Contains(i);
            }
        }
    }

    /// <summary>
    /// Selected Items for the multiple selections.
    /// </summary>
    public IList SelectedItems { get => (IList)GetValue(SelectedItemsProperty); set => SetValue(SelectedItemsProperty, value); }

    /// <summary>
    /// Changes all <see cref="SelectableButton.UnselectedColor"/> in <see cref="SelectionView"/>
    /// </summary>
    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set
        {
            foreach (var item in Children)
                if (item is SelectableButton)
                    (item as SelectableButton).UnselectedColor = value;
        }
    }

    /// <summary>
    /// Gets or sets the label position.
    /// </summary>
    public LabelPosition LabelPosition
    {
        get => (LabelPosition)GetValue(LabelPositionProperty);
        set => SetValue(LabelPositionProperty, value);
    }

    /// <summary>
    ///         Gets or sets a binding that selects the property that will be displayed for each
    ///object in the list of items.
    /// </summary>
    public BindingBase ItemDisplayBinding { get => _itemDisplayBinding; set { _itemDisplayBinding = value; SetTextBindings(); } }
    #endregion

    #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(SelectionView), null, propertyChanged: (bo, ov, nv) => (bo as SelectionView).SetItemsSource((IList)nv));
    public static readonly BindableProperty DisabledSourceProperty = BindableProperty.Create(nameof(DisabledSource), typeof(IList), typeof(SelectionView), null, propertyChanged: (bo, ov, nv) => (bo as SelectionView).UpdateView());
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(SelectionView), null, BindingMode.TwoWay,
        propertyChanged: (bo, ov, nv) =>
        {
            if (ov != nv)
            {
                (bo as SelectionView).SetSelectedItem(nv);
            }
        });
    public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems), typeof(IList), typeof(SelectionView), null, BindingMode.TwoWay,
        propertyChanged: (bo, ov, nv) => (bo as SelectionView).SetSelectedItems((IList)nv));
    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(SelectionView), -1, BindingMode.TwoWay,
        propertyChanged: (bo, ov, nv) =>
        {
            if (ov != nv)
            {
                (bo as SelectionView).SelectedIndex = (int)nv;
            }
        });
    public static readonly BindableProperty SelectedIndexesProperty = BindableProperty.Create(nameof(SelectedIndexes), typeof(IEnumerable<int>), typeof(SelectionView), new int[0], BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as SelectionView).SelectedIndexes = (IEnumerable<int>)nv);
    public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SelectionView), GlobalSetting.BackgroundColor, propertyChanged: (bo, ov, nv) => (bo as SelectionView).BackgroundColor = (Color)nv);
    public static readonly BindableProperty LabelPositionProperty = BindableProperty.Create(
        propertyName: nameof(LabelPosition), declaringType: typeof(SelectionView),
        returnType: typeof(LabelPosition), defaultBindingMode: BindingMode.TwoWay,
        defaultValue: GlobalSetting.LabelPosition,
        propertyChanged: (bo, ov, nv) => (bo as SelectionView).UpdateView());

    public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color),
        typeof(Color), typeof(SelectionView), defaultValue: GlobalSetting.Color, propertyChanged: (bo, ov, nv) => (bo as SelectionView).UpdateColor());
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    #endregion

    #region Methods
    private void UpdateEvents(IList value)
    {
        if (value is INotifyCollectionChanged notifyCollectionChanged)
        {
            notifyCollectionChanged.CollectionChanged -= MultiSelectionView_CollectionChanged;
            notifyCollectionChanged.CollectionChanged += MultiSelectionView_CollectionChanged;
        }
    }
    private void MultiSelectionView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateView();
    }

    /// <summary>
    /// Updates views when data changed
    /// </summary>
    private void UpdateView()
    {
        if (ItemsSource == null) return;

        Children.Clear();

        SetRowAndColumnDefinitions();

        SetValue(SelectedItemProperty, null);
        foreach (var item in ItemsSource)
        {
            try
            {
                var _View = GetInstance(item);

                SetTextBinding(_View as View);

                _View.Clicked -= Element_Clicked;
                _View.Clicked += Element_Clicked;

                if (!string.IsNullOrEmpty(IsDisabledPropertyName)) //Sets if property Disabled
                    _View.IsDisabled = Convert.ToBoolean(item.GetType().GetProperty(IsDisabledPropertyName)?.GetValue(item) ?? false);
                if (DisabledSource?.Contains(item) ?? false)
                    _View.IsDisabled = true;

                var addedView = _View as View;
                var column = Children.Count % ColumnNumber;
                var row = Children.Count / ColumnNumber;

                this.Add(addedView, column, row);

                _View.IsSelected = Children.Count == _selectedIndex; //to keep selected index when content is changed
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }

    protected void SetRowAndColumnDefinitions()
    {
        this.ColumnDefinitions.Clear();
        for (int i = 0; i < ColumnNumber; i++)
        {
            this.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        this.RowDefinitions.Clear();
        for (int i = 0; i < ItemsSource.Count / ColumnNumber; i++)
        {
            this.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        }
    }
    /// <summary>
    /// Updates colors of inside, when color property changed on runtime
    /// </summary>
    private void UpdateColor()
    {
        foreach (var item in Children)
        {
            SetInstanceColor(item, Color);
        }
    }

    private void Element_Clicked(object sender, EventArgs e)
    {
        if ((int)SelectionType % 2 == 0)
        {
            if (sender is ISelection selection)
            {
                if (selection.IsSelected)
                {
                    if (SelectedItems == null)
                        SelectedItems = Activator.CreateInstance(typeof(ObservableCollection<>).MakeGenericType(selection.Value.GetType())) as IList;

                    SelectedItems.Add(selection.Value);
                }
                else
                {
                    SelectedItems.Remove(selection.Value);
                }
            }

            var query = Children.Where(x => x is ISelection s && s.IsSelected);

            var selectedIndexes = query
                                    .Select(s => ItemsSource.IndexOf((s as ISelection).Value))
                                    .ToList();

            SetValue(SelectedIndexesProperty, selectedIndexes);
        }
        else
        {
            if (sender is ISelection selection && selection.IsSelected)
            {
                SelectedItem = selection.Value;
                SetValue(SelectedIndexProperty, ItemsSource.IndexOf(selection.Value));
            }
        }
    }

    public virtual ISelection GetInstance(object obj)
    {
        switch (SelectionType)
        {
            case SelectionType.Button:
            case SelectionType.MultipleButton:
                var btn = new SelectableButton(obj, this)
                {
                    UnselectedColor = BackgroundColor,
                    CanChangeSelectedState = SelectionType == SelectionType.MultipleButton
                };
                return btn;
            case SelectionType.MultipleRadioButton:
            case SelectionType.RadioButton:
                var rb = new SelectableRadioButton(obj)
                {
                    LabelPosition = LabelPosition
                };
                return rb;
            case SelectionType.CheckBox:
            case SelectionType.SingleCheckBox:
                var cb = new SelectableCheckBox(obj)
                {
                    LabelPosition = LabelPosition
                };
                return cb;
        }
        return null;
    }

    private void SetTextBinding(IView control)
    {
        if (ItemDisplayBinding == null) return;

        BindingBase _binding = new Binding((ItemDisplayBinding as Binding)?.Path, source: (control as ISelection)?.Value);
        switch (SelectionType)
        {
            case SelectionType.Button:
            case SelectionType.MultipleButton:
                (control as SelectableButton).SetBinding(Button.TextProperty, _binding);
                break;
            case SelectionType.RadioButton:
                (control as SelectableRadioButton).SetBinding(RadioButton.TextProperty, _binding);
                break;
            case SelectionType.CheckBox:
            case SelectionType.SingleCheckBox:
                (control as SelectableCheckBox).SetBinding(CheckBox.TextProperty, _binding);
                break;
        }
    }
    private void SetTextBindings()
    {
        if (ItemDisplayBinding == null) return;

        foreach (var item in Children)
        {
            SetTextBinding(item);
        }
    }

    private void SetInstanceColor(IView view, Color color)
    {
        switch (SelectionType)
        {
            case SelectionType.Button:
            case SelectionType.MultipleButton:
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

    private void SetSelectedItem(object value)
    {
        foreach (var item in Children)
            if (item is ISelection selection && !selection.IsDisabled)
                selection.IsSelected = selection.Value.Equals(value);
    }
    private void SetSelectedItems(IList value)
    {
        foreach (var item in Children)
        {
            if (item is ISelection selection && value != null)
            {

                selection.IsSelected = value.Contains(selection.Value);
            }
        }

        if (value is INotifyCollectionChanged observable)
            observable.CollectionChanged += SelectedItemsChanged;
    }

    private void SelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var item in e.NewItems)
                    (Children.FirstOrDefault(x => x is ISelection s && s.Value.Equals(item)) as ISelection).IsSelected = true;
                break;
            case NotifyCollectionChangedAction.Remove:
                foreach (var item in e.OldItems)
                    (Children.FirstOrDefault(x => x is ISelection s && s.Value.Equals(item)) as ISelection).IsSelected = false;

                break;
        }
    }

    private void SetItemsSource(IList value)
    {
        UpdateEvents(value);
        UpdateView();
    }
    #endregion

    #region Nested Classes

    /// <summary>
    /// A Button which ISelection Implemented
    /// </summary>
    public class SelectableButton : Button, ISelection
    {
        private bool _isSelected = false;
        private object _value;
        private Color _selectionColor = InputKitOptions.GetAccentColor();
        private Color _unselectedColor;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SelectableButton()
        {
            //this.Margin = new Thickness(0);
            UpdateColors();
        }

        /// <summary>
        /// Generates with its value
        /// </summary>
        /// <param name="value">Value to keep</param>
        public SelectableButton(object value) : this()
        {
            Value = value;
            FontFamily = GlobalSetting.FontFamily;
            TextColor = GlobalSetting.TextColor;
            FontSize = GlobalSetting.FontSize;
            CornerRadius = (int)GlobalSetting.CornerRadius;
            BorderColor = GlobalSetting.BorderColor;
            UnselectedColor = GlobalSetting.BackgroundColor;
            BorderWidth = 2;
            Clicked += (s, args) => UpdateSelection();
        }

        /// <summary>
        /// Colored Constructor
        /// </summary>
        /// <param name="value"></param>
        /// <param name="selectionColor">Color of selected situation</param>
        public SelectableButton(object value, SelectionView parent) : this(value)
        {
            SelectedColor = parent.Color;
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

        /// <summary>
        /// This button is selected or not
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {

                if (IsDisabled)
                {
                    return;
                }

                _isSelected = value;
                UpdateColors();
            }
        }

        /// <summary>
        /// Updates colors, Triggered when color property changed
        /// </summary>
        private void UpdateColors()
        {
            if (IsSelected)
            {
                BackgroundColor = SelectedColor;
                TextColor = SelectedColor.ToSurfaceColor();
            }
            else
            {
                BackgroundColor = UnselectedColor;
                TextColor = GlobalSetting.TextColor ?? UnselectedColor?.ToSurfaceColor();
            }
        }

        /// <summary>
        /// Value is stored on this control
        /// </summary>
        public object Value { get => _value; set { _value = value; Text = value?.ToString(); } }

        /// <summary>
        /// This button is disabled or not. Disabled buttons(if it's true) can not be choosen.
        /// Default Value: <see langword="false"/>
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Defines Can Selected State Change by itself
        /// </summary>
        public bool CanChangeSelectedState { get; set; }

        private void UpdateSelection()
        {
            IsSelected = !IsSelected;
        }
    }

    /// <summary>
    /// A Radio Button which ISelection Implemented
    /// </summary>
    public class SelectableRadioButton : RadioButton, ISelection
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SelectableRadioButton() { }

        /// <summary>
        /// Constructor with value
        /// </summary>
        /// <param name="value">Value to keep</param>
        public SelectableRadioButton(object value)
        {
            Value = value;
            Text = value?.ToString();
        }

        /// <summary>
        /// Colored Constructor
        /// </summary>
        public SelectableRadioButton(object value, SelectionView parent) : this(value)
        {
            Color = parent.Color;
        }

        /// <summary>
        /// ISelection interface property
        /// </summary>
        public bool IsSelected
        {
            get => IsChecked; set
            {
                if (IsDisabled)
                {
                    return;
                }

                if (IsChecked != value)
                {
                    IsChecked = value;
                }
            }
        }
    }

    /// <summary>
    /// A CheckBox which ISelection Implemented
    /// </summary>
    public class SelectableCheckBox : CheckBox, ISelection
    {

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SelectableCheckBox()
        {
            CheckChanged += (s, e) => Clicked?.Invoke(s, e);
        }

        /// <summary>
        /// Constructor with Value
        /// </summary>
        /// <param name="value">Parameter too keep</param>
        public SelectableCheckBox(object value) : this()
        {
            Value = value;
            Text = value?.ToString();
        }

        /// <summary>
        /// Capsulated IsChecked
        /// </summary>
        public bool IsSelected
        {
            get => IsChecked; set
            {
                if (IsDisabled)
                {
                    return;
                }

                if (IsChecked != value)
                {
                    IsChecked = value;
                }
            }
        }

        /// <summary>
        /// Parameter to keep
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Triggers when CheckChanged
        /// </summary>
        public event EventHandler Clicked;
    }
    #endregion
}
