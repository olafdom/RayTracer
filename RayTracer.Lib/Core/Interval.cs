namespace RayTracer.Lib.Core;

public readonly struct Interval
{
    public float Min { get; }
    public float Max { get; }

    public Interval()
    {
        this = Empty;
    }

    public Interval(float min, float max)
    {
        Min = min;
        Max = max;
    }
    
    public static readonly Interval Empty = new (float.PositiveInfinity, float.NegativeInfinity);
    public static readonly Interval Universe = new (float.NegativeInfinity, float.PositiveInfinity);

    public static Interval Intersect(Interval a, Interval b)
    {
        return new Interval(MathF.Max(a.Min, b.Min), MathF.Min(a.Max, b.Max));
    }
    
    public Interval Intersect(float min, float max)
    {
        return new Interval(MathF.Max(min, Min), MathF.Min(max, Max));
    }
}