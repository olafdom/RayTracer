namespace RayTracer.Lib.Core;

public readonly struct Vec3
{
    public float X { get; }
    public float Y { get; }
    public float Z { get; }

    public Vec3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    public Vec3(Vec3 vec3)
    {
        X = vec3.X;
        Y = vec3.Y;
        Z = vec3.Z;
    }
    
    public override string ToString() => $"{X} {Y} {Z}";
    
    public float LengthSquared => (X * X) + (Y * Y) + (Z * Z);
    
    public float Length => MathF.Sqrt(LengthSquared);
    
    public Vec3 Normalize()
    {
        float len = Length;
        if (len > 1e-6f)
        {
            float invLen = 1.0f / len;
            return new Vec3(X * invLen, Y * invLen, Z * invLen);
        }
        return new Vec3(0, 0, 0);
    }
    
    public static Vec3 operator -(Vec3 a) => new Vec3(-a.X, -a.Y, -a.Z);
    
    public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    
    public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    
    public static Vec3 operator *(float a, Vec3 v) => new Vec3(a * v.X, a * v.Y, a * v.Z);
    
    public static Vec3 operator *(Vec3 v, float a) => new Vec3(v.X * a, v.Y * a, v.Z * a);
    
    public static Vec3 operator *(Vec3 a, Vec3 b) => new Vec3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    
    public static Vec3 operator /(Vec3 v, float a)
    {
        float inv = 1.0f / a;
        return new Vec3(v.X * inv, v.Y * inv, v.Z * inv);
    }
    
    public static float DotProduct(Vec3 a, Vec3 b) => (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
    
    public static Vec3 CrossProduct(Vec3 a, Vec3 b)
    {
        return new Vec3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        );
    }
}