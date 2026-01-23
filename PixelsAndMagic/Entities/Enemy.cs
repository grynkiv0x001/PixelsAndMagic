using System;
using GameLibrary.Graphics;
using GameLibrary.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelsAndMagic.Entities;

public enum EnemyState
{
    Alive,
    Dying,
    Dead
}

public class Enemy
{
    private const float MOVEMENT_SPEED = 5.0f;
    private const float HIT_FLASH_DURATION = 0.2f;
    private const float DYING_DURATION = 0.25f;

    private readonly AnimatedSprite _enemySprite;

    private float _dyingTimer;
    private float _hitFlashTimer;

    private bool _isMoving;

    public Enemy(AnimatedSprite sprite, Vector2 position, bool freerun = true)
    {
        _enemySprite = sprite;
        Position = position;

        if (freerun)
            AssignRandomVelocity();
    }

    public Enemy(AnimatedSprite sprite, Vector2 position, float baseHealth, float baseDamage, bool freerun = true)
    {
        _enemySprite = sprite;
        Position = position;

        Health = baseHealth;
        BaseDamage = baseDamage;

        if (freerun)
            AssignRandomVelocity();
    }

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }

    public Color Tint { get; set; } = Color.White;

    public float Health { get; set; }
    public float BaseDamage { get; set; }

    public EnemyState State { get; private set; } = EnemyState.Alive;

    public Circle Collider => new(
        Position.X + _enemySprite.Width * 0.5f,
        Position.Y + _enemySprite.Height * 0.5f,
        _enemySprite.Width * 0.5f
    );

    public event Action<Enemy> OnDeath;

    public void Update(GameTime gameTime, Rectangle screenBounds)
    {
        var newPosition = Position + Velocity;
        var normal = Vector2.Zero;

        var enemyBounds = new Circle(
            newPosition.X + _enemySprite.Width * 0.5f,
            newPosition.Y + _enemySprite.Height * 0.5f,
            _enemySprite.Width * 0.5f
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

        if (State == EnemyState.Dying)
        {
            var alpha = _dyingTimer / DYING_DURATION;

            // Stop movement
            Velocity = Vector2.Zero;

            _dyingTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            Tint = Color.Gray * alpha;

            if (_dyingTimer <= 0f)
                State = EnemyState.Dead;
        }
        else if (_hitFlashTimer > 0f)
        {
            _hitFlashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            Tint = Color.IndianRed;
        }
        else
        {
            Tint = Color.White;
        }

        if (Health <= 0 && State == EnemyState.Alive)
            Die();

        _enemySprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _enemySprite.Draw(spriteBatch, Position, Tint);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        _hitFlashTimer = HIT_FLASH_DURATION;
    }

    private void Die()
    {
        State = EnemyState.Dying;
        _dyingTimer = DYING_DURATION;

        OnDeath?.Invoke(this);
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
