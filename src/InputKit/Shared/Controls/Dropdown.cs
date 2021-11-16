using InputKit.Shared.Abstraction;
using InputKit.Shared.Configuration;
using InputKit.Shared.Layouts;
using InputKit.Shared.Utils;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections;
using System.Collections.Specialized;

namespace InputKit.Shared.Controls;

/// <summary>
/// A dropdown picker
/// </summary>
public partial class Dropdown : StatefulStackLayout, IValidatable
{
    public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
    {
        BackgroundColor = Colors.White,
        CornerRadius = 20,
        BorderColor = (Color)Button.BorderColorProperty.DefaultValue,
        Color = InputKitOptions.GetAccentColor(),
        FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Button)),
        Size = -1,
        TextColor = Colors.Black,
    };

    #region Constants
    public const string RESOURCE_ARROWDOWN = "InputKit.Shared.Resources.arrow_down.png";
    #endregion

    protected IconView imgIcon = new IconView { InputTransparent = true, FillColor = GlobalSetting.Color, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(10, 5, 5, 5) };
    protected IconView imgArrow = new IconView { InputTransparent = true, FillColor = GlobalSetting.Color, Source = ImageSource.FromResource(RESOURCE_ARROWDOWN), HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(5, 5, 10, 5), WidthRequest = 15, HeightRequest = 15 };
    protected Label lblTitle = new Label { Margin = new Thickness(6, 0, 0, 0), IsVisible = false, TextColor = GlobalSetting.TextColor, LineBreakMode = LineBreakMode.TailTruncation, FontFamily = GlobalSetting.FontFamily };
    protected Label lblAnnotation = new Label { Margin = new Thickness(6, 0, 0, 0), IsVisible = false, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), Opacity = 0.8, TextColor = GlobalSetting.TextColor, FontFamily = GlobalSetting.FontFamily };
    protected Frame frmBackground = new Frame { Padding = 0, BackgroundColor = GlobalSetting.BackgroundColor, HasShadow = false, CornerRadius = (int)GlobalSetting.CornerRadius, BorderColor = GlobalSetting.BorderColor };
    protected Entry txtInput = new EmptyEntry { TextColor = Colors.Blue, PlaceholderColor = Colors.Blue, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, FontFamily = GlobalSetting.FontFamily, IsEnabled = false };

    private protected PopupMenu pMenu = new PopupMenu();
    private string _placeholder;
    private string _validationMessage;
    private bool _isRequired;

    public Dropdown()
    {
        Children.Add(lblTitle);
        Children.Add(lblAnnotation);
        frmBackground.Content = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Children =
                        {
                            imgIcon,
                            txtInput,
                            imgArrow
                        }
        };
        Children.Add(frmBackground);
        frmBackground.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(Menu_Requested), CommandParameter = frmBackground });
        txtInput.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(Menu_Requested), CommandParameter = txtInput });

        //+= Menu_Requested;
        pMenu.OnItemSelected += Menu_Item_Selected;
        txtInput.TextChanged += (s, args) => Text = args.NewTextValue;
        UpdateMainText();
    }

    public event EventHandler ValidationChanged;

    public event EventHandler<SelectedItemChangedArgs> SelectedItemChanged;

    #region SelectionRegion
    private void Menu_Requested(object sender, EventArgs e)
    {
        ShowMenu();
    }

    private void ShowMenu() => pMenu.ShowPopup(imgArrow);

    private void Menu_Requested(object obj)
    {
        if (obj != txtInput || !IsEditable)
            ShowMenu();
        else
        {
            txtInput.Focus();
        }
    }

    private void Menu_Item_Selected(string item, int index)
    {
        try
        {
            SelectedItem = ItemsSource[index];
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
    }

    public IList ItemsSource
    {
        get => pMenu.ItemsSource;
        set
        {
            pMenu.ItemsSource = value;
            if (value is INotifyCollectionChanged)
                (value as INotifyCollectionChanged).CollectionChanged += Dropdown_CollectionChanged;
        }
    }

    public object SelectedItem { get => GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }

    private void Dropdown_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        pMenu.ItemsSource = (IList)GetValue(ItemsSourceProperty);
    }
    private void UpdateSelected()
    {
        UpdateMainText();
        DisplayValidation();
        ValidationChanged?.Invoke(this, new EventArgs());
        SelectedItemChanged?.Invoke(this, new SelectedItemChangedArgs(SelectedItem, ItemsSource?.IndexOf(SelectedItem) ?? -1));
    }
    #endregion

    public string Title { get => lblTitle.Text; set { lblTitle.Text = value; lblTitle.IsVisible = !string.IsNullOrEmpty(value); } }

    [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
    public double TitleFontSize { get => lblTitle.FontSize; set => lblTitle.FontSize = value; }

    [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
    public double FontSize { get => txtInput.FontSize; set => txtInput.FontSize = value; }

    public ImageSource IconImage { get => imgIcon.Source; set => imgIcon.Source = value; }

    public ImageSource ArrowImage { get => GetValue(ArrowImageProperty) as ImageSource; set => SetValue(ArrowImageProperty, value); }

    public string FontFamily { get => txtInput.FontFamily; set { txtInput.FontFamily = value; lblTitle.FontFamily = value; lblAnnotation.FontFamily = value; } }

    public new Color BackgroundColor { get => frmBackground.BackgroundColor; set => frmBackground.BackgroundColor = value; }

    public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }

    public Color AnnotationColor { get => lblAnnotation.TextColor; set => lblAnnotation.TextColor = value; }

    public Color TitleColor { get => lblTitle.TextColor; set => lblTitle.TextColor = value; }

    public Color BorderColor { get => frmBackground.BorderColor; set { frmBackground.BorderColor = value; } }

    public float CornerRadius { get => frmBackground.CornerRadius; set => frmBackground.CornerRadius = value; }

    public string Placeholder { get => _placeholder; set { _placeholder = value; UpdateMainText(); } }

    public Color PlaceholderColor { get => (Color)GetValue(PlaceholderColorProperty); set => SetValue(PlaceholderColorProperty, value); }

    public bool IsRequired { get => _isRequired; set { _isRequired = value; DisplayValidation(); } }

    public bool IsValidated => !IsRequired || SelectedItem != null;

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

    public bool IsEditable { get => txtInput.IsEnabled; set => txtInput.IsEnabled = value; }

    public string ValidationMessage { get => _validationMessage; set { _validationMessage = value; DisplayValidation(); } }

    private void UpdateColors()
    {
        imgIcon.FillColor = Color;
        imgArrow.FillColor = Color;
    }

    private void UpdateMainText()
    {
        txtInput.Text = SelectedItem == null ? Placeholder : SelectedItem.ToString();
        txtInput.TextColor = SelectedItem == null ? PlaceholderColor : TextColor;
    }

    public void DisplayValidation()
    {
        lblAnnotation.Text = IsValidated ? null : ValidationMessage;
        lblAnnotation.IsVisible = !string.IsNullOrEmpty(lblAnnotation.Text);
    }

    #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(Dropdown), propertyChanged: (bo, ov, nv) => (bo as Dropdown).ItemsSource = (IList)nv);
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(Dropdown), null, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as Dropdown).UpdateSelected());
    public new static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(Dropdown), Colors.White, propertyChanged: (bo, ov, nv) => (bo as Dropdown).BackgroundColor = (Color)nv);
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(Dropdown), GlobalSetting.Color, propertyChanged: (bo, ov, nv) => (bo as Dropdown).UpdateColors());
    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Dropdown), GlobalSetting.TextColor, propertyChanged: (bo, ov, nv) => (bo as Dropdown).UpdateMainText());
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(Dropdown), propertyChanged: (bo, ov, nv) => (bo as Dropdown).Title = (string)nv);
    public static readonly BindableProperty IconImageProperty = BindableProperty.Create(nameof(IconImage), typeof(ImageSource), typeof(Dropdown), propertyChanged: (bo, ov, nv) => (bo as Dropdown).IconImage = (ImageSource)nv);
    public static readonly BindableProperty AnnotationColorProperty = BindableProperty.Create(nameof(AnnotationColor), typeof(Color), typeof(Dropdown), GlobalSetting.Color, propertyChanged: (bo, ov, nv) => (bo as Dropdown).AnnotationColor = (Color)nv);
    public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(Dropdown), GlobalSetting.TextColor, propertyChanged: (bo, ov, nv) => (bo as Dropdown).TitleColor = (Color)nv);
    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(Dropdown), GlobalSetting.BorderColor, propertyChanged: (bo, ov, nv) => (bo as Dropdown).BorderColor = (Color)nv);
    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(Dropdown), null, propertyChanged: (bo, ov, nv) => (bo as Dropdown).Placeholder = (string)nv);
    public static readonly BindableProperty IsRequiredProperty = BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(Dropdown), false, propertyChanged: (bo, ov, nv) => (bo as Dropdown).IsRequired = (bool)nv);
    public static readonly BindableProperty ValidationMessageProperty = BindableProperty.Create(nameof(ValidationMessage), typeof(string), typeof(Dropdown), null, propertyChanged: (bo, ov, nv) => (bo as Dropdown).ValidationMessage = (string)nv);
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(TextProperty), typeof(string), typeof(Dropdown), null, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as Dropdown).txtInput.Text = (string)nv);
    public static readonly BindableProperty IsEditableProperty = BindableProperty.Create(nameof(IsEditable), typeof(bool), typeof(Dropdown), false, propertyChanged: (bo, ov, nv) => (bo as Dropdown).IsEditable = (bool)nv);
    public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(Dropdown), Colors.LightGray, propertyChanged: (bo, ov, nv) => { (bo as Dropdown).txtInput.PlaceholderColor = (Color)nv; (bo as Dropdown).UpdateMainText(); });
    public static readonly BindableProperty ArrowImageProperty = BindableProperty.Create(nameof(ArrowImage), typeof(ImageSource), typeof(Dropdown), ImageSource.FromResource(RESOURCE_ARROWDOWN), propertyChanged: (bo, ov, nv) => (bo as Dropdown).imgArrow.Source = (ImageSource)nv);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    #endregion
}
