using System.Runtime.InteropServices;
using RayTracer.Lib.Core;
using RayTracer.Lib.Objects;

namespace RayTracer.Lib.Rendering;

public class Scene : IHittable
{
    public List<IHittable> Objects { get; } = new();
    
    public Aabb Aabb { get; set; } = Aabb.Empty;
    
    private volatile BvhTree? _bvhTree;
    private readonly List<IHittable> _unboundedObjects = new();
    private readonly object _bvhLock = new();

    private BvhTree BuildBvhTree()
    {
        lock (_bvhLock)
        {
            if (_bvhTree != null)
                return _bvhTree;

            List<IHittable> bounded = new(Objects.Count);
            _unboundedObjects.Clear();

            foreach (IHittable obj in Objects)
            {
                if (obj.Aabb.IsUnbounded)
                    _unboundedObjects.Add(obj);
                else
                    bounded.Add(obj);
            }

            _bvhTree = new BvhTree(CollectionsMarshal.AsSpan(bounded));
            return _bvhTree;
        }
    }

    public void AddObject(IHittable hittable)
    {
        Objects.Add(hittable);
        Aabb = new Aabb(Aabb, hittable.Aabb);
        _bvhTree = null;
    }

    public void RemoveObject(IHittable hittable)
    {
        Objects.Remove(hittable);
        _bvhTree = null;
    }
    

    //Function returning HitInfo about nearest object that was hit by a given Ray
    public bool Hit(in Ray ray, Interval tInterval, out HitInfo hitInfo)
    {
        if (Objects.Count == 0)
        {
            hitInfo = default;
            return false;
        }

        BvhTree bvh = _bvhTree ?? BuildBvhTree();
        bool hitAnything = bvh.Hit(ray, tInterval, out hitInfo);
        float closestSoFar = hitAnything ? hitInfo.T : tInterval.Max;

        foreach (IHittable obj in _unboundedObjects)
        {
            if (obj.Hit(ray, new Interval(tInterval.Min, closestSoFar), out HitInfo tempHitInfo))
            {
                hitAnything = true;
                closestSoFar = tempHitInfo.T;
                hitInfo = tempHitInfo;
            }
        }

        return hitAnything;
    }
}
