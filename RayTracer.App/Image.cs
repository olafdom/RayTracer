namespace RayTracer.App;
using System.Text;

public class Image
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Vec3[,] Pixels { get; private set; }

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
                sb.Append(Pixels[x, y]);
                sb.Append("\t");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}