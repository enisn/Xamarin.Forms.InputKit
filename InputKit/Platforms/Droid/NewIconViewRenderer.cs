using Plugin.InputKit.Platforms.Droid;
using Plugin.InputKit.Shared.Controls;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System;

[assembly: ExportRenderer(typeof(IconView), typeof(NewIconViewRenderer))]
namespace Plugin.InputKit.Platforms.Droid
{
    public class NewIconViewRenderer : ViewRenderer<IconView, ImageView>
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
        protected override void OnElementChanged(ElementChangedEventArgs<IconView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                SetNativeControl(new ImageView(Context));
            }
            UpdateBitmapAsync(e.OldElement);
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == IconView.SourceProperty.PropertyName)
            {
                UpdateBitmapAsync(null);
            }
            else if (e.PropertyName == IconView.FillColorProperty.PropertyName)
            {
                UpdateBitmapAsync(null);
            }
        }

        private async void UpdateBitmapAsync(IconView previous = null)
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
                else if(Element.Source is FileImageSource fileImageSource)
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
                    d.SetColorFilter(new LightingColorFilter(Xamarin.Forms.Color.Black.ToAndroid(), Element.FillColor.ToAndroid()));

                d.Alpha = Element.FillColor.ToAndroid().A;
                Control.SetImageDrawable(d);
                ((IVisualElementController)Element).NativeSizeChanged();
            }
        }
    }
}
