using GameLibrary.Graphics;
using GameLibrary.Input;
using GameLibrary.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelsAndMagic.Entities;

public class Player
{
    private const float MOVEMENT_SPEED = 5.0f;

    private readonly InputManager _inputManager;
    private readonly AnimatedSprite _playerSprite;

    private bool _isMoving;
    private bool _isReversed;

    public Player(AnimatedSprite playerSprite, InputManager inputManager)
    {
        _playerSprite = playerSprite;
        _inputManager = inputManager;
        Position = new Vector2(0, 0);
    }

    public Player(AnimatedSprite playerSprite, InputManager inputManager, Vector2 position)
    {
        _playerSprite = playerSprite;
        _inputManager = inputManager;
        Position = position;
    }

    public Player(AnimatedSprite playerSprite, InputManager inputManager, Vector2 startPosition, float baseHealth,
        float baseDamage)
    {
        _playerSprite = playerSprite;
        _inputManager = inputManager;

        Position = startPosition;
        Health = baseHealth;
        BaseDamage = baseDamage;
    }

    public float Health { get; set; }
    public float BaseDamage { get; set; }

    public Vector2 Position { get; set; }

    public Circle Collider => new(
        Position.X + _playerSprite.Width * 0.5f,
        Position.Y + _playerSprite.Height * 0.5f,
        _playerSprite.Width * 0.5f
    );

    public void Update(GameTime gameTime, Rectangle screenBounds)
    {
        HandleKeyboardInput();
        HandleCollision(screenBounds);

        if (_isReversed)
            _playerSprite.Effects = SpriteEffects.FlipHorizontally;
        else
            _playerSprite.Effects = SpriteEffects.None;

        _playerSprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _playerSprite.Draw(spriteBatch, Position);
    }

    public void HandleEntityCollision(Circle target)
    {
        var self = Collider;

        if (!self.Intersects(Collider))
            return;

        var direction = self.Center - target.Center;

        if (direction == Vector2.Zero)
            direction = Vector2.UnitX;

        direction.Normalize();

        var distance = Vector2.Distance(self.Center, target.Center);
        var penetration = self.Radius + target.Radius - distance;

        Position += direction * penetration;
    }

    private void HandleCollision(Rectangle boundaries)
    {
        var playerBounds = new Circle(
            Position.X + _playerSprite.Width * 0.5f,
            Position.Y + _playerSprite.Height * 0.5f,
            _playerSprite.Width * 0.5f
        );

        // Using distance-based checks to determine if the Player is
        // within the bounds of the game screen & if not - move it back
        if (playerBounds.Left < boundaries.Left)
            Position = new Vector2(boundaries.Left, Position.Y);
        else if (playerBounds.Right > boundaries.Right)
            Position = new Vector2(boundaries.Right - _playerSprite.Width, Position.Y);

        if (playerBounds.Top < boundaries.Top)
            Position = new Vector2(Position.X, boundaries.Top);
        else if (playerBounds.Bottom > boundaries.Bottom)
            Position = new Vector2(Position.X, boundaries.Bottom - _playerSprite.Height);
    }

    private void HandleKeyboardInput()
    {
        var speed = MOVEMENT_SPEED;

        if (_inputManager.Keyboard.IsKeyDown(Keys.Up))
        {
            Position += new Vector2(0, -speed);
            _isMoving = true;
        }

        if (_inputManager.Keyboard.IsKeyDown(Keys.Down))
        {
            Position += new Vector2(0, speed);
            _isMoving = true;
        }

        if (_inputManager.Keyboard.IsKeyDown(Keys.Left))
        {
            Position += new Vector2(-speed, 0);
            _isMoving = true;
            _isReversed = true;
        }

        if (_inputManager.Keyboard.IsKeyDown(Keys.Right))
        {
            Position += new Vector2(speed, 0);
            _isMoving = true;
            _isReversed = false;
        }

        if (_inputManager.Keyboard.IsKeyReleased(Keys.Up) || _inputManager.Keyboard.IsKeyReleased(Keys.Down) ||
            _inputManager.Keyboard.IsKeyReleased(Keys.Left) || _inputManager.Keyboard.IsKeyReleased(Keys.Right))
            _isMoving = false;
    }
}
