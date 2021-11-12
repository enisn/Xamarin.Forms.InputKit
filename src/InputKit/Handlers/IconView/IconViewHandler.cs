using InputKit.Shared.Controls;
using Microsoft.Maui;
using System;

#if __IOS__ || MACCATALYST
using NativeView = UIKit.UIImageView;
#elif ANDROID
using NativeView = Android.Widget.ImageView;
#elif WINDOWS
using NativeView = Microsoft.Maui.MauiButton;
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

        };

        public IconViewHandler() : base(IconViewMapper)
        {

        }

        public IconViewHandler(IPropertyMapper? mapper = null) : base(mapper ?? IconViewMapper)
        {
        }

        IIconView IIconViewHandler.TypedVirtualView => VirtualView;

        NativeView IIconViewHandler.TypedNativeView => NativeView;
    }
}
