using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameLibrary;
using GameLibrary.Graphics;

namespace PixelsAndMagic;

public class PixelsAndMagic : Core
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 720;
    private const bool FULLSCREEN = false;
    
    private AnimatedSprite _playerSprite;
    private Sprite _enemySprite;

    public PixelsAndMagic() : base("PixelsAndMagic", WINDOW_WIDTH, WINDOW_HEIGHT, FULLSCREEN)
    {
        
    }

    protected override void LoadContent()
    {
        // Enemy sprite loading
        SpriteSheet spriteSheet = SpriteSheet.FromFile(Content, "Images/spritesheet.xml");

        _enemySprite = spriteSheet.CreateSprite("Enemy");
        _enemySprite.Scale = new Vector2(4.0f, 4.0f);
        
        // Player (Wizard) sprite loading
        SpriteSheet playerSheet  = SpriteSheet.FromFile(Content, "Images/player-spritesheet.xml");
        
        _playerSprite = playerSheet.CreateAnimatedSprite("wizard-animation");
        _playerSprite.Scale = new Vector2(4.0f, 4.0f);
        
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        _playerSprite.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.FloralWhite);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        _playerSprite.Draw(SpriteBatch, new Vector2(10, 0));
        _enemySprite.Draw(SpriteBatch, new Vector2(200, 0));
        
        SpriteBatch.End();
        
        base.Draw(gameTime);
    }
}
