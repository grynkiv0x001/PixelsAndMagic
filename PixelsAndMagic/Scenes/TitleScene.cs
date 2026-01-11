using GameLibrary;
using GameLibrary.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelsAndMagic.Scenes;

public class TitleScene(int windowWidth, int windowHeight) : Scene
{
    private const string TITLE_FIRST = "Pixels";
    private const string TITLE_SECOND = "& Magic";
    private const string HELPER_TEXT = "Press Enter to continue";

    private SpriteFont _font;

    private Vector2 _helperTextOrigin;
    private Vector2 _helperTextPosition;

    private Vector2 _titleFirstPartOrigin;
    private Vector2 _titleFirstPartPosition;

    private Vector2 _titleSecondPartOrigin;
    private Vector2 _titleSecondPartPosition;

    public override void Initialize()
    {
        base.Initialize();

        var size = _font.MeasureString(TITLE_FIRST);

        _titleFirstPartPosition = new Vector2(windowWidth / 2, 160);
        _titleFirstPartOrigin = size * 0.5f;

        size = _font.MeasureString(TITLE_SECOND);

        _titleSecondPartPosition = new Vector2(windowWidth / 2, 280);
        _titleSecondPartOrigin = size * 0.5f;

        size = _font.MeasureString(HELPER_TEXT);

        _helperTextPosition = new Vector2(windowWidth / 2, 600);
        _helperTextOrigin = size * 0.5f;
    }

    public override void LoadContent()
    {
        _font = Content.Load<SpriteFont>("Fonts/Jacquard_12");
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.InputManager.Keyboard.IsKeyPressed(Keys.Enter))
            Core.SetScene(new GameScene());
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        var dropShadowColor = Color.Black * 0.5f;

        SpriteBatch.DrawString(
            _font,
            TITLE_FIRST,
            _titleFirstPartPosition,
            Color.Black,
            0.0f,
            _titleFirstPartOrigin,
            5.0f,
            SpriteEffects.None,
            0.0f
        );

        SpriteBatch.DrawString(
            _font,
            TITLE_SECOND,
            _titleSecondPartPosition,
            Color.Black,
            0.0f,
            _titleSecondPartOrigin,
            5.0f,
            SpriteEffects.None,
            0.0f
        );

        SpriteBatch.DrawString(
            _font,
            HELPER_TEXT,
            _helperTextPosition,
            Color.Black,
            0.0f,
            _helperTextOrigin,
            2.0f,
            SpriteEffects.None,
            0.0f
        );

        SpriteBatch.End();
    }
}
