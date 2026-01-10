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

    public void Update(GameTime gameTime, Rectangle screenBounds)
    {
        HandleKeyboardInput();

        var playerBounds = new Circle(
            (int)(Position.X + _playerSprite.Width * 0.5f),
            (int)(Position.Y + _playerSprite.Height * 0.5f),
            (int)(_playerSprite.Width * 0.5f)
        );

        // Using distance-based checks to determine if the Player is
        // within the bounds of the game screen & if not - move it back
        if (playerBounds.Left < screenBounds.Left)
            Position = new Vector2(screenBounds.Left, Position.Y);
        else if (playerBounds.Right > screenBounds.Right)
            Position = new Vector2(screenBounds.Right - _playerSprite.Width, Position.Y);

        if (playerBounds.Top < screenBounds.Top)
            Position = new Vector2(Position.X, screenBounds.Top);
        else if (playerBounds.Bottom > screenBounds.Bottom)
            Position = new Vector2(Position.X, screenBounds.Bottom - _playerSprite.Height);

        _playerSprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _playerSprite.Draw(spriteBatch, Position);
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
        }

        if (_inputManager.Keyboard.IsKeyDown(Keys.Right))
        {
            Position += new Vector2(speed, 0);
            _isMoving = true;
        }

        if (_inputManager.Keyboard.IsKeyReleased(Keys.Up) || _inputManager.Keyboard.IsKeyReleased(Keys.Down) ||
            _inputManager.Keyboard.IsKeyReleased(Keys.Left) || _inputManager.Keyboard.IsKeyReleased(Keys.Right))
            _isMoving = false;
    }
}
