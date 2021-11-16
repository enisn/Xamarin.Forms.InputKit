using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Platform;
using Plugin.InputKit.Platforms.iOS;
using Plugin.InputKit.Shared.Controls;
using UIKit;

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
