using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Plugin.InputKit.Platforms.iOS;
using Plugin.InputKit.Shared.Layouts;
using UIKit;

[assembly: ExportRenderer(typeof(StatefulStackLayout), typeof(StatefulStackLayoutRenderer))]
namespace Plugin.InputKit.Platforms.iOS
{
    public class StatefulStackLayoutRenderer : VisualElementRenderer<StackLayout>
    {
        public StatefulStackLayoutRenderer()
        {
            
        }
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            VisualStateManager.GoToState(Element, "Pressed");
            base.TouchesBegan(touches, evt);
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            VisualStateManager.GoToState(Element, "Normal");
            base.TouchesCancelled(touches, evt);
        }
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            VisualStateManager.GoToState(Element, "Normal");
            base.TouchesEnded(touches, evt);
        }
    }
}
