using Microsoft.Xna.Framework;

using GameLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace PixelsAndMagic.Entities;

public class Player
{
    private readonly AnimatedSprite _playerSprite;
    
    public float Health { get; set; }
    public float BaseDamage { get; set; }
    
    public Vector2 Position { get; set; }

    public Player(AnimatedSprite playerSprite)
    {
        _playerSprite = playerSprite;
        Position = new Vector2(0, 0);
    }

    public Player(AnimatedSprite playerSprite, Vector2 position)
    {
        _playerSprite = playerSprite;
        Position = position;
    }

    public Player(AnimatedSprite playerSprite, Vector2 startPosition, float baseHealth, float baseDamage)
    {
       _playerSprite = playerSprite;
       Position = startPosition;
       
       Health = baseHealth;
       BaseDamage = baseDamage;
    }

    public void Update(GameTime gameTime)
    {
        _playerSprite.Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        _playerSprite.Draw(spriteBatch, Position);
    }
}