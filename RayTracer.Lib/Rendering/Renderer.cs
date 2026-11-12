using RayTracer.Lib.Core;
using RayTracer.Lib.Objects;
using System.Threading.Tasks;

namespace RayTracer.Lib.Rendering;

public class Renderer
{
    public int MaxDepth { get; set; } = 50;
    
    public Image Render(Camera camera, IHittable world)
    {
        Image img = new Image(camera.ImageWidth, camera.ImageHeight);

        Parallel.For(0, camera.ImageHeight, y =>
        {
            for (int x = 0; x < camera.ImageWidth; ++x)
            {
                Color colorSum = new Color(0, 0, 0);
                for (int sample = 0; sample < camera.SamplesPerPixel; ++sample)
                {
                    Ray ray = camera.GetRay(x, y);

                    colorSum += RayColor(in ray, MaxDepth, world);
                }

                img[x, y] = (colorSum / camera.SamplesPerPixel);
            }
        });
        return img;
    }
    
    private static Color RayColor(in Ray ray, int depth, IHittable world)
    {
        if (depth <= 0)
            return new Color(0, 0, 0);
        
        if (!world.Hit(in ray, new (0.001f, float.PositiveInfinity), out HitInfo hitInfo))
            return new Color(0, 0, 0);
        
        Color emitted = hitInfo.Material.Emit(); 
        
        if (hitInfo.Material.Scatter(in ray, ref hitInfo, out Vec3 attenuation, out Ray scattered))
        {
            Color scatteredColor = RayColor(scattered, depth - 1, world);
            return emitted + (attenuation * scatteredColor);
        }
        
        return emitted;
    }
}