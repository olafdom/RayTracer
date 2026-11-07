using RayTracer.Lib.Core;
using RayTracer.Lib.Objects;

namespace RayTracer.Lib.Materials;

public class Metal : Material
{
    private Vec3 Albedo { get; }
    private float Fuzz { get; }
    
    public Metal(Vec3 albedo, float fuzz = 0.5f)
    {
        Albedo = albedo;
        Fuzz = fuzz;
    }
    
    public override bool Scatter(in Ray rIn, ref HitInfo hitInfo, out Vec3 attenuation, out Ray scattered)
    {
        attenuation = Albedo;
        
        //Adding a (unit vector * fuzziness) at the endpoint of a reflected ray to add a little randomness to a reflection
        scattered = new Ray(hitInfo.Position, (Vec3.Reflect(rIn.Direction, hitInfo.Normal) + (Fuzz * Vec3.RandomUnitVector())).Normalize());
        
        /*
         Prevent scattering below surface, as the unit sphere is centered at the end of a reflected vector, 
         a part of it can be below surface and the random vector could make the reflection go below surface
        */
        return Vec3.DotProduct(scattered.Direction, hitInfo.Normal) > 0;
    }
}