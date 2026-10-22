namespace RayTracer.App;

public class Camera
{
    private float _aspectRatio = (16.0f / 9.0f);
    private int _imageWidth = 1280;
    private int _imageHeight; //Will be calculated in Recalculate()
    private Vec3 _position = new Vec3(0, 0, 0);
    private Vec3 _target = new Vec3(0, 0, -1);
    private float _fov = 90.0f;
    private Vec3 _pixelDy; //Will be calculated in Recalculate()
    private Vec3 _pixelDx; //Will be calculated in Recalculate()
    private Vec3 _viewportCorner; //Will be calculated in Recalculate()

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

    public Image Render(IHittable world)
    {
        Image img = new Image(_imageWidth, _imageHeight);
        for (int y = 0; y < _imageHeight; ++y)
        {
            for (int x = 0; x < _imageWidth; ++x)
            {
                Vec3 pixelCenter = _viewportCorner + (y * _pixelDy) + (x * _pixelDx);
                Vec3 rayDirection = pixelCenter - Position;
                Ray ray = new Ray(Position, rayDirection);
                if (world.Hit(ray, 0.001f, float.PositiveInfinity, out HitInfo hitInfo))
                {
                    float minDistance = 1.0f;
                    float maxDistance = 3.5f;
                    float distance = (hitInfo.Position - Position).Length;
                    float ratio = (distance - minDistance) / (maxDistance - minDistance);
                    float intensity = Math.Clamp(1f - ratio, 0f, 1f) * 255f;
                    img[x, y] = new Vec3((int)intensity, 0, 0);
                }
                else
                {
                    img[x, y] = new Vec3(0, 0, 0); 
                }
            }
        }
        return img;
    }

    public Camera(Vec3 position, float fov, float aspectRatio)
    {
        Position = position;
        Fov = fov;
        AspectRatio = aspectRatio;
        Recalculate();
    }
}