using RayTracer.Lib.Core;
using RayTracer.Lib.Objects;

namespace RayTracer.Lib.Rendering;

public class Scene : IHittable
{
    public List<IHittable> Objects { get; } = new();
    public void AddObject(IHittable hittable) => Objects.Add(hittable);
    public void RemoveObject(IHittable hittable) => Objects.Remove(hittable);

    //Function returning HitInfo about nearest object that was hit by a given Ray
    public bool Hit(in Ray ray, Interval tInterval, out HitInfo hitInfo)
    {
        bool hitAnything = false;
        float closestSoFar = tInterval.Max;
        hitInfo = default;

        foreach (var obj in Objects)
        {
            if (obj.Hit(ray, new (tInterval.Min, closestSoFar), out HitInfo tempHitInfo))
            {
                hitAnything = true;
                closestSoFar = tempHitInfo.T;
                hitInfo = tempHitInfo;
            }
        }

        return hitAnything;
    }
}