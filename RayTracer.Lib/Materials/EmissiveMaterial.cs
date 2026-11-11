using RayTracer.Lib.Core;
using RayTracer.Lib.Objects;

namespace RayTracer.Lib.Materials;

public class EmissiveMaterial : Material
{
    public Color Color { get; }

    public EmissiveMaterial(Color color) 
    {
        Color = color;
    }

    public override bool Scatter(in Ray rIn, ref HitInfo hitInfo, out Vec3 attenuation, out Ray scattered)
    {
        attenuation = default;
        scattered = default;
        return false;
    }

    public override Color Emit() => Color;
}