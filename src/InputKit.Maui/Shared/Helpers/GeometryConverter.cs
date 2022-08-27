using Microsoft.Maui.Controls.Shapes;

namespace InputKit.Shared.Helpers;
public static class GeometryConverter
{
    private static PathGeometryConverter PathGeometryConverter { get; } = new PathGeometryConverter();
    public static Geometry FromPath(string path)
    {
        return (Geometry)PathGeometryConverter.ConvertFromInvariantString(path);
    }
}
