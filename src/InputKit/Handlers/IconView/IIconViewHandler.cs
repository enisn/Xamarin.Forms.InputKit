using InputKit.Shared.Controls;
using Microsoft.Maui;

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
    public interface IIconViewHandler : IViewHandler
    {
        IIconView TypedVirtualView { get; }
        NativeView TypedNativeView { get; }
    }
}
