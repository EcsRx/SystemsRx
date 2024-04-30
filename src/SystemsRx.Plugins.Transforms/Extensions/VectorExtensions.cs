using System;
using System.Numerics;

namespace SystemsRx.Plugins.Transforms.Extensions
{
    public static class VectorExtensions
    {
        public static float ToAngle(this Vector2 vector)
        { return MathF.Atan2(vector.Y, vector.X) * Transform2DExtensions.RadiansToDegrees; }
    }
}