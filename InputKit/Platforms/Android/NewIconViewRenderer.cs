using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Graphics;
using Plugin.InputKit.Platforms.Droid;
using Plugin.InputKit.Shared.Controls;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(IconView), typeof(NewIconViewRenderer))]
namespace Plugin.InputKit.Platforms.Droid
{
    public class NewIconViewRenderer : Microsoft.Maui.Controls.Compatibility.Platform.Android.AppCompat.ViewRenderer<IconView, ImageView>
    {
        private bool _isDisposed;
        Context _context;
        public NewIconViewRenderer(Context context) : base(context)
        {
            base.AutoPackage = false;
            _context = context;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(Microsoft.Maui.Controls.Compatibility.Platform.Android.ElementChangedEventArgs<IconView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                SetNativeControl(new ImageView(Context));
            }
            UpdateBitmap(e.OldElement);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == IconView.SourceProperty.PropertyName)
            {
                UpdateBitmap(null);
            }
            else if (e.PropertyName == IconView.FillColorProperty.PropertyName)
            {
                UpdateBitmap(null);
            }
        }

        private void UpdateBitmap(IconView previous = null)
        {
            if (!_isDisposed)
            {
                if (Element.Source == null) return;

                Drawable d = default;
                if (Element.Source is StreamImageSource streamImageSource)
                {
                    var cTokenSource = new CancellationTokenSource(30000);
                    var stream = streamImageSource.Stream(cTokenSource.Token).Result;
                    d = Drawable.CreateFromStream(stream, "inputkit_check");
                }
                else if (Element.Source is FileImageSource fileImageSource)
                {
                    d = _context?.GetDrawable(fileImageSource.File);
                }
                else
                {
                    d = _context?.GetDrawable(Element.Source.ToString());
                }

                if (d == null) return;

                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                    d.SetTint(Element.FillColor.ToAndroid());
                else
                    d.SetColorFilter(new LightingColorFilter(Colors.Black.ToAndroid(), Element.FillColor.ToAndroid()));

                d.Alpha = Element.FillColor.ToAndroid().A;
                Control.SetImageDrawable(d);
                ((IVisualElementController)Element).NativeSizeChanged();
            }
        }

        private async Task UpdateBitmapAsync(IconView previous = null)
        {
            if (!_isDisposed)
            {
                if (Element.Source == null) return;

                Drawable d = default;
                if (Element.Source is StreamImageSource streamImageSource)
                {
                    var stream = await streamImageSource.Stream(new System.Threading.CancellationToken());
                    d = Drawable.CreateFromStream(stream, "inputkit_check");
                }
                else if (Element.Source is FileImageSource fileImageSource)
                {
                    d = _context?.GetDrawable(fileImageSource.File);
                }
                else
                {
                    d = _context?.GetDrawable(Element.Source.ToString());
                }

                //var d = _context?.GetDrawable(Element.Source)?.Mutate();

                if (d == null) return;

                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                    d.SetTint(Element.FillColor.ToAndroid());
                else
                    d.SetColorFilter(new LightingColorFilter(Colors.Black.ToAndroid(), Element.FillColor.ToAndroid()));

                d.Alpha = Element.FillColor.ToAndroid().A;
                Control.SetImageDrawable(d);
                ((IVisualElementController)Element).NativeSizeChanged();
            }
        }
    }
}
