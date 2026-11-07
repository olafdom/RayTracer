using RayTracer.Lib.Core;
using RayTracer.Lib.Materials;

namespace RayTracer.Lib.Objects;

public struct HitInfo
{
    public Material Material { get; set; }
    public Vec3 Position { get; set; }
    public Vec3 Normal { get; set; }
    public float T { get; set; }
    public bool FrontFace { get; set; }

    public void SetFaceNormal(in Ray r, Vec3 outwardNormal)
    {
        if (Vec3.DotProduct(r.Direction, outwardNormal) <= 0)
        {
            FrontFace = true;
            Normal = outwardNormal;
        }
        else
        {
            FrontFace = false;
            Normal = -outwardNormal;
        }
    }
}