using RayTracer.Lib.Core;
using RayTracer.Lib.Objects;

namespace RayTracer.Lib.Materials;

public class EmissiveMaterial : Material
{
    public Color Color { get; }
    public float Intensity { get; }

    public EmissiveMaterial(Color color, float intensity = 1) 
    {
        Color = color;
        Intensity = intensity;
    }

    public override bool Scatter(in Ray rIn, ref HitInfo hitInfo, out Vec3 attenuation, out Ray scattered)
    {
        attenuation = default;
        scattered = default;
        return false;
    }

    public override Color Emit() => Color * Intensity;
}