using RayTracer.Lib.Core;
using RayTracer.Lib.Objects;

namespace RayTracer.Lib.Rendering;

public class Renderer
{
    public int MaxDepth { get; set; } = 50;
    
    public Image Render(Camera camera, IHittable world)
    {
        Image img = new Image(camera.ImageWidth, camera.ImageHeight);
        
        for (int y = 0; y < camera.ImageHeight; ++y)
        {
            for (int x = 0; x < camera.ImageWidth; ++x)
            {
                Ray ray = camera.GetRay(x, y);
                
                img[x, y] = RayColor(ray, MaxDepth, world) * 255f; 
            }
        }
        return img;
    }
    
    private static Vec3 RayColor(in Ray ray, int depth, IHittable world)
    {
        if (depth <= 0)
            return new Vec3(0, 0, 0);
        
        if (!world.Hit(in ray, 0.001f, float.PositiveInfinity, out HitInfo hitInfo))
            return new Vec3(0, 0, 0);
        
        Vec3 emitted = hitInfo.Material.Emit(); 
        
        if (hitInfo.Material.Scatter(in ray, ref hitInfo, out Vec3 attenuation, out Ray scattered))
        {
            Vec3 scatteredColor = RayColor(scattered, depth - 1, world);
            return emitted + (attenuation * scatteredColor);
        }
        
        return emitted;
    }
}