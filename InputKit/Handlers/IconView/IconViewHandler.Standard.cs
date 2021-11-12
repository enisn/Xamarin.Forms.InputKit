#if NETSTANDARD || (NET6_0 && !IOS && !ANDROID)
using Microsoft.Maui.Handlers;
using Plugin.InputKit.Shared.Controls;
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
    }
}
#endif