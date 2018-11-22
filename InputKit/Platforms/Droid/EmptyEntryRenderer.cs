using Android.Content;
using Android.Graphics.Drawables;
using Plugin.InputKit.Platforms.Droid;
using Plugin.InputKit.Shared.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(EmptyEntry),typeof(EmptyEntryRenderer))]
namespace Plugin.InputKit.Platforms.Droid
{
    public class EmptyEntryRenderer : EntryRenderer
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
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
                Control.SetTextColor(Android.Graphics.Color.Black);
            }
        }
    }
}
