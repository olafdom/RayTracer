using System.Drawing;

namespace RayTracer.App;

public class EmissiveMaterial : Material
{
    private Vec3 Color;

    public EmissiveMaterial(Vec3 color)
    {
        Color = color;
    }

    public override bool Scatter(in Ray rIn, ref HitInfo hitInfo, out Vec3 attenuation, out Ray scattered)
    {
        attenuation = default;
        scattered = default;
        return false;
    }

    public override Vec3 Emit() => Color;
}