using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Plugin.InputKit.Platforms.Android;
using Plugin.InputKit.Shared.Controls;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.OS.Build;

[assembly: ExportRenderer(typeof(IconView), typeof(IconViewRenderer))]
namespace Plugin.InputKit.Platforms.Android
{
    public class IconViewRenderer : ViewRenderer<IconView, ImageView>
    {
        private bool _isDisposed;
        Context _context;

        public IconViewRenderer()
        {

        }

        protected IconViewRenderer(Context context) : base(context)
        {

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
        protected override void OnElementChanged(ElementChangedEventArgs<IconView> e)
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
                var d = _context?.GetDrawable(Element.Source)?.Mutate();

                if (d == null) return;
                
                if (VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    d.SetTint(Element.FillColor.ToAndroid());
                else
                    d.SetColorFilter(new LightingColorFilter(Xamarin.Forms.Color.Black.ToAndroid(), Element.FillColor.ToAndroid()));

                d.Alpha = Element.FillColor.ToAndroid().A;
                Control.SetImageDrawable(d);
                ((IVisualElementController)Element).NativeSizeChanged();
            }
        }
    }
}
