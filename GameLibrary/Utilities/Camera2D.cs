using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Utilities;

public class Camera2D
{
    private Vector2 _velocity;

    public Vector2 Position { get; set; }

    public float Zoom { get; set; } = 1.0f;

    public int BoundaryPadding { get; set; } = 0;

    public Rectangle WorldBounds { get; set; }

    public Matrix GetViewMatrix()
    {
        return
            Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
            Matrix.CreateScale(Zoom, Zoom, 1.0f);
    }

    public void Follow(
        Vector2 target,
        Viewport viewport,
        float smoothing = 0.12f
    )
    {
        // Desired camera position (center target)
        var targetPosition = target - new Vector2(
            viewport.Width * 0.5f / Zoom,
            viewport.Height * 0.5f / Zoom
        );

        // Smoothly move towards target
        Position = Vector2.Lerp(Position, targetPosition, smoothing);

        ClampToWorld(viewport);
    }

    private void ClampToWorld(Viewport viewport)
    {
        var left = WorldBounds.Left - BoundaryPadding;
        var top = WorldBounds.Top - BoundaryPadding;
        var right = WorldBounds.Right + BoundaryPadding;
        var bottom = WorldBounds.Bottom + BoundaryPadding;

        var maxX = right - viewport.Width / Zoom;
        var maxY = bottom - viewport.Height / Zoom;

        Position = new Vector2(
            MathHelper.Clamp(Position.X, left, maxX),
            MathHelper.Clamp(Position.Y, top, maxY)
        );
    }
}
