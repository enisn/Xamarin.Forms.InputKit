using Android.Content;
using Android.Graphics.Drawables;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Plugin.InputKit.Platforms.Droid;
using Plugin.InputKit.Shared.Controls;

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

        protected override void OnElementChanged(Microsoft.Maui.Controls.Compatibility.Platform.Android.ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                this.Control.SetBackground(gd);
                Control.SetTextColor(Element.TextColor.ToAndroid());
            }
        }
    }
}
