using RayTracer.Lib.Core;
using RayTracer.Lib.Materials;

namespace RayTracer.Lib.Objects;

public readonly struct Sphere : IHittable
{
    public Vec3 Center { get; }
    public float Radius { get; }    
    public Material Material { get; }

    public Sphere(Vec3 center, float radius, Material material)
    {
        Center = center;
        Radius = radius;
        Material = material;
    }

    public bool Hit(in Ray ray, Interval tInterval, out HitInfo hitInfo)
    {
        hitInfo = default;
        
        //The vector from the sphere’s center to the ray origin
        Vec3 v = new Vec3(ray.Origin - Center);
        
        //Quadratic Equation:
        float a = ray.Direction.LengthSquared;
        float halfb = Vec3.DotProduct(v, ray.Direction);
        float c = v.LengthSquared - Radius * Radius;
        
        float discriminantReduced = halfb * halfb - a * c;
        float sqrtDiscriminant = MathF.Sqrt(discriminantReduced);
        
        if (discriminantReduced < 0) 
            return false;
        
        //Take closer root:
        float root = (-halfb - sqrtDiscriminant) / a;
        if (root < tInterval.Min || tInterval.Max < root)
        {
            root = (-halfb + sqrtDiscriminant) / a;
            if (root < tInterval.Min || tInterval.Max < root)
            {
                return false;
            }
        }
        
        hitInfo.T = root;
        hitInfo.Position = ray.At(root);
        Vec3 outwardNormal = (hitInfo.Position - Center) / Radius;
        hitInfo.SetFaceNormal(ray, outwardNormal);
        hitInfo.Material = Material;
        
        return true;
    }
}