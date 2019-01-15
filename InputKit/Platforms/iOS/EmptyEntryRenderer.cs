using Plugin.InputKit.Platforms.iOS;
using Plugin.InputKit.Shared.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(EmptyEntry), typeof(EmptyEntryRenderer))]

namespace Plugin.InputKit.Platforms.iOS
{
    public class EmptyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null && e.NewElement != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.TextColor = Element.TextColor.ToUIColor();
            }
        }
    }
}
