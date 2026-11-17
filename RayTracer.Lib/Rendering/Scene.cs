using System.Runtime.InteropServices;
using RayTracer.Lib.Core;
using RayTracer.Lib.Objects;

namespace RayTracer.Lib.Rendering;

public class Scene : IHittable
{
    public List<IHittable> Objects { get; } = new();
    
    public Aabb Aabb { get; set; } = Aabb.Empty;
    
    private volatile BvhTree? _bvhTree;
    private readonly object _bvhLock = new();

    private BvhTree BuildBvhTree()
    {
        lock (_bvhLock)
        {
            return _bvhTree ??= new BvhTree(CollectionsMarshal.AsSpan(Objects));
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
        return bvh.Hit(ray, tInterval, out hitInfo);
    }
}
