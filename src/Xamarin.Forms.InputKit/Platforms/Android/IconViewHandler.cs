using Android.Widget;
using Microsoft.Maui.Handlers;
using Plugin.InputKit.Shared.Controls;

namespace Plugin.InputKit.Platforms.Droid
{
    public class IconViewHandler : ViewHandler<IconView, ImageView>
    {
        public IconViewHandler() : base(null)
        {
        }

        protected override ImageView CreateNativeView()
        {
            return new ImageView(Context);
        }
    }
}
