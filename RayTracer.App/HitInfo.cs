namespace RayTracer.App;

public struct HitInfo
{
    public Material Material;
    public Vec3 Position;
    public Vec3 Normal;
    public float T; //t parameter of a Ray
    public bool FrontFace; //Ray hits from outside - true, otherwise - false 

    public void SetFaceNormal(Ray r, Vec3 OutwardNormal)
    {
        if (Vec3.DotProduct(r.Direction, OutwardNormal) <= 0)
        {
            FrontFace = true;
            Normal = OutwardNormal;
        }
        else
        {
            FrontFace = false;
            Normal = -OutwardNormal;
        }
    }
}