using RayTracer.Lib.Core;

namespace RayTracer.Lib.Objects;

public interface IHittable
{
    public bool Hit(in Ray ray, float tMin, float tMax, out HitInfo hitInfo);
}