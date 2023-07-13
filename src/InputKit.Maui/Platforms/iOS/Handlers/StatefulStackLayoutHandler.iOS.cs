#if IOS
using Foundation;
using InputKit.Shared.Layouts;
using Microsoft.Maui.Platform;
using System.Diagnostics;
using UIKit;

namespace InputKit.Handlers
{
    public partial class StatefulStackLayoutHandler
    {
        protected override void ConnectHandler(LayoutView nativeView)
        {
            nativeView.AddGestureRecognizer(new UIContinousGestureRecognizer(Tapped));

            base.ConnectHandler(nativeView);
        }

        private void Tapped(UIGestureRecognizer recognizer)
        {
            if (VirtualView is StatefulStackLayout stateful)
            {
                switch (recognizer.State)
                {
                    case UIGestureRecognizerState.Began:

                        VisualStateManager.GoToState(stateful, "Pressed");
                        stateful.ApplyIsPressedAction?.Invoke(stateful, true);

                        break;
                    case UIGestureRecognizerState.Ended:

                        stateful.GoDefaultVisualState();
                        stateful.ApplyIsPressedAction?.Invoke(stateful, false);

                        //// TODO: Fix working of native gesture recognizers of MAUI
                        foreach (var item in stateful.GestureRecognizers)
                        {
                            Debug.WriteLine(item.GetType().Name);
                            if (item is TapGestureRecognizer tgr)
                            {
                                tgr.Command.Execute(stateful);
                            }
                        }

                        break;
                }
            }
        }
    }

    internal class UIContinousGestureRecognizer : UIGestureRecognizer
    {
        private readonly Action<UIGestureRecognizer> action;

        public UIContinousGestureRecognizer(Action<UIGestureRecognizer> action)
        {
            this.action = action;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            State = UIGestureRecognizerState.Began;

            action(this);

            base.TouchesBegan(touches, evt);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            State = UIGestureRecognizerState.Ended;

            action(this);

            base.TouchesEnded(touches, evt);
        }
    }
}
#endif