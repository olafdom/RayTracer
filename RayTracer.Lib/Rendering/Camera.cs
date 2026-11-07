using RayTracer.Lib.Core;

namespace RayTracer.Lib.Rendering;

public class Camera
{
    public int MaxDepth { get; set; } = 50;
    public int SamplesPerPixel { get; set; } = 100;
    private float _maxOffsetX = 0.5f;
    private float _maxOffsetY = 0.5f;
    private float _aspectRatio = (16.0f / 9.0f);
    private int _imageWidth = 1280;
    private int _imageHeight; //Calculated in Recalculate()
    private Vec3 _position = new Vec3(0, 0, 0);
    private Vec3 _target = new Vec3(0, 0, -1);
    private float _fov = 90.0f;
    private Vec3 _pixelDy; //Calculated in Recalculate()
    private Vec3 _pixelDx; //Calculated in Recalculate()
    private Vec3 _viewportCorner; //Calculated in Recalculate()
    
    public Camera(Vec3 position, float fov, float aspectRatio)
    {
        Position = position;
        Fov = fov;
        AspectRatio = aspectRatio;
        Recalculate();
    }

    public float AspectRatio
    {
        get => _aspectRatio;
        set
        {
            _aspectRatio = value;
            Recalculate();
        }
    }
    
    public int ImageWidth
    {
        get => _imageWidth;
        set
        {
            _imageWidth = value;
            Recalculate();
        }
    }
    
    public int ImageHeight
    {
        get => _imageHeight;
        set
        {
            _imageHeight = value;
            Recalculate();
        }
    }
    
    public Vec3 Position
    {
        get => _position;
        set
        {
            _position = value;
            Recalculate();
        }
    }
    
    public Vec3 Target
    {
        get => _target;
        set
        {
            _target = value;
            Recalculate();
        }
    }

    public float Fov
    {
        get => _fov;
        set
        {
            _fov = value;
            Recalculate();
        }
    }
    
    private float FovInRadians => _fov * MathF.PI / 180.0f;
    private float DistanceToViewport => (_position - _target).Length;

    private void Recalculate()
    {
        _imageHeight = (int)(ImageWidth / AspectRatio);
        
        float viewportHeight = 2.0f * MathF.Tan(FovInRadians / 2) * DistanceToViewport;
        float viewportWidth = viewportHeight * _aspectRatio;
        
        Vec3 vForward = (_position - _target).Normalize();
        Vec3 vRight = Vec3.CrossProduct(new Vec3(0, 1, 0), vForward).Normalize();
        Vec3 vUp = Vec3.CrossProduct(vForward, vRight).Normalize();
        
        Vec3 viewportY = viewportHeight * -vUp;
        Vec3 viewportX = viewportWidth * vRight;
        
        _pixelDy = viewportY / _imageHeight;
        _pixelDx = viewportX / _imageWidth;
        
        Vec3 viewportUpperLeft = _position - (DistanceToViewport * vForward) - viewportY / 2.0f - viewportX / 2.0f;
        
        _viewportCorner = viewportUpperLeft + 0.5f * (_pixelDy + _pixelDx);
    }
    
    public Ray GetRay(int x, int y)
    {
        float offsetX = Random.Shared.NextSingle() - _maxOffsetX;
        float offsetY = Random.Shared.NextSingle() - _maxOffsetY;
        
        //Vec3 pixelCenter = _viewportCorner + (y * _pixelDy) + (x * _pixelDx);
        //Casting a ray with some random offset for antyaliasing:
        Vec3 pixelCenterWithOffset = _viewportCorner + ((y + offsetY) * _pixelDy) + ((x + offsetX) * _pixelDx);
        Vec3 rayDirection = pixelCenterWithOffset - Position;
        
        return new Ray(Position, rayDirection);
    }
}