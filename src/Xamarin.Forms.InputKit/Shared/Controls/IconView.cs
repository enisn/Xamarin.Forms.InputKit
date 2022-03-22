using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// Default Constructor
    /// </summary>
    public partial class IconView : View
    {

        public static readonly BindableProperty FillColorProperty = BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(IconView), Color.Black);

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
    }
}
