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

public class PauseScene : Scene
{
    private const string PAUSE_TEXT = "Paused";

    private SpriteFont _font;

    private Texture2D _overlay;

    private Vector2 _pausedTextOrigin;
    private Vector2 _pausedTextPosition;

    private StackPanel _stackPanel;

    public override void Initialize()
    {
        base.Initialize();

        _overlay = new Texture2D(Core.GraphicsDevice, 1, 1);
        _overlay.SetData([
            Color.Black
        ]);

        var size = _font.MeasureString(PAUSE_TEXT);

        _pausedTextPosition = new Vector2(Core.Graphics.PreferredBackBufferWidth / 2, 200);
        _pausedTextOrigin = size * 0.5f;

        _stackPanel = new StackPanel();
        _stackPanel.AddToRoot();
        _stackPanel.Anchor(Anchor.Center);
        _stackPanel.Spacing = 10;
        _stackPanel.MaxWidth = 200;

        var resumeButton = new Button();
        var settingsButton = new Button();
        var exitButton = new Button();

        _stackPanel.AddChild(resumeButton);
        _stackPanel.AddChild(settingsButton);
        _stackPanel.AddChild(exitButton);

        resumeButton.Text = "Resume the game";
        resumeButton.IsFocused = true;
        resumeButton.Width = 180;
        resumeButton.Click += (_, _) =>
            Core.PopScene();

        settingsButton.Text = "Game settings";
        settingsButton.Width = 180;
        settingsButton.Click += (_, _) =>
        {
            Core.PopScene();
            Core.PushScene(new SettingsScene());
        };

        exitButton.Text = "Exit the game";
        exitButton.Width = 180;
        exitButton.Click += (_, _) =>
            Core.ExitGame();
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
            PAUSE_TEXT,
            _pausedTextPosition,
            Color.FloralWhite,
            0.0f,
            _pausedTextOrigin,
            4.0f,
            SpriteEffects.None,
            0.0f
        );

        SpriteBatch.End();
    }
}
