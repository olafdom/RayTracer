using System.Text;
using RayTracer.Lib.Core;

namespace RayTracer.Lib.Rendering;

public class Image
{
    private int Width { get; }
    private int Height { get; }
    private Vec3[,] Pixels { get; }

    public Image(int width, int height)
    {
        Width = width;
        Height = height;
        Pixels = new Vec3[width, height];
    }

    public Vec3 this[int x, int y]
    {
        get => Pixels[x, y];
        set => Pixels[x, y] = value;
    }

    public string ToPpm()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("P3");
        sb.AppendLine($"{Width} {Height}");
        sb.AppendLine("255");

        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                Vec3 color = Pixels[x, y];
                
                int r = Math.Clamp((int)color.X, 0, 255);
                int g = Math.Clamp((int)color.Y, 0, 255);
                int b = Math.Clamp((int)color.Z, 0, 255);
        
                sb.Append($"{r} {g} {b}\t");
                sb.Append("\t");
            }
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}