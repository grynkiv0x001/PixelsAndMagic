using Microsoft.Xna.Framework;

namespace PixelsAndMagic;

public class PixelsAndMagic : GameLibrary.Core
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 720;
    private const bool FULLSCREEN = false;

    public PixelsAndMagic() : base("PixelsAndMagic", WINDOW_WIDTH, WINDOW_HEIGHT, FULLSCREEN)
    {
        
    }
    
    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}
