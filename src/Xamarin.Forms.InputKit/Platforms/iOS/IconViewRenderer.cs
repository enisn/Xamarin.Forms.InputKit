using CoreGraphics;
using Foundation;
using Plugin.InputKit.Platforms.iOS;
using Plugin.InputKit.Shared.Controls;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
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
                //Task.Run(async () => await SetImageAsync(e.OldElement));
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == IconView.SourceProperty.PropertyName)
            {
                SetImage(null);
                //Task.Run(async () => await SetImageAsync(null));
            }
            else if (e.PropertyName == IconView.FillColorProperty.PropertyName)
            {
                SetImage(null);
                //Task.Run(async () => await SetImageAsync(null));
            }
        }

        private void SetImage(IconView previous = null)
        {
            if (previous == null)
            {
                System.Diagnostics.Debug.WriteLine("[IconView] Source updated as : " + Element?.Source);
                if (Element?.Source == null) return;

                UIImage uiImage;

                if (Element.Source is StreamImageSource streamImageSource)
                {
                    var cTokenSource = new CancellationTokenSource(30000);
                    var stream = streamImageSource.Stream(cTokenSource.Token).Result;
                    var data = NSData.FromStream(stream);
                    uiImage = UIImage.LoadFromData(data);
                }
                else if (Element.Source is FileImageSource fileImageSource)
                {
                    uiImage = UIImage.FromBundle(fileImageSource.File);
                }
                else
                {
                    if (Element.Source?.ToString().StartsWith("http") ?? false)
                        uiImage = new UIImage(Element.Source.ToString());
                    else
                        uiImage = UIImage.FromBundle(Element.Source.ToString());
                }

                uiImage = uiImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                Control.TintColor = Element.FillColor.ToUIColor();
                Control.Image = uiImage;
                if (!_isDisposed)
                {
                    ((IVisualElementController)Element).NativeSizeChanged();
                }
            }
        }

        private async Task SetImageAsync(IconView previous = null)
        {
            if (previous == null)
            {
                System.Diagnostics.Debug.WriteLine("SOURCE : " + Element?.Source?.ToString());
                if (Element?.Source == null) return;

                UIImage uiImage;

                if (Element.Source is StreamImageSource streamImageSource)
                {
                    var stream = await streamImageSource.Stream(new CancellationToken());
                    var data = NSData.FromStream(stream);
                    uiImage = UIImage.LoadFromData(data);
                }
                else if (Element.Source is FileImageSource fileImageSource)
                {
                    uiImage = UIImage.FromBundle(fileImageSource.File);
                }
                else
                {
                    if (Element.Source?.ToString().Contains("http") ?? false)
                        uiImage = new UIImage(Element.Source.ToString());
                    else
                        uiImage = UIImage.FromBundle(Element.Source.ToString());
                }


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
