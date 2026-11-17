using RayTracer.Lib.Core;

namespace RayTracer.Lib.Objects;

public readonly struct Aabb
{
    public static readonly Aabb Empty = new(Interval.Empty, Interval.Empty, Interval.Empty);
    public static readonly Aabb Universe = new(Interval.Universe, Interval.Universe, Interval.Universe);

    public Interval IntervalX { get; } = Interval.Empty;
    public Interval IntervalY { get; } = Interval.Empty;
    public Interval IntervalZ { get; } = Interval.Empty;

    public bool IsUnbounded =>
        float.IsNegativeInfinity(IntervalX.Min) || float.IsPositiveInfinity(IntervalX.Max) ||
        float.IsNegativeInfinity(IntervalY.Min) || float.IsPositiveInfinity(IntervalY.Max) ||
        float.IsNegativeInfinity(IntervalZ.Min) || float.IsPositiveInfinity(IntervalZ.Max);
    
    //Constructor taking only two points (two corners of a box), which is enough as it is Axis-Aligned
    public Aabb(Vec3 a, Vec3 b)
    {
        IntervalX = new Interval(MathF.Min(a.X, b.X), MathF.Max(a.X, b.X));
        IntervalY = new Interval(MathF.Min(a.Y, b.Y), MathF.Max(a.Y, b.Y));
        IntervalZ = new Interval(MathF.Min(a.Z, b.Z), MathF.Max(a.Z, b.Z));
    }
    
    //Constructor making a box covering two boxes
    public Aabb(Aabb a, Aabb b)
    {
        IntervalX = Interval.Enclose(a.IntervalX, b.IntervalX);
        IntervalY = Interval.Enclose(a.IntervalY, b.IntervalY);
        IntervalZ = Interval.Enclose(a.IntervalZ, b.IntervalZ);
    }

    public Aabb(Interval x, Interval y, Interval z)
    {
        IntervalX = x;
        IntervalY = y;
        IntervalZ = z;
    }
    
    public Aabb(Aabb other)
    {
        IntervalX = other.IntervalX;
        IntervalY = other.IntervalY;
        IntervalZ = other.IntervalZ;
    }

    public Aabb(float a, float b)
    {
        IntervalX = new Interval(a, b);
        IntervalY = new Interval(a, b);
        IntervalZ = new Interval(a, b);
    }

    public int LongestAxis()
    {
        float sizeX = IntervalX.Max - IntervalX.Min;
        float sizeY = IntervalY.Max - IntervalY.Min;
        float sizeZ = IntervalZ.Max - IntervalZ.Min;

        if (sizeX > sizeY)
            return sizeX > sizeZ ? 0 : 2;
        return sizeY > sizeZ ? 1 : 2;
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