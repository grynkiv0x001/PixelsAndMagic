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
    }

    public Enemy(AnimatedSprite sprite, Vector2 position, float baseHealth, float baseDamage)
    {
        _enemySprite = sprite;
        Position = position;

        Health = baseHealth;
        BaseDamage = baseDamage;
    }

    public Vector2 Position { get; set; }

    public float Health { get; set; }
    public float BaseDamage { get; set; }

    public void Update(GameTime gameTime, Rectangle screenBounds)
    {
        var enemyBounds = new Circle(
            (int)(Position.X + _enemySprite.Width * 0.5f),
            (int)(Position.Y + _enemySprite.Height * 0.5f),
            (int)(_enemySprite.Width * 0.5f)
        );

        // Using distance-based checks to determine if the Player is
        // within the bounds of the game screen & if not - move it back
        if (enemyBounds.Left < screenBounds.Left)
            Position = new Vector2(screenBounds.Left, Position.Y);
        else if (enemyBounds.Right > screenBounds.Right)
            Position = new Vector2(screenBounds.Right - _enemySprite.Width, Position.Y);

        if (enemyBounds.Top < screenBounds.Top)
            Position = new Vector2(Position.X, screenBounds.Top);
        else if (enemyBounds.Bottom > screenBounds.Bottom)
            Position = new Vector2(Position.X, screenBounds.Bottom - _enemySprite.Height);

        _enemySprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _enemySprite.Draw(spriteBatch, Position);
    }
}
