namespace RayTracer.App;

public class Scene : IHittable
{
    public List<IHittable> Objects { get; } = new List<IHittable>();
    public void AddObject(IHittable hittable) => Objects.Add(hittable);
    public void RemoveObject(IHittable hittable) => Objects.Remove(hittable);

    //Function returning HitInfo about nearest object that was hit by a given Ray
    public bool Hit(Ray ray, float tMin, float tMax, out HitInfo hitInfo)
    {
        HitInfo tempHitInfo;
        bool hitAnything = false;
        float closestSoFar = tMax;
        hitInfo = new HitInfo();

        foreach (var obj in Objects)
        {
            if (obj.Hit(ray, tMin, closestSoFar, out tempHitInfo))
            {
                hitAnything = true;
                closestSoFar = tempHitInfo.T;
                hitInfo = tempHitInfo;
            }
        }

        return hitAnything;
    }
}