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
        world.AddObject(new Sphere(new Vec3(0, 0, 0), 1000.0f, new EmissiveMaterial(new Vec3(0, 0.933f, 1))));
        world.AddObject(new Sphere(new Vec3(0, 0, -1), 0.6f, new EmissiveMaterial(new Vec3(1, 0, 0)))); 
        world.AddObject(new Plane(new Vec3(0, -0.5f, 0), new Vec3(0, 1, 0), new EmissiveMaterial(new Vec3(0.3f, 0.3f, 0.3f)))); 
        //world.AddObject(new Plane(new Vec3(0, -0.5f, 0), new Vec3(0, 1, 0)));

        Camera cam = new Camera(new Vec3(0, 0, 0), 90.0f, 16.0f / 9.0f)
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