using RayTracer.Lib.Core;

namespace RayTracer.Lib.Objects;

public class BvhTree : IHittable
{
    private class BvhNode : IHittable
    {
        private IHittable _left;
        private IHittable _right;
        public Aabb Aabb { get; }
        
        //Recursively builds a BVH Tree. Implementation guaranteeing that all leafs will be primitives
        public BvhNode(in Span<IHittable> objects)
        {
            //both children become the same primitive
            if (objects.Length == 1)
            {
                _left = _right = objects[0];
            }
            //only two primitives split to children
            else if (objects.Length == 2)
            {
                _left = objects[0];
                _right = objects[1];
            }
            else
            {
                //random axis which with respect to which we will sort primitives to try to cut scene in all dimensions
                int axis = Random.Shared.Next(0, 3);

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
            }

            Aabb = new (_left.Aabb, _right.Aabb);
        }
        
        public bool Hit(in Ray ray, Interval tInterval, out HitInfo hitInfo)
        {
            hitInfo = default;
            
            if (!Aabb.Hit(ray, tInterval))
                return false;
            
            bool hitLeft = _left.Hit(ray, tInterval, out hitInfo);
            if (_right.Hit(ray, new (tInterval.Min, hitLeft ? hitInfo.T : tInterval.Max), out HitInfo rightHitInfo))
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