#if NETSTANDARD || (NET6_0 && !IOS && !ANDROID)
using InputKit.Shared.Controls;
using Microsoft.Maui.Handlers;
using InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputKit.Handlers.IconView
{
    public partial class IconViewHandler : ViewHandler<IIconView, object>
    {
        protected override object CreateNativeView() => throw new NotImplementedException();

        static void MapSource(IIconViewHandler handler, IIconView view)
        {
        }

        static void MapFillColor(IIconViewHandler handler, IIconView view)
        {
        }


        // TODO: Remove after following issue closed https://github.com/dotnet/maui/issues/3410
        static void MapIsVisible(IIconViewHandler handler, IIconView view)
        {
        }
    }
}
#endif