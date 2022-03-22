using InputKit.Shared.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;

#if __IOS__ || MACCATALYST
using NativeView = UIKit.UIImageView;
#elif ANDROID
using NativeView = Android.Widget.ImageView;
#elif UWP
using NativeView = Microsoft.UI.Xaml.Controls.Image;
#elif NETSTANDARD || (NET6_0 && !IOS && !ANDROID)
using NativeView = System.Object;
#endif

namespace InputKit.Handlers.IconView
{
    public partial class IconViewHandler : IIconViewHandler
    {
        public static IPropertyMapper<IIconView, IIconViewHandler> IconViewMapper => new PropertyMapper<IIconView, IIconViewHandler>()
        {
            [nameof(IIconView.Source)] = MapSource,
            [nameof(IIconView.FillColor)] = MapFillColor,

            // TODO: Remove after following issue closed https://github.com/dotnet/maui/issues/3410
            [nameof(View.IsVisible)] = MapIsVisible,

        };

        public IconViewHandler() : base(IconViewMapper)
        {

        }

        public IconViewHandler(IPropertyMapper? mapper = null) : base(mapper ?? IconViewMapper)
        {
        }

        IIconView IIconViewHandler.TypedVirtualView => VirtualView;

        NativeView IIconViewHandler.TypedPlatformView => PlatformView;
    }
}
