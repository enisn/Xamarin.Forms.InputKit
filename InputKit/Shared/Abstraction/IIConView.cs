using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Plugin.InputKit.Shared.Abstraction
{
    public interface IIConView : IView
    {
        Color FillColor { get; set; }
        ImageSource Source { get; set; }
    }
}
