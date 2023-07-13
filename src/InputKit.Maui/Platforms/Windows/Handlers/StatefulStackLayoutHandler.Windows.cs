#if UWP
using InputKit.Shared.Layouts;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;

namespace InputKit.Handlers
{
    public partial class StatefulStackLayoutHandler
    {
        protected override void ConnectHandler(LayoutPanel platformView)
        {
            PlatformView.PointerPressed += NativeView_PointerPressed;
            PlatformView.PointerReleased += NativeView_PointerReleased;
            PlatformView.PointerEntered += NativeView_PointerEntered;
            PlatformView.PointerExited += NativeView_PointerExited;
        }

        protected override void DisconnectHandler(LayoutPanel platformView)
        {
            PlatformView.PointerPressed -= NativeView_PointerPressed;
            PlatformView.PointerReleased -= NativeView_PointerReleased;
            PlatformView.PointerEntered -= NativeView_PointerEntered;
            PlatformView.PointerExited -= NativeView_PointerExited;
        }

        private void NativeView_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (VirtualView is StatefulStackLayout stateful)
            {
                stateful.GoDefaultVisualState();
            }
        }

        private void NativeView_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(VirtualView as View, VisualStateManager.CommonStates.PointerOver);
        }

        private void NativeView_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (VirtualView is StatefulStackLayout stateful)
            {
                VisualStateManager.GoToState(stateful, "Pressed");
                stateful.ApplyIsPressedAction?.Invoke(stateful, true);
            }
        }

        private void NativeView_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (VirtualView is StatefulStackLayout stateful)
            {
                stateful.GoDefaultVisualState();
                stateful.ApplyIsPressedAction?.Invoke(stateful, false);
            }
        }
    }
}
#endif