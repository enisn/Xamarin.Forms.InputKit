using Plugin.InputKit.Shared.Helpers;
using Xamarin.Forms.Shapes;

namespace Plugin.InputKit.Shared.Controls
{
    public static class PredefinedShapes
    {
        public static Geometry Check = GeometryConverter.FromPath(Paths.Check);
        public static Geometry CheckCircle = GeometryConverter.FromPath(Paths.CheckCircle);
        public static Geometry Line = GeometryConverter.FromPath(Paths.LineHorizontal);
        public static Geometry Square = GeometryConverter.FromPath(Paths.Square);
        public static Geometry Dot = GeometryConverter.FromPath(Paths.Dot);
        public static Geometry ExclamationCircle = GeometryConverter.FromPath(Paths.ExclamationCircle);

        public static class Paths
        {
            public const string Check = "M 6.5212 16.4777 l -6.24 -6.24 c -0.3749 -0.3749 -0.3749 -0.9827 0 -1.3577 l 1.3576 -1.3577 c 0.3749 -0.3749 0.9828 -0.3749 1.3577 0 L 7.2 11.7259 L 16.2036 2.7224 c 0.3749 -0.3749 0.9828 -0.3749 1.3577 0 l 1.3576 1.3577 c 0.3749 0.3749 0.3749 0.9827 0 1.3577 l -11.04 11.04 c -0.3749 0.3749 -0.9828 0.3749 -1.3577 -0 z";
            public const string CheckCircle = "M12,22 C6.4771525,22 2,17.5228475 2,12 C2,6.4771525 6.4771525,2 12,2 C17.5228475,2 22,6.4771525 22,12 C22,17.5228475 17.5228475,22 12,22 Z M8,10 L6,12 L11,17 L18,10 L16,8 L11,13 L8,10 Z";
            public const string Square = "M12 12H0V0h12v12z";
            public const string LineHorizontal = "M 17.2026 6.7911 H 0.9875 C 0.4422 6.7911 0 7.2332 0 7.7784 v 2.6331 c 0 0.5453 0.442 0.9873 0.9875 0.9873 h 16.2151 c 0.5453 0 0.9873 -0.442 0.9873 -0.9873 v -2.6331 C 18.1901 7.2332 17.7481 6.7911 17.2026 6.7911 z";
            public const string Dot = "M12 18a6 6 0 100-12 6 6 0 000 12z";
            public const string ExclamationCircle = "M 2.7835 16.2165 A 9.5 9.5 90 1 1 16.2165 2.7835 A 9.5 9.5 90 0 1 2.7835 16.2165 z M 8.55 4.75 v 5.7 h 1.9 V 4.75 H 8.55 z m 0 7.6 v 1.9 h 1.9 v -1.9 H 8.55 z";
        }
    }
}