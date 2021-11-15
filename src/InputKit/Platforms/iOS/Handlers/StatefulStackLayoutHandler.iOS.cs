using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace InputKit.Handlers
{
    public partial class StatefulStackLayoutHandler
    {
        protected override LayoutView CreateNativeView()
        {
            var nativeView = base.CreateNativeView();

            // TODO: Implement for iOS
            //nativeView.GestureRecognizers.Append(new UITapGestureRecognizer(Tapped))

            return nativeView;
        }
    }
}
