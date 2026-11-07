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
        world.AddObject(new Sphere(new (0, 0, 0), 1000.0f, new EmissiveMaterial(new (0.5f, 0.7f, 1.0f))));
        world.AddObject(new Sphere(new (0, 0, -1), 0.5f, new Lambertian(new (1, 0, 0)))); 
        world.AddObject(new Sphere(new (-1.1f, 0, -1), 0.5f, new Lambertian(new (0, 0, 1)))); 
        world.AddObject(new Sphere(new (1.1f, 0, -1), 0.5f, new Lambertian(new (1, 1, 0)))); 
        world.AddObject(new Plane(new (0, -0.5f, 0), new (0, 1, 0), new Lambertian(new (0.2f, 0.8f, 0.2f))));
        //world.AddObject(new Plane(new Vec3(0, -0.5f, 0), new Vec3(0, 1, 0)));

        Camera cam = new Camera(new Vec3(0, 0, 2f), 90.0f, 16.0f / 9.0f)
        {
            Target = new Vec3(0, 0, -1)
        };

        Renderer renderer = new Renderer
        {
            MaxDepth = 50
        };
        
        Image image = renderer.Render(cam, world);
        Console.WriteLine(image.ToPpm());
    }
}