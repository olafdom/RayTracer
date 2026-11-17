using RayTracer.Lib.Core;

namespace RayTracer.Lib.Objects;

public class BvhTree : IHittable
{
    private class BvhNode : IHittable
    {
        private readonly IHittable _left;
        private readonly IHittable? _right;
        public Aabb Aabb { get; }
        
        //Recursively builds a BVH Tree. Implementation guaranteeing that all leafs will be primitives
        public BvhNode(in Span<IHittable> objects)
        {
            if (objects.Length == 1)
            {
                _left = objects[0];
                _right = null;
                Aabb = _left.Aabb;
                return;
            }

            if (objects.Length == 2)
            {
                _left = objects[0];
                _right = objects[1];
                Aabb = new(_left.Aabb, _right.Aabb);
                return;
            }

            Aabb bounds = Aabb.Empty;
            foreach (IHittable obj in objects)
                bounds = new Aabb(bounds, obj.Aabb);

            int axis = bounds.LongestAxis();

            Comparison<IHittable> comparator = axis switch
            {
                0 => (a, b) => a.Aabb.IntervalX.Min.CompareTo(b.Aabb.IntervalX.Min),
                1 => (a, b) => a.Aabb.IntervalY.Min.CompareTo(b.Aabb.IntervalY.Min),
                _ => (a, b) => a.Aabb.IntervalZ.Min.CompareTo(b.Aabb.IntervalZ.Min)
            };

            objects.Sort(comparator);

            int mid = objects.Length / 2;

            _left = new BvhNode(objects.Slice(0, mid));
            _right = new BvhNode(objects.Slice(mid));
            Aabb = new(_left.Aabb, _right.Aabb);
        }
        
        public bool Hit(in Ray ray, Interval tInterval, out HitInfo hitInfo)
        {
            hitInfo = default;
            
            if (!Aabb.Hit(ray, tInterval))
                return false;
            
            bool hitLeft = _left.Hit(ray, tInterval, out hitInfo);
            if (_right is not null && _right.Hit(ray, new (tInterval.Min, hitLeft ? hitInfo.T : tInterval.Max), out HitInfo rightHitInfo))
            {
                hitInfo = rightHitInfo;
                return true;
            }

            return hitLeft;
        }
    }

    private readonly BvhNode? _root;

    public BvhTree(in Span<IHittable> objects)
    {
        if (objects.Length > 0)
            _root = new BvhNode(objects);
    }

    public Aabb Aabb => _root?.Aabb ?? Aabb.Empty; 
    
    public bool Hit(in Ray ray, Interval tInterval, out HitInfo hitInfo)
    {
        if (_root is null)
        {
            hitInfo = default;
            return false;
        }

        return _root.Hit(ray, tInterval, out hitInfo);
    }
}