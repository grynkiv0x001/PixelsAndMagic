using GameLibrary.Graphics;
using GameLibrary.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelsAndMagic.Entities;

public class Projectile(
    Sprite sprite,
    Vector2 position,
    Vector2 velocity,
    float damage,
    bool isReversed = false
)
{
    public Circle CircleCollider => new(
        Position.X + sprite.Width * 0.5f,
        Position.Y + sprite.Height * 0.5f,
        sprite.Width * 0.5f
    );

    public Vector2 Position { get; private set; } = position;
    public Vector2 Velocity { get; set; } = velocity;
    public float Damage { get; set; } = damage;
    public bool IsAlive { get; set; } = true;

    public void Update(GameTime gameTime)
    {
        Position += Velocity;

        if (isReversed) sprite.Effects = SpriteEffects.FlipHorizontally;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Position);
    }
}
