using Foundation;
using Plugin.InputKit.Platforms.iOS;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(StackLayout),typeof(StackLayoutWithVisualStatesRenderer))]
namespace Plugin.InputKit.Platforms.iOS
{
    public class StackLayoutWithVisualStatesRenderer : VisualElementRenderer<StackLayout>
    {
        public StackLayoutWithVisualStatesRenderer()
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
