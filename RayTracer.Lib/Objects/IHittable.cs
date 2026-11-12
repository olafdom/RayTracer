using RayTracer.Lib.Core;

namespace RayTracer.Lib.Objects;

public interface IHittable
{
    public bool Hit(in Ray ray, Interval tInterval, out HitInfo hitInfo);
}