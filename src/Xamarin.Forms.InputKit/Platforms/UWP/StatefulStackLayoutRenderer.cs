using Plugin.InputKit.Platforms.UWP;
using Plugin.InputKit.Shared.Layouts;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(StatefulStackLayout), typeof(StatefulStackLayoutRenderer))]
namespace Plugin.InputKit.Platforms.UWP
{
    public class StatefulStackLayoutRenderer : VisualElementRenderer<StackLayout, StackPanel>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.PointerPressed += Control_PointerPressed;
                Control.PointerReleased += Control_PointerReleased;
            }
        }

        private void Control_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(Element, "Pressed");
        }

        private void Control_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(Element, "Normal");
        }
    }
}
