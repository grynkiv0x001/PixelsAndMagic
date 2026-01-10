using GameLibrary;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelsAndMagic.Entities;

namespace PixelsAndMagic;

public class PixelsAndMagic : Core
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 720;
    private const bool FULLSCREEN = false;

    private Enemy _enemy;
    private Player _player;

    public PixelsAndMagic() : base("PixelsAndMagic", WINDOW_WIDTH, WINDOW_HEIGHT, FULLSCREEN)
    {
    }

    protected override void LoadContent()
    {
        // Enemy sprite loading
        var enemySheet = SpriteSheet.FromFile(Content, "Images/enemy-spritesheet.xml");

        var enemySprite = enemySheet.CreateAnimatedSprite("enemy-animation");
        enemySprite.Scale = new Vector2(4.0f, 4.0f);

        _enemy = new Enemy(enemySprite, new Vector2(200, 0));

        // Player (Wizard) sprite loading
        var playerSheet = SpriteSheet.FromFile(Content, "Images/player-spritesheet.xml");

        var playerSprite = playerSheet.CreateAnimatedSprite("wizard-animation");
        playerSprite.Scale = new Vector2(4.0f, 4.0f);

        _player = new Player(playerSprite, InputManager, new Vector2(10, 0));

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // Screen boundaries
        // TODO: Create a proper world instance
        var screenBounds = new Rectangle(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        _player.Update(gameTime, screenBounds);
        _enemy.Update(gameTime, screenBounds);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.FloralWhite);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _player.Draw(SpriteBatch);
        _enemy.Draw(SpriteBatch);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
