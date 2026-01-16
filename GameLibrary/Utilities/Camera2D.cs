using Microsoft.Xna.Framework;

namespace GameLibrary.Utilities;

public class Camera2D
{
    public Vector2 Position { get; set; }
    public float Zoom { get; set; } = 1.0f;

    public Matrix GetViewMatrix()
    {
        return
            Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
            Matrix.CreateScale(Zoom, Zoom, 1.0f);
    }
}
