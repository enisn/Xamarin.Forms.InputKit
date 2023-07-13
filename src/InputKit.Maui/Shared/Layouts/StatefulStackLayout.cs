using Microsoft.Maui.Controls;

namespace InputKit.Shared.Layouts
{

    /// <summary>
    /// This class generated for prevent future errors with VisualStates of StackLayout. This StackLayout has VisaulStates like 'Pressed', 'Normal' etc. 
    /// </summary>
    public class StatefulStackLayout : StackLayout
    {
        public string DefaultVisualState { get; set; } = VisualStateManager.CommonStates.Normal;

        /// <summary>
        /// Applies pressed effect. You can set another <see cref="void"/> to make custom pressed effects.
        /// </summary>
        public Action<StatefulStackLayout, bool> ApplyIsPressedAction { get; set; }

        public void GoDefaultVisualState()
        {
            VisualStateManager.GoToState(this, DefaultVisualState);
        }
    }
}
