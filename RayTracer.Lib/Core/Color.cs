namespace RayTracer.Lib.Core;

public readonly struct Color
{
    public float R { get; }
    public float G { get; }
    public float B { get; }

    public Color(float r, float g, float b)
    {
        R = r;
        G = g;
        B = b;
    }
    
    public Color(Color color)
    {
        R = color.R;
        G = color.G;
        B = color.B;
    }
    
    public static Color operator -(Color a) => new Color(-a.R, -a.G, -a.B);
    
    public static Color operator +(Color a, Color b) => new Color(a.R + b.R, a.G + b.G, a.B + b.B);
    
    public static Color operator -(Color a, Color b) => new Color(a.R - b.R, a.G - b.G, a.B - b.B);
    
    public static Color operator *(float a, Color v) => new Color(a * v.R, a * v.G, a * v.B);
    
    public static Color operator *(Color v, float a) => new Color(v.R * a, v.G * a, v.B * a);
    
    public static Color operator *(Color a, Color b) => new Color(a.R * b.R, a.G * b.G, a.B * b.B);
    
    public static Color operator /(Color v, float a)
    {
        float inv = 1.0f / a;
        return new Color(v.R * inv, v.G * inv, v.B * inv);
    }
    
    public static Color operator *(Vec3 v, Color c)  => new Color(v.X * c.R, v.Y * c.G, v.Z * c.B);
    
    public static Color operator *(Color c, Vec3 v) => new Color(c.R * v.X, c.G * v.Y, c.B * v.Z);
}