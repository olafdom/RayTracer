namespace RayTracer.App;

public class Plane : IHittable
{
    Vec3 Normal;
    Vec3 Position;

    public Plane(Vec3 normal, Vec3 position)
    {
        Normal = normal.Normalize();
        Position = position;
    }

    public bool Hit(Ray ray, float tMin, float tMax, out HitInfo hitInfo)
    {
        hitInfo = new HitInfo();
        float denominator = Vec3.DotProduct(Normal, ray.Direction);
        if (MathF.Abs(denominator) < 1e-6f)
        {
            return false;
        }

        float t = Vec3.DotProduct(Position - ray.Origin, Normal) / denominator;
        if (t < tMin || t > tMax) return false;
        hitInfo.T = t;
        hitInfo.SetFaceNormal(ray, Normal);
        hitInfo.Position = ray.At(t);
        return true;
    }
}