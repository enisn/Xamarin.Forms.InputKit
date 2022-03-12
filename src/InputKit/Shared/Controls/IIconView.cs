using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace InputKit.Shared.Controls;

public interface IIconView : IView, IImageSourcePart
{
    Color FillColor { get; set; }
}
