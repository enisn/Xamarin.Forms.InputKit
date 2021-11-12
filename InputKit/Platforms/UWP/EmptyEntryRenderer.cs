using Plugin.InputKit.Platforms.UWP;
using Plugin.InputKit.Shared.Controls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly:ExportRenderer(typeof(EmptyEntry),typeof(EmptyEntryRenderer))]
namespace Plugin.InputKit.Platforms.UWP
{
    public class EmptyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            if (Control != null)
            {
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);
            }
            base.OnElementChanged(e);
        }
    }
}
