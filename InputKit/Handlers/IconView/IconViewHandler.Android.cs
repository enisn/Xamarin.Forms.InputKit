using Android.Widget;
using Microsoft.Maui.Handlers;
using Plugin.InputKit.Shared.Abstraction;

namespace Plugin.InputKit.Handlers.IconView
{
    public partial class IconViewHandler : ViewHandler<IIConView, ImageView>
    {
        protected override ImageView CreateNativeView()
        {
            return new ImageView(Context);
        }

        public static void MapSource(ViewHandler handler, IIConView view) 
        {
        
        }

        public static void MapFillColor(ViewHandler handler, IIConView view) 
        {
        
        }
    }
}
