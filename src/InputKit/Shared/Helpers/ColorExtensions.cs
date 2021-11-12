using Microsoft.Maui.Graphics;

namespace InputKit.Shared.Helpers;

public static class ColorExtensions
{
    /// <summary>
    /// Defines a surface color will be black or white.
    /// </summary>
    /// <param name="color">Background color</param>
    /// <returns>Surface color on background color</returns>
    public static Color ToSurfaceColor(this Color color)
    {
        if (color.Red + color.Green + color.Blue >= 1.8)
            return Colors.Black;
        else
            return Colors.White;
    }
}
