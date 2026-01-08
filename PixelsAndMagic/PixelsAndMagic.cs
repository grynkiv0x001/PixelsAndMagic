using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameLibrary;

namespace PixelsAndMagic;

public class PixelsAndMagic : Core
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 720;
    private const bool FULLSCREEN = false;
    
    private Sprite _playerSprite;
    private Sprite _enemySprite;

    public PixelsAndMagic() : base("PixelsAndMagic", WINDOW_WIDTH, WINDOW_HEIGHT, FULLSCREEN)
    {
        
    }

    protected override void LoadContent()
    {
        SpriteSheet spriteSheet = SpriteSheet.FromFile(Content, "Images/spritesheet.xml");

        spriteSheet.AddRegion("Wizard", 32, 0, 32, 32);
        spriteSheet.AddRegion("Rock", 0, 0, 32, 32);

        _playerSprite = spriteSheet.CreateSprite("Wizard");
        _enemySprite = spriteSheet.CreateSprite("Rock");
        
        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();
        
        _playerSprite.Draw(SpriteBatch, Vector2.Zero);
        _enemySprite.Draw(SpriteBatch, new Vector2(100, 0));
        
        SpriteBatch.End();
        
        base.Draw(gameTime);
    }
}
