using Microsoft.Maui.Graphics;
using System;

namespace InputKit.Shared;

public class InputKitOptions
{
    public static Func<Color> GetAccentColor { get; set; } = () => Color.FromArgb("#512bdf");
}
