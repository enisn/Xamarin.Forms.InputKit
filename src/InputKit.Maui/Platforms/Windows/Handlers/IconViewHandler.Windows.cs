#if UWP
using InputKit.Shared.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace InputKit.Handlers.IconView;

public partial class IconViewHandler : ViewHandler<IIconView, Microsoft.UI.Xaml.Controls.Image>
{
    protected override Microsoft.UI.Xaml.Controls.Image CreatePlatformView()
    {
        return new Microsoft.UI.Xaml.Controls.Image();
    }
    public static async void MapSource(IIconViewHandler handler, IIconView view)
    {
        await MapSourceAsync(handler, view);
    }

    public static void MapFillColor(IIconViewHandler handler, IIconView view)
    {
        //UpdateBitmap(handler, view); // TODO: Will be implemented
    }

    // TODO: Remove after following issue closed https://github.com/dotnet/maui/issues/3410
    public static void MapIsVisible(IIconViewHandler handler, IIconView view)
    {
        ViewHandler.MapVisibility(handler, view);
    }

    ImageSourcePartLoader _imageSourcePartLoader;
    public ImageSourcePartLoader SourceLoader =>
        _imageSourcePartLoader ??= new ImageSourcePartLoader(this, () => VirtualView, OnSetImageSource);

    void OnSetImageSource(Microsoft.UI.Xaml.Media.ImageSource obj)
    {
        PlatformView.Source = obj;
    }

    public static Task MapSourceAsync(IIconViewHandler handler, IIconView image)
    {
        if (handler.TypedPlatformView == null)
            return Task.CompletedTask;

        return (handler as IconViewHandler).SourceLoader.UpdateImageSourceAsync();
    }
}
#endif