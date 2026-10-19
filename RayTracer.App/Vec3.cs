namespace RayTracer.App;

public struct Vec3
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Vec3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString()
    {
        return $"{X} {Y} {Z}";
    }
}