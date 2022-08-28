#if UWP
using Microsoft.Maui.Platform;

namespace InputKit.Handlers
{
    public partial class StatefulStackLayoutHandler
    {
        protected override LayoutPanel CreatePlatformView()
        {
            var nativeView = base.CreatePlatformView();

            nativeView.PointerPressed += NativeView_PointerPressed;

            nativeView.PointerReleased += NativeView_PointerReleased;

            return nativeView;
        }

        private void NativeView_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var element = VirtualView as View;

            VisualStateManager.GoToState(element, "Pressed");
        }

        private void NativeView_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var element = VirtualView as View;

            VisualStateManager.GoToState(element, "Normal");
        }
    }
}
#endif