namespace RayTracer.App;

public class Sphere : IHittable
{
    public Vec3 Center;
    public float Radius;
    public Material Material { get; set; }

    public Sphere(Vec3 center, float radius, Material material)
    {
        Center = center;
        Radius = radius;
        Material = material;
    }

    public bool Hit(Ray ray, float tMin, float tMax, out HitInfo hitInfo)
    {
        hitInfo = new HitInfo();
        Vec3 V = new Vec3(ray.Origin - Center); //The vector from the sphere’s center to the ray origin
        //Quadratic Equation:
        float a = ray.Direction.LengthSquared;
        float halfb = Vec3.DotProduct(V, ray.Direction);
        float c = V.LengthSquared - Radius * Radius;
        float discriminantReduced = halfb * halfb - a * c;
        float sqrtDiscriminant = MathF.Sqrt(discriminantReduced);
        if (discriminantReduced < 0) return false;
        float root = (-halfb - sqrtDiscriminant) / a;
        if (root < tMin || tMax < root)
        {
            root = (-halfb + sqrtDiscriminant) / a;
            if (root < tMin || tMax < root)
            {
                return false;
            }
        }
        hitInfo.Material = Material;
        hitInfo.T = root;
        hitInfo.Position = ray.At(hitInfo.T);
        Vec3 outwardNormal = (hitInfo.Position - Center) / Radius;
        hitInfo.SetFaceNormal(ray, outwardNormal);
        return true;
    }
}