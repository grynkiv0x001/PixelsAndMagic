using GameLibrary;
using GameLibrary.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelsAndMagic.Scenes;

public class PauseScene : Scene
{
    private const string PAUSE_TEXT = "Paused";

    private SpriteFont _font;

    private Vector2 _pausedTextOrigin;
    private Vector2 _pausedTextPosition;

    public override void Initialize()
    {
        base.Initialize();

        var size = _font.MeasureString(PAUSE_TEXT);

        _pausedTextPosition = new Vector2(Core.Graphics.PreferredBackBufferWidth / 2, 200);
        _pausedTextOrigin = size * 0.5f;
    }

    public override void LoadContent()
    {
        _font = Content.Load<SpriteFont>("Fonts/Jacquard_12");
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.InputManager.Keyboard.IsKeyPressed(Keys.Escape))
            Core.PopScene();
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        SpriteBatch.DrawString(
            _font,
            PAUSE_TEXT,
            _pausedTextPosition,
            Color.Black,
            0.0f,
            _pausedTextOrigin,
            4.0f,
            SpriteEffects.None,
            0.0f
        );

        SpriteBatch.End();
    }
}
