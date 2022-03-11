using CoreGraphics;
using Foundation;
using InputKit.Shared.Controls;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Handlers;
using System.Threading;
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
                ClipsToBounds = false
            };
        }

        public static void MapSource(IIconViewHandler handler, IIconView view)
        {
            SetUIImage(handler, view);
        }

        public static void MapFillColor(IIconViewHandler handler, IIconView view)
        {
            SetUIImage(handler, view);
        }

        // TODO: Remove after following issue closed https://github.com/dotnet/maui/issues/3410
        public static void MapIsVisible(IIconViewHandler handler, IIconView view)
        {
            ViewHandler.MapVisibility(handler, view);
        }

        private static void SetUIImage(IIconViewHandler handler, IIconView view)
        {
            if (view?.Source == null) return;

            UIImage uiImage = default;

            if (view.Source is StreamImageSource streamImageSource)
            {
                var cTokenSource = new CancellationTokenSource(30000);
                var stream = streamImageSource.Stream(cTokenSource.Token).Result;
                var data = NSData.FromStream(stream);
                uiImage = UIImage.LoadFromData(data);
            }
            else if (view.Source is FileImageSource fileImageSource)
            {
                uiImage = UIImage.FromBundle(fileImageSource.File);
            }
            else
            {
                if (view.Source?.ToString().StartsWith("http") ?? false)
                    uiImage = new UIImage(view.Source.ToString());
                else
                    uiImage = UIImage.FromBundle(view.Source.ToString());
            }

            uiImage = uiImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            handler.TypedNativeView.TintColor = view.FillColor.ToUIColor();
            handler.TypedNativeView.Image = uiImage;
 
            ((IVisualElementController)view).NativeSizeChanged();
        }
    }
}
