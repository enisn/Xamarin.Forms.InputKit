using CoreGraphics;
using Plugin.InputKit.Platforms.iOS;
using Plugin.InputKit.Shared.Controls;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(IconView), typeof(IconViewRenderer))]
namespace Plugin.InputKit.Platforms.iOS
{
    public class IconViewRenderer : ViewRenderer<IconView, UIImageView>
    {
        private bool _isDisposed;
        protected override void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing && base.Control != null)
            {
                UIImage image = base.Control.Image;
                UIImage uIImage = image;
                if (image != null)
                {
                    uIImage.Dispose();
                    uIImage = null;
                }
            }
            _isDisposed = true;
            base.Dispose(disposing);
        }
        protected override void OnElementChanged(ElementChangedEventArgs<IconView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                UIImageView uIImageView = new UIImageView(CGRect.Empty)
                {
                    ContentMode = UIViewContentMode.ScaleAspectFit,
                    ClipsToBounds = true
                };
                SetNativeControl(uIImageView);
            }
            if (e.NewElement != null)
            {
                SetImage(e.OldElement);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == IconView.SourceProperty.PropertyName)
            {
                SetImage(null);
            }
            else if (e.PropertyName == IconView.FillColorProperty.PropertyName)
            {
                SetImage(null);
            }
        }

        private void SetImage(IconView previous = null)
        {
            if (previous == null)
            {
                System.Diagnostics.Debug.WriteLine("SOURCE : " + Element?.Source?.ToString());
                if (Element?.Source == null)
                    return;
                UIImage uiImage;
                if (Element.Source?.Contains("http") ?? false)
                    uiImage = new UIImage(Element.Source);
                else
                    uiImage = UIImage.FromBundle(Element.Source);

                uiImage = uiImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                Control.TintColor = Element.FillColor.ToUIColor();
                Control.Image = uiImage;
                if (!_isDisposed)
                {
                    ((IVisualElementController)Element).NativeSizeChanged();
                }
            }
        }
    }
}
