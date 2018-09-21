using Plugin.InputKit.Platforms.UWP;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(IconView), typeof(IconViewRenderer))]
namespace Plugin.InputKit.Platforms.UWP
{
    public class IconViewRenderer : ViewRenderer<IconView, BitmapIcon>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<IconView> e)
        {
            if (Control != null)
            {
                var brush = new ImageBrush();
                if (e?.NewElement != null)
                    brush.ImageSource = new BitmapImage(new Uri(e.NewElement.Source, UriKind.Relative));

                Control.Foreground = brush;
            }
            base.OnElementChanged(e);
        }
    }
}
