using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelsAndMagic;

public class PixelsAndMagic : GameLibrary.Core
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 720;
    private const bool FULLSCREEN = false;
    
    private Texture2D _playerTexture;

    public PixelsAndMagic() : base("PixelsAndMagic", WINDOW_WIDTH, WINDOW_HEIGHT, FULLSCREEN)
    {
        
    }

    protected override void LoadContent()
    {
        _playerTexture = Content.Load<Texture2D>("Images/wizard");
        
        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();
        
        SpriteBatch.Draw(_playerTexture, Vector2.Zero, Color.White);
        
        SpriteBatch.End();
        
        base.Draw(gameTime);
    }
}
