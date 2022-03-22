using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Plugin.InputKit.Shared.Controls
{
    public interface IIconView : IView
    {
        Color FillColor { get; set; }
        ImageSource Source { get; set; }
    }
}