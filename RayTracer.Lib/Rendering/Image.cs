using System.Text;
using RayTracer.Lib.Core;

namespace RayTracer.Lib.Rendering;

public class Image
{
    private int Width { get; }
    private int Height { get; }
    private Color[,] Pixels { get; }

    public Image(int width, int height)
    {
        Width = width;
        Height = height;
        Pixels = new Color[width, height];
    }

    public Color this[int x, int y]
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
                Color color = Pixels[x, y];
                
                (byte r, byte g, byte b) = color.ToRgb255();
        
                sb.Append($"{r} {g} {b}\t");
                sb.Append("\t");
            }
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}