using GameLibrary;
using Microsoft.Xna.Framework.Media;
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

        PushScene(new TitleScene(WINDOW_WIDTH, WINDOW_HEIGHT));
    }

    protected override void LoadContent()
    {
        // Loading audio
        _mainAmbientTrack = Content.Load<Song>("Audio/main-ambient");
    }
}
