using InputKit.Shared.Abstraction;
using System.ComponentModel;
using System.Windows.Input;

namespace InputKit.Shared.Controls;

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
		RegisterChildrenEvents();
		ChildAdded += FormView_ChildAdded;
		ChildRemoved += FormView_ChildRemoved;
	}
	
	private void UnregisterEvents()
	{
        UnregisterChildrenEvents();
		ChildAdded -= FormView_ChildAdded;
		ChildRemoved -= FormView_ChildRemoved;
	}
    
	private void FormView_ChildRemoved(object sender, ElementEventArgs e)
    {
        if (e is IValidatable validatable)
        {
            validatable.PropertyChanged -= OnValidatablePropertyChanged;
        }
    }

    private void FormView_ChildAdded(object s, ElementEventArgs e)
    {
        if (e.Element is IValidatable || e.Element is Button)
        {
            RegisterChildEvent(e.Element);
        }
        else if (e.Element is Layout layout)
        {
            layout.ChildAdded -= FormView_ChildAdded;
            layout.ChildAdded += FormView_ChildAdded;
            layout.ChildRemoved -= FormView_ChildRemoved;
            layout.ChildRemoved += FormView_ChildRemoved;

            foreach (var child in GetChildValitablesAndButtons(layout))
            {
                RegisterChildEvent(child);
            }
        }
    }

    void RegisterChildrenEvents()
    {
        foreach (var item in GetChildValitablesAndButtons(this))
        {
            RegisterChildEvent(item);
        }
    }
	
    void UnregisterChildrenEvents()
    {
        foreach (var item in GetChildValitablesAndButtons(this))
        {
            UnregisterChildEvent(item);
        }
    }

    void RegisterChildEvent(BindableObject bindable)
    {
        if (bindable == null)
        {
            return;
        }

        bindable.PropertyChanged -= OnValidatablePropertyChanged;
        bindable.PropertyChanged += OnValidatablePropertyChanged;

        if (GetIsSubmitButton(bindable) && bindable is View view)
        {
            if(view is Button btn)
            {
                btn.Clicked -= SubmitButtonClicked;
                btn.Clicked += SubmitButtonClicked;
                return;
            }

            if (view is ImageButton imgBtn)
            {
                imgBtn.Clicked -= SubmitButtonClicked;
                imgBtn.Clicked += SubmitButtonClicked;
                return;
            }

            var tapGestureRecognizer = view.GestureRecognizers.FirstOrDefault(x => x is TapGestureRecognizer) as TapGestureRecognizer;

            if (tapGestureRecognizer == null)
            {
                tapGestureRecognizer = new TapGestureRecognizer();
                view.GestureRecognizers.Add(tapGestureRecognizer);
            }

            tapGestureRecognizer.Tapped -= SubmitButtonClicked;
            tapGestureRecognizer.Tapped += SubmitButtonClicked;
        }
    }
    void UnregisterChildEvent(BindableObject bindable)
    {
        if (bindable == null)
        {
            return;
        }

        bindable.PropertyChanged -= OnValidatablePropertyChanged;

        if (GetIsSubmitButton(bindable) && bindable is View view)
        {
            var tapGestureRecognizer = view.GestureRecognizers.FirstOrDefault(x => x is TapGestureRecognizer) as TapGestureRecognizer;

            if (tapGestureRecognizer is not null)
            {
                tapGestureRecognizer.Tapped -= SubmitButtonClicked;
            }
        }
    }

    private void SubmitButtonClicked(object sender, EventArgs e)
    {
        if (CheckValidation(this))
        {
            SubmitCommand?.Execute(IsValidated);
        }

        if (!IsValidated)
        {
            foreach (var child in GetChildValitablesAndButtons(this))
            {
                if (child is IValidatable validatable)
                {
                    validatable.DisplayValidation();
                }
            }
        }
    }

    private void OnValidatablePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IValidatable.IsValid))
        {
            FormView_ValidationChanged(sender as IValidatable);
        }
    }

    private void FormView_ValidationChanged(IValidatable validatable)
    {
        if (validatable.IsValid)
        {
            SetValue(IsValidatedProperty, CheckValidation(this));
        }
        else
        {
            SetValue(IsValidatedProperty, false);
        }
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
        foreach (var item in GetChildValitablesAndButtons(view))
        {
            if (item is IValidatable validatable && !validatable.IsValid)
            {
                return false;
            }
        }

        return true;
    }

    static IEnumerable<BindableObject> GetChildValitablesAndButtons(Layout layout)
    {
        foreach (View item in layout.Children)
        {
            if (item is IValidatable)
            {
                yield return item;
            }
            else if (item is Layout la)
            {
                foreach (var child in GetChildValitablesAndButtons(la))
                {
                    yield return child;
                }
            }
            else if (item is View view && GetIsSubmitButton(view))
            {
                yield return view;
            }
        }
    }

    public static readonly BindableProperty IsValidatedProperty = BindableProperty.Create(
        nameof(IsValidated),
        typeof(bool),
        typeof(FormView),
        false,
        BindingMode.OneWayToSource);

    public ICommand SubmitCommand { get => (ICommand)GetValue(SubmitCommandProperty); set => SetValue(SubmitCommandProperty, value); }

    public static readonly BindableProperty SubmitCommandProperty =
        BindableProperty.Create(nameof(SubmitCommand), typeof(ICommand), typeof(FormView));

    public static readonly BindableProperty IsSubmitButtonProperty =
        BindableProperty.CreateAttached("IsSubmitButton", typeof(bool), typeof(Button), false);

    public static bool GetIsSubmitButton(BindableObject view)
    {
        return (bool)view.GetValue(IsSubmitButtonProperty);
    }

    public static void SetIsSubmitButton(BindableObject view, bool value)
    {
        view.SetValue(IsSubmitButtonProperty, value);
    }
}
