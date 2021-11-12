using CoreGraphics;
using Microsoft.Maui.Handlers;
using Plugin.InputKit.Shared.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Plugin.InputKit.Handlers.IconView
{
    public partial class IconViewHandler : ViewHandler<IIConView, UIImageView>
    {
        protected override UIImageView CreateNativeView()
        {
            return new UIImageView(CGRect.Empty)
            {
                ContentMode = UIViewContentMode.ScaleAspectFit,
                ClipsToBounds = true
            };
        }

        public static void MapSource(ViewHandler handler, IIConView view)
        {

        }

        public static void MapFillColor(ViewHandler handler, IIConView view)
        {

        }
    }
}
