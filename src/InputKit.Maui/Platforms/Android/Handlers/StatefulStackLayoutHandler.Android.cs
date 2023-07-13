#if ANDROID
using Android.Views;
using InputKit.Shared.Layouts;
using Microsoft.Maui.Platform;
using View = Android.Views.View;

namespace InputKit.Handlers
{
    public partial class StatefulStackLayoutHandler
    {
        protected override LayoutViewGroup CreatePlatformView()
        {
            var nativeView = base.CreatePlatformView();

            nativeView.Touch += NativeView_Touch;

            return nativeView;
        }

        private void NativeView_Touch(object sender, View.TouchEventArgs e)
        {
            if (VirtualView is StatefulStackLayout stateful)
            {
                if (e.Event.Action == MotionEventActions.Down)
                {
                    VisualStateManager.GoToState(stateful, "Pressed");
                    stateful.ApplyIsPressedAction?.Invoke(stateful, true);
                }
                else if (e.Event.Action == MotionEventActions.Up || e.Event.Action == MotionEventActions.Cancel)
                {
                    stateful.GoDefaultVisualState();
                    stateful.ApplyIsPressedAction?.Invoke(stateful, false);
                }
            }
        }
    }
}
#endif