using RayTracer.Lib.Core;
using RayTracer.Lib.Objects;

namespace RayTracer.Lib.Materials;

public class EmissiveMaterial : Material
{
    public Vec3 Color { get; }

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