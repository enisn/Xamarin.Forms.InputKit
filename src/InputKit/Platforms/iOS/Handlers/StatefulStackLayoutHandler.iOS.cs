using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace InputKit.Handlers
{
    public partial class StatefulStackLayoutHandler
    {
        protected override LayoutView CreateNativeView()
        {
            var nativeView = base.CreateNativeView();

            // TODO: Implement for iOS
            nativeView.GestureRecognizers.Append(new UITapGestureRecognizer(Tapped));

            return nativeView;
        }

        private void Tapped(UITapGestureRecognizer recognizer)
        {
            var element = VirtualView as View;
            switch (recognizer.State)
            {
                case UIGestureRecognizerState.Began:
                    VisualStateManager.GoToState(element, "Pressed");
                    break;
                case UIGestureRecognizerState.Ended:
                case UIGestureRecognizerState.Cancelled:
                case UIGestureRecognizerState.Failed:
                    VisualStateManager.GoToState(element, "Normal");
                    break;
            }
        }
    }
}
