using RayTracer.Lib.Core;
using RayTracer.Lib.Materials;

namespace RayTracer.Lib.Objects;

public readonly struct Plane : IHittable
{
    private const float Epsilon = 1e-6f;
    public Vec3 Normal { get; }
    public Vec3 Position { get; }
    public Material Material { get; }

    public Plane(Vec3 position, Vec3 normal, Material material)
    {
        Normal = normal.Normalize();
        Position = position;
        Material = material;
    }

    public bool Hit(in Ray ray, Interval tInterval, out HitInfo hitInfo)
    {
        hitInfo = default;
        
        float denominator = Vec3.DotProduct(Normal, ray.Direction);
        
        if (MathF.Abs(denominator) < Epsilon)
            return false;

        float t = Vec3.DotProduct(Position - ray.Origin, Normal) / denominator;
        
        if (t < tInterval.Min || t > tInterval.Max) 
            return false;
        
        hitInfo.T = t;
        hitInfo.Position = ray.At(t);
        hitInfo.SetFaceNormal(ray, Normal);
        hitInfo.Material = Material;
        
        return true;
    }
}