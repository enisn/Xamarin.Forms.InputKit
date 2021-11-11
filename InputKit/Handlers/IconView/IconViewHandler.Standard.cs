using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Plugin.InputKit.Shared.Abstraction;
using System;

namespace Plugin.InputKit.Handlers.IconView
{
    public partial class IconViewHandler : ViewHandler<IIConView, object>
    {
        protected override object CreateNativeView() => throw new NotImplementedException();

        public static void MapSource(ViewHandler handler, IIConView view) { }
        public static void MapFillColor(ViewHandler handler, IIConView view) { }
    }
}
