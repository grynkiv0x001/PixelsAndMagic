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

    private Texture2D _overlay;

    private Vector2 _settingsTextOrigin;
    private Vector2 _settingsTextPosition;

    private StackPanel _stackPanel;

    public override void Initialize()
    {
        base.Initialize();

        _overlay = new Texture2D(Core.GraphicsDevice, 1, 1);
        _overlay.SetData([
            Color.FloralWhite
        ]);

        var size = _font.MeasureString(SETTINGS_TEXT);

        _settingsTextPosition = new Vector2(Core.Graphics.PreferredBackBufferWidth / 2, 200);
        _settingsTextOrigin = size * 0.5f;

        _stackPanel = new StackPanel();
        _stackPanel.AddToRoot();
        _stackPanel.Anchor(Anchor.Center);
        _stackPanel.Spacing = 10;

        var isMusicMuted = Core.AudioController.IsMuted;

        var muteMusicButton = new Button();
        var backButton = new Button();

        _stackPanel.AddChild(muteMusicButton);
        _stackPanel.AddChild(backButton);

        muteMusicButton.Text = $"{(isMusicMuted ? "Unmute" : "Mute")} the Music";
        muteMusicButton.IsFocused = true;
        muteMusicButton.Click += (_, _) =>
            Core.AudioController.ToggleMute();

        backButton.Text = "Go back";
        backButton.Click += (_, _) =>
        {
            Core.PopScene();
            Core.PushScene(new PauseScene());
        };
    }

    public override void LoadContent()
    {
        _font = Content.Load<SpriteFont>("Fonts/Jacquard_12");
    }

    public override void UnloadContent()
    {
        _stackPanel?.RemoveFromRoot();
        _stackPanel = null;
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.InputManager.Keyboard.IsKeyPressed(Keys.Escape))
            Core.PopScene();
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();

        SpriteBatch.Draw(
            _overlay,
            new Rectangle(
                0,
                0,
                Core.Graphics.PreferredBackBufferWidth,
                Core.Graphics.PreferredBackBufferHeight
            ),
            Color.Black * 0.6f
        );

        SpriteBatch.End();

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
