using RayTracer.Lib.Core;

namespace RayTracer.Lib.Objects;

public readonly struct AABB
{
    public Interval IntervalX { get; }
    public Interval IntervalY { get; }
    public Interval IntervalZ { get; }

    public AABB(Interval x, Interval y, Interval z)
    {
        IntervalX = x;
        IntervalY = y;
        IntervalZ = z;
    }
    
    public AABB(AABB other)
    {
        IntervalX = other.IntervalX;
        IntervalY = other.IntervalY;
        IntervalZ = other.IntervalZ;
    }

    public AABB(float a, float b)
    {
        IntervalX = new Interval(a, b);
        IntervalY = new Interval(a, b);
        IntervalZ = new Interval(a, b);
    }

    public bool Hit(in Ray ray, Interval tInterval)
    {
        /*
         * Consider each dimension as an interval. Check for which t the given ray is inside each dimension's interval
         * by the formula: t0 = (x0 - Q_x) / d_x, t1 = (x1 - Q_x) / d_x 
         * If all the intervals intersect that means that for some t the ray is inside all intervals => HIT
         */
        
        //X Axis
        float invD = 1 / ray.Direction.X;
        float t0 = (IntervalX.Min - ray.Origin.X) * invD;
        float t1 = (IntervalX.Max - ray.Origin.X) * invD;
        
        
        if (invD < 0.0f) 
        { 
            (t0, t1) = (t1, t0);
        }

        Interval interval = new(t0, t1);

        interval = Interval.Intersect(interval, tInterval);
        if (interval.Min > interval.Max)
            return false;
        
        //Y Axis
        invD = 1 / ray.Direction.Y;
        t0 = (IntervalY.Min - ray.Origin.Y) * invD;
        t1 = (IntervalY.Max - ray.Origin.Y) * invD;
        
        
        if (invD < 0.0f) 
        { 
            (t0, t1) = (t1, t0);
        }

        interval = interval.Intersect(t0, t1);
        
        if (interval.Min > interval.Max)
            return false;
        
        //Z Axis
        invD = 1 / ray.Direction.Z;
        t0 = (IntervalZ.Min - ray.Origin.Z) * invD;
        t1 = (IntervalZ.Max - ray.Origin.Z) * invD;
        
        
        if (invD < 0.0f) 
        { 
            (t0, t1) = (t1, t0);
        }

        interval = interval.Intersect(t0, t1);
        
        if (interval.Min > interval.Max)
            return false;

        return true;
    }
}