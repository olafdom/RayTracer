namespace RayTracer.Lib.Core;

public readonly struct Interval
{
    public float Min { get; }
    public float Max { get; }

    public Interval(float min, float max)
    {
        Min = min;
        Max = max;
    }
}