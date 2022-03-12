using Android.Views;
using Java.Interop;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System;
using View = Android.Views.View;

namespace InputKit.Handlers
{
    public partial class StatefulStackLayoutHandler
    {
        protected override LayoutViewGroup CreateNativeView()
        {
            var nativeView = base.CreateNativeView();

            nativeView.Touch += NativeView_Touch;

            return nativeView;
        }

        private void NativeView_Touch(object sender, View.TouchEventArgs e)
        {
            var element = VirtualView as Microsoft.Maui.Controls.View;

            if (e.Event.Action == MotionEventActions.Down)
            {
                Microsoft.Maui.Controls.VisualStateManager.GoToState(element, "Pressed");
            }
            else if (e.Event.Action == MotionEventActions.Up || e.Event.Action == MotionEventActions.Cancel)
            {
                Microsoft.Maui.Controls.VisualStateManager.GoToState(element, "Normal");
            }
        }
    }
}
