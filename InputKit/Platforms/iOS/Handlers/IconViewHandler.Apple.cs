using CoreGraphics;
using Microsoft.Maui.Handlers;
using Plugin.InputKit.Shared.Controls;
using UIKit;

namespace InputKit.Handlers.IconView
{
    public partial class IconViewHandler : ViewHandler<IIconView, UIImageView>
    {
        protected override UIImageView CreateNativeView()
        {
            return new UIImageView(CGRect.Empty)
            {
                ContentMode = UIViewContentMode.ScaleAspectFit,
                ClipsToBounds = true
            };
        }

        public static void MapSource(ViewHandler handler, IIconView view)
        {

        }

        public static void MapFillColor(ViewHandler handler, IIconView view)
        {

        }
    }
}
