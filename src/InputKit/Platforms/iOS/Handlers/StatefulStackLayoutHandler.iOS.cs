using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
            var element = VirtualView as View;
            switch (recognizer.State)
            {
                case UIGestureRecognizerState.Began:
                    VisualStateManager.GoToState(element, "Pressed");
                    
                    break;
                case UIGestureRecognizerState.Ended:
                    // TODO: Fix working of native gesture recognizers of MAUI
                    foreach (var item in element.GestureRecognizers)
                    {
                        if (item is TapGestureRecognizer tgr)
                        {
                            tgr.Command?.Execute(element);
                        }
                    }

                    VisualStateManager.GoToState(element, "Normal");
                    break;
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
