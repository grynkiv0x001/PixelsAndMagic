using Microsoft.Xna.Framework;

namespace GameLibrary.Utilities;

public static class CollisionHelper
{
    public static bool Intersects(Circle circle, Rectangle rect)
    {
        var closestX = MathHelper.Clamp(circle.X, rect.Left, rect.Right);
        var closestY = MathHelper.Clamp(circle.Y, rect.Top, rect.Bottom);

        var dx = circle.X - closestX;
        var dy = circle.Y - closestY;

        return dx * dx + dy * dy <= circle.Radius * circle.Radius;
    }
}
