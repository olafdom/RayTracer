namespace RayTracer.App;

class Program
{
    static void Main(string[] args)
    {
        Scene world = new Scene();
        world.AddObject(new Sphere(new Vec3(2, 1, -1), 0.5f)); 
        world.AddObject(new Sphere(new Vec3(1, 1, -1), 0.2f)); 
        world.AddObject(new Sphere(new Vec3(-1, -1, -1), 0.2f)); 
        world.AddObject(new Sphere(new Vec3(-2, -2, -1), 0.3f)); 
        //world.AddObject(new Plane(new Vec3(0, -0.5f, 0), new Vec3(0, 1, 0)));

        Camera cam = new Camera(new Vec3(0, 0, 0), 90.0f, 16.0f/9.0f);
        cam.Position = new Vec3(0, 0, 1);
        cam.Target = new Vec3(0, 0, -1);

        Image image = cam.Render(world);

        Console.WriteLine(image.ToPpm());
    }
}