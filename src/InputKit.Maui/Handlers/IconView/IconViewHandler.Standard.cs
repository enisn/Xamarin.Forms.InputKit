#if NETSTANDARD || (NET6_0 && !IOS && !MACCATALYST && !ANDROID && !UWP)
using InputKit.Shared.Controls;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputKit.Handlers.IconView
{
    public partial class IconViewHandler : ViewHandler<IIconView, object>
    {

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

        protected override object CreatePlatformView()
        {
            throw new NotImplementedException();
        }
    }
}
#endif