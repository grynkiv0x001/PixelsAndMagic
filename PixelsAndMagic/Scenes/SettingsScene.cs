using GameLibrary;
using GameLibrary.Scenes;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using RenderingLibrary;

namespace PixelsAndMagic.Scenes;

public class SettingsScene : Scene
{
    private const string SETTINGS_TEXT = "Game Settings";

    private SpriteFont _font;

    private Vector2 _settingsTextOrigin;
    private Vector2 _settingsTextPosition;

    public override void Initialize()
    {
        base.Initialize();

        var size = _font.MeasureString(SETTINGS_TEXT);

        _settingsTextPosition = new Vector2(Core.Graphics.PreferredBackBufferWidth / 2, 200);
        _settingsTextOrigin = size * 0.5f;

        var stackPanel = new StackPanel();
        stackPanel.AddToRoot();
        stackPanel.Anchor(Anchor.Center);
        stackPanel.Spacing = 10;
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
        SystemManagers.Default.Draw();

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        SpriteBatch.DrawString(
            _font,
            SETTINGS_TEXT,
            _settingsTextPosition,
            Color.FloralWhite,
            0.0f,
            _settingsTextOrigin,
            4.0f,
            SpriteEffects.None,
            0.0f
        );

        SpriteBatch.End();
    }
}
