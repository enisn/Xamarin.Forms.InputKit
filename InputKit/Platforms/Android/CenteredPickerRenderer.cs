using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Plugin.InputKit.Platforms.Android;
using Plugin.InputKit.Shared.Controls;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;



[assembly: ExportRenderer(typeof(CenteredPicker), typeof(CenteredPickerRenderer))]
namespace Plugin.InputKit.Platforms.Android
{
    public class CenteredPickerRenderer : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
    {
        //public CenteredPickerRenderer(Context context) : base(context)
        //{

        //}

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(global::Android.Graphics.Color.Transparent);
            this.Control.SetBackground(gd);
            this.Control.Gravity = GravityFlags.Center;
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "TextAlign")
            {
                switch ((this.Element as CenteredPicker).TextAlingn)
                {
                    case Xamarin.Forms.TextAlignment.Start:
                        this.Control.Gravity = GravityFlags.Start;
                        break;
                    case Xamarin.Forms.TextAlignment.Center:
                        this.Control.Gravity = GravityFlags.Center;
                        break;
                    case Xamarin.Forms.TextAlignment.End:
                        this.Control.Gravity = GravityFlags.End;
                        break;
                }
            }


        }
    }
}
