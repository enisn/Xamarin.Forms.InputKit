using Android.Content;
using Android.Graphics.Drawables;
using Plugin.InputKit.Platforms.Android;
using Plugin.InputKit.Shared.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(EmptyEntry),typeof(EmptyEntryRenderer))]
namespace Plugin.InputKit.Platforms.Android
{
    internal class EmptyEntryRenderer : EntryRenderer
    {
        public EmptyEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                this.Control.SetBackground(gd);
            }
        }
    }
}
