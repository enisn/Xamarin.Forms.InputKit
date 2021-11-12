using CoreGraphics;
using InputKit.Shared.Controls;
using Microsoft.Maui.Handlers;
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

        public static void MapSource(IIconViewHandler handler, IIconView view)
        {

        }

        public static void MapFillColor(IIconViewHandler handler, IIconView view)
        {

        }
    }
}
