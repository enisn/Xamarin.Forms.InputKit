using InputKit.Shared.Controls;
using Microsoft.Maui;

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
    public partial interface IIconViewHandler : IViewHandler
    {
        IIconView TypedVirtualView { get; }
        NativeView TypedNativeView { get; }
    }
}
