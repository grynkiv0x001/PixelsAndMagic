using GameLibrary;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using PixelsAndMagic.Entities;

namespace PixelsAndMagic;

public class PixelsAndMagic : Core
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 768;
    private const bool FULLSCREEN = false;

    private Enemy _enemy;

    private SpriteFont _fontPixel;
    private SpriteFont _fontRegular;

    private Vector2 _gameInfoTextOrigin;
    private Vector2 _gameInfoTextPosition;

    private Song _mainAmbientTrack;

    private Player _player;

    private Tilemap _world;

    public PixelsAndMagic() : base("PixelsAndMagic", WINDOW_WIDTH, WINDOW_HEIGHT, FULLSCREEN)
    {
    }

    protected override void LoadContent()
    {
        // Enemy sprite loading
        var enemySheet = SpriteSheet.FromFile(Content, "Images/enemy-spritesheet.xml");

        var enemySprite = enemySheet.CreateAnimatedSprite("enemy-animation");
        enemySprite.Scale = new Vector2(4.0f, 4.0f);

        _enemy = new Enemy(enemySprite, new Vector2(600, 380), false);

        // Player (Wizard) sprite loading
        var playerSheet = SpriteSheet.FromFile(Content, "Images/player-spritesheet.xml");

        var playerSprite = playerSheet.CreateAnimatedSprite("wizard-animation");
        playerSprite.Scale = new Vector2(4.0f, 4.0f);

        _player = new Player(playerSprite, InputManager, new Vector2(200, 200), 100.0f, 20.0f);

        // World loading (using the Tilemap)
        _world = Tilemap.FromFile(Content, "Images/world-tilemap.xml");
        _world.Scale = new Vector2(4.0f, 4.0f);

        // Loading audio
        _mainAmbientTrack = Content.Load<Song>("Audio/main-ambient");

        // Loading font
        _fontPixel = Content.Load<SpriteFont>("Fonts/Jacquard_12");
        _fontRegular = Content.Load<SpriteFont>("Fonts/Caudex");

        base.LoadContent();
    }

    protected override void Initialize()
    {
        base.Initialize();

        AudioController.PlaySoundTrack(_mainAmbientTrack);

        var infoTextYOrigin = _fontPixel.MeasureString("Health").Y * 0.5f;

        _gameInfoTextPosition = new Vector2(32, 32);
        _gameInfoTextOrigin = new Vector2(0, infoTextYOrigin);
    }

    protected override void Update(GameTime gameTime)
    {
        // Screen boundaries
        // TODO: Create a proper world instance
        var tileWidth = (int)_world.TileWidth;
        var tileHeight = (int)_world.TileHeight;

        var screenBounds = new Rectangle(
            tileWidth,
            tileHeight,
            (int)(_world.Columns * _world.TileWidth) - tileWidth * 2,
            (int)(_world.Rows * _world.TileHeight) - tileHeight * 2
        );

        _player.Update(gameTime, screenBounds);
        _enemy.Update(gameTime, screenBounds);

        if (_player.Collider.Intersects(_enemy.Collider)) _player.HandleEntityCollision(_enemy.Collider);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.FloralWhite);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _world.Draw(SpriteBatch);

        _player.Draw(SpriteBatch);
        _enemy.Draw(SpriteBatch);

        SpriteBatch.DrawString(
            _fontPixel,
            $"Health: {_player.Health}",
            _gameInfoTextPosition,
            Color.FloralWhite,
            0.0f,
            _gameInfoTextOrigin,
            2.0f,
            SpriteEffects.None,
            0.0f
        );

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
