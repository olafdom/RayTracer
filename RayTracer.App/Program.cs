using RayTracer.Lib.Core;
using RayTracer.Lib.Materials;
using RayTracer.Lib.Objects;
using RayTracer.Lib.Rendering;

namespace RayTracer.App;

class Program
{
    static void Main(string[] args)
    {
        Scene world = new Scene();
        world.AddObject(new Sphere(new (0, 0, 0), 100.0f, new EmissiveMaterial(color:new (0.7f, 0.8f, 1.0f), intensity:1.5f))); //SKY
        world.AddObject(new Sphere(new (50, 100, 50), 50.0f, new EmissiveMaterial(color:new (1.0f, 0.8f, 0.5f), intensity:10f))); //SUN
        world.AddObject(new Sphere(new (0, 0, -1), 0.5f, new Lambertian(new (1, 0, 0)))); 
        world.AddObject(new Sphere(new (-1.1f, 0, -1), 0.5f, new Metal(new Vec3(0.8f, 0.6f, 0.2f)))); 
        world.AddObject(new Sphere(new (1.1f, 0, -1), 0.5f, new Lambertian(new (0.851f, 0.231f, 0.859f)))); 
        world.AddObject(new Sphere(new (0, 4, -8), 8f, new Lambertian(new (1f, 0.765f, 0.835f)))); 
        world.AddObject(new Plane(new (0, -0.5f, 0), new (0, 1, 0), new Lambertian(new (0.2f, 0.8f, 0.2f))));

        Camera cam = new Camera(new Vec3(-1.5f, 0, 2f), 90.0f, 16.0f / 9.0f)
        {
            Fov = 90.0f,
            SamplesPerPixel = 100,
            Target = new Vec3(-0.3f, 0, -1.5f)
        };

        Renderer renderer = new Renderer
        {
            MaxDepth = 50
        };
        
        Image image = renderer.Render(cam, world);
        Console.WriteLine(image.ToPpm());
    }
}