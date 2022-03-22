using System;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared
{
    public class InputKitOptions
    {
        public static Func<Color> GetAccentColor { get; set; } = () => Color.Accent;
    }
}
