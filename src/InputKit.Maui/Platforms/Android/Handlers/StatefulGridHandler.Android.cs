#if ANDROID
using Android.Views;
using InputKit.Shared.Layouts;
using Microsoft.Maui.Platform;
using View = Android.Views.View;


namespace InputKit.Handlers
{
    public partial class StatefulGridHandler
    {
        protected override LayoutViewGroup CreatePlatformView()
        {
            return base.CreatePlatformView();
        }

        protected override void ConnectHandler(LayoutPanel platformView)
        {
            base.ConnectHandler(platformView);
            
            platformView.Touch += NativeView_Touch;
        }

        protected override void DisconnectHandler(LayoutPanel platformView)
        {
            platformView.Touch -= NativeView_Touch;
            
            base.DisconnectHandler(platformView);
        }

        private void NativeView_Touch(object sender, View.TouchEventArgs e)
        {
            if (VirtualView is StatefulGrid stateful)
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
