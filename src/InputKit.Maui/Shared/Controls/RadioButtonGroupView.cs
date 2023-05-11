using InputKit.Shared.Abstraction;
using InputKit.Shared.Layouts;
using InputKit.Shared.Validations;
using System.Windows.Input;

namespace InputKit.Shared.Controls;

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
        Spacing = 10;
        RegisterEvents();
    }

	protected override void OnHandlerChanging(HandlerChangingEventArgs args)
	{
        UnregisterEvents();

        if (args.NewHandler is not null)
        {
            RegisterEvents();
        }
    }

	private void RegisterEvents()
	{
		ChildAdded += OnChildAdded;
		ChildrenReordered += RadioButtonGroupView_ChildrenReordered;
	}

	private void UnregisterEvents()
	{
		ChildAdded -= OnChildAdded;
		ChildrenReordered -= RadioButtonGroupView_ChildrenReordered;
	}

	//-----------------------------------------------------------------------------
	/// <summary>
	/// Invokes when tapped on RadioButon
	/// </summary>
	public event EventHandler SelectedItemChanged;

    //-----------------------------------------------------------------------------
    /// <summary>
    /// Executes when tapped on RadioButton
    /// </summary>
    public ICommand SelectedItemChangedCommand
    {
        get => (ICommand)GetValue(SelectedItemChangedCommandProperty);
        set => SetValue(SelectedItemChangedCommandProperty, value);
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
    /// Returns selected radio button's index from inside of this.
    /// </summary>
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }
    //-----------------------------------------------------------------------------
    /// <summary>
    /// Returns selected radio button's Value from inside of this.
    /// You can change the selectedItem too by sending a Value which matches ones of radio button's value
    /// </summary>
    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public List<IValidation> Validations { get; } = new();
    public bool IsValid => ValidationResults().All(x => x.isValid);
    protected IEnumerable<(bool isValid, string message)> ValidationResults()
    {
        foreach (var validation in Validations)
        {
            var validated = validation.Validate(this.SelectedItem ?? (SelectedIndex == -1 ? null : SelectedIndex));
            yield return new(validated, validation.Message);
        }
    }

    /// <summary>
    /// this will be added later
    /// </summary>
    public async void DisplayValidation()
    {
        if (!IsValid)
        {
            BackgroundColor = ValidationColor.WithAlpha(.6f);
            await Task.Delay(500);
            BackgroundColor = Colors.Transparent;
        }
    }


    public void ResetValidation()
    {
        BackgroundColor = Colors.Transparent;
    }

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
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(RadioButtonGroupView), null, propertyChanged: OnCommandParameterChanged, defaultBindingMode: BindingMode.TwoWay);

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
        else if (e.Element is Layout la)
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
            OnPropertyChanged(nameof(IsValid));
        }
    }

    private IEnumerable<RadioButton> GetChildRadioButtons(Layout layout)
    {
        foreach (var view in layout.Children)
        {
            if (view is RadioButton)
            {
                yield return view as RadioButton;
            }
            else if (view is Layout la)
            {
                foreach (var chk in GetChildRadioButtons(la))
                    yield return chk;
            }
        }
    }
    #endregion

    public Color ValidationColor
    {
        get => (Color)GetValue(ValidationColorProperty);
        set => SetValue(ValidationColorProperty, value);
    }

    public static readonly BindableProperty ValidationColorProperty = BindableProperty.Create(
      nameof(ValidationColor),
      typeof(Color),
      typeof(RadioButton),
      defaultValue: Colors.Red);
}
