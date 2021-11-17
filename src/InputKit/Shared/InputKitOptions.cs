using Microsoft.Maui.Graphics;
using System;

namespace InputKit.Shared;

public class InputKitOptions
{
    public static Func<Color> GetAccentColor { get; set; } = () => Color.FromHex("#512bdf");
}
