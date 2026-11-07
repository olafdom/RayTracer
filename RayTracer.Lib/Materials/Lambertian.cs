using RayTracer.Lib.Core;
using RayTracer.Lib.Objects;

namespace RayTracer.Lib.Materials;

public class Lambertian : Material
{
    private Vec3 Albedo { get; }

    public Lambertian(Vec3 albedo)
    {
        Albedo = albedo;
    }
    
    public override bool Scatter(in Ray rIn, ref HitInfo hitInfo, out Vec3 attenuation, out Ray scattered)
    {
        Vec3 randomDirection = Vec3.RandomUnitVector();
            
        Vec3 scatterDirection = hitInfo.Normal + randomDirection;
        
        if (scatterDirection.NearZero())
            scatterDirection = hitInfo.Normal;
        
        scattered = new Ray(hitInfo.Position, scatterDirection.Normalize());
        attenuation = Albedo;
        
        return true;
    }
}