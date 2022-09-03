using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace InputKit.Shared.Controls;

[Obsolete("IconView is deprecated. It won't be part of this library anymore. Please change your icons to FontImage or equivalent.")]
public partial class IconView : View, IIconView
{
    public IconView()
    {
    }

    public static readonly BindableProperty FillColorProperty = BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(IconView), Colors.Black);

    public Color FillColor
    {
        get
        {
            return (Color)GetValue(FillColorProperty);
        }
        set
        {
            SetValue(FillColorProperty, value);
        }
    }

    public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(IconView), default(ImageSource));
    public ImageSource Source
    {
        get
        {
            return (ImageSource)GetValue(SourceProperty);
        }
        set
        {
            SetValue(SourceProperty, value);
        }
    }

    IImageSource IImageSourcePart.Source => Source;

    public bool IsAnimationPlaying { get; }

    public void UpdateIsLoading(bool isLoading)
    {
        //throw new NotImplementedException();
    }
}
