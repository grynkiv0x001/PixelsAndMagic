using GameLibrary;
using Gum.Forms;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using PixelsAndMagic.Scenes;

namespace PixelsAndMagic;

public class PixelsAndMagic : Core
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 768;
    private const bool FULLSCREEN = false;

    private Song _mainAmbientTrack;

    public PixelsAndMagic() : base("Pixels & Magic", WINDOW_WIDTH, WINDOW_HEIGHT, FULLSCREEN)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();

        // TODO: Move it to the game settings (controlling the volume)
        AudioController.PlaySoundTrack(_mainAmbientTrack);

        InitializeGum();

        PushScene(new TitleScene(WINDOW_WIDTH, WINDOW_HEIGHT));
        // PushScene(new GameScene());
    }

    protected override void LoadContent()
    {
        // Loading audio
        _mainAmbientTrack = Content.Load<Song>("Audio/main-ambient");
    }

    protected override void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);

        base.Update(gameTime);
    }

    private void InitializeGum()
    {
        GumService.Default.Initialize(this, DefaultVisualsVersion.V3);

        GumService.Default.ContentLoader.XnaContentManager = Content;

        FrameworkElement.KeyboardsForUiControl.Add(GumService.Default.Keyboard);

        FrameworkElement.GamePadsForUiControl.AddRange(GumService.Default.Gamepads);

        FrameworkElement.TabReverseKeyCombos.Add(
            new KeyCombo { PushedKey = Keys.Up });

        FrameworkElement.TabKeyCombos.Add(
            new KeyCombo { PushedKey = Keys.Down });

        FrameworkElement.ClickCombos.Add(
            new KeyCombo { PushedKey = Keys.Enter });

        GumService.Default.CanvasWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
        GumService.Default.CanvasHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

        GumService.Default.Renderer.Camera.Zoom = 1.0f;
    }
}
