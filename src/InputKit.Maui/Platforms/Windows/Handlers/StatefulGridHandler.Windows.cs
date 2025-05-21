using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InputKit.Shared.Layouts;
using Microsoft.Maui.Platform;

namespace InputKit.Handlers
{
    public partial class StatefulGridHandler
    {
        protected override void ConnectHandler(LayoutPanel platformView)
        {
            platformView.PointerPressed += NativeView_PointerPressed;
            platformView.PointerReleased += NativeView_PointerReleased;
            platformView.PointerEntered += NativeView_PointerEntered;
            platformView.PointerExited += NativeView_PointerExited;
        }

        protected override void DisconnectHandler(LayoutPanel platformView)
        {
            platformView.PointerPressed -= NativeView_PointerPressed;
            platformView.PointerReleased -= NativeView_PointerReleased;
            platformView.PointerEntered -= NativeView_PointerEntered;
            platformView.PointerExited -= NativeView_PointerExited;
        }
            
        private void NativeView_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (VirtualView is StatefulGrid stateful)
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
            if (VirtualView is StatefulGrid stateful)
            {
                VisualStateManager.GoToState(stateful, "Pressed");
                stateful.ApplyIsPressedAction?.Invoke(stateful, true);
            }
        }

        private void NativeView_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (VirtualView is StatefulGrid stateful)
            {
                stateful.GoDefaultVisualState();
                stateful.ApplyIsPressedAction?.Invoke(stateful, false);
            }
        }
    }
}
