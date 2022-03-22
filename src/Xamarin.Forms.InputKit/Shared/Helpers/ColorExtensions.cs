using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Helpers
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Defines a surface color will be black or white.
        /// </summary>
        /// <param name="color">Background color</param>
        /// <returns>Surface color on background color</returns>
        public static Color ToSurfaceColor(this Color color)
        {
            if ((color.R + color.G + color.B) >= 1.8)
                return Color.Black;
            else
                return Color.White;
        }
    }
}
