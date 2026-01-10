using System;
using GameLibrary.Graphics;
using GameLibrary.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelsAndMagic.Entities;

public class Enemy
{
    private const float MOVEMENT_SPEED = 5.0f;

    private readonly AnimatedSprite _enemySprite;

    private bool _isMoving;

    public Enemy(AnimatedSprite sprite, Vector2 position)
    {
        _enemySprite = sprite;
        Position = position;

        AssignRandomVelocity();
    }

    public Enemy(AnimatedSprite sprite, Vector2 position, float baseHealth, float baseDamage)
    {
        _enemySprite = sprite;
        Position = position;

        Health = baseHealth;
        BaseDamage = baseDamage;

        AssignRandomVelocity();
    }

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }

    public float Health { get; set; }
    public float BaseDamage { get; set; }

    public void Update(GameTime gameTime, Rectangle screenBounds)
    {
        var newPosition = Position + Velocity;
        var normal = Vector2.Zero;

        var enemyBounds = new Circle(
            (int)(newPosition.X + _enemySprite.Width * 0.5f),
            (int)(newPosition.Y + _enemySprite.Height * 0.5f),
            (int)(_enemySprite.Width * 0.5f)
        );

        // Using distance-based checks to determine if the Player is
        // within the bounds of the game screen & if not - move it back
        if (enemyBounds.Left < screenBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            Position = new Vector2(screenBounds.Left, Position.Y);
        }
        else if (enemyBounds.Right > screenBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            Position = new Vector2(screenBounds.Right - _enemySprite.Width, Position.Y);
        }

        if (enemyBounds.Top < screenBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            Position = new Vector2(Position.X, screenBounds.Top);
        }
        else if (enemyBounds.Bottom > screenBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            Position = new Vector2(Position.X, screenBounds.Bottom - _enemySprite.Height);
        }

        // If the normal is anything but Vector2.Zero, this means the enemy had
        // moved outside the screen edge so we should reflect it about the normal
        if (normal != Vector2.Zero) Velocity = Vector2.Reflect(Velocity, Vector2.Normalize(normal));

        Position = newPosition;

        _enemySprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _enemySprite.Draw(spriteBatch, Position);
    }

    private void AssignRandomVelocity()
    {
        var angle = (float)(Random.Shared.NextDouble() * 2 * Math.PI);

        var x = (float)Math.Cos(angle);
        var y = (float)Math.Sin(angle);

        var direction = new Vector2(x, y);

        Velocity = Vector2.Normalize(direction) * MOVEMENT_SPEED;
    }
}
