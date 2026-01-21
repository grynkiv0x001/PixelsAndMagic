using GameLibrary;
using GameLibrary.Graphics;
using GameLibrary.Scenes;
using GameLibrary.Utilities;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using PixelsAndMagic.Entities;
using PixelsAndMagic.GameWorld;
using RenderingLibrary;

namespace PixelsAndMagic.Scenes;

public class GameScene : Scene
{
    private Camera2D _camera;

    private Panel _debugPanel;
    private Label _fpsCounter;

    private World _world;

    public override void Initialize()
    {
        base.Initialize();

        _debugPanel = new Panel();
        _debugPanel.AddToRoot();
        _debugPanel.Anchor(Anchor.TopLeft);

        _fpsCounter = new Label
        {
            Text = "FPS: 0",
            X = 10,
            Y = 10,
            IsVisible = false
        };

        _debugPanel.AddChild(_fpsCounter);

        // BoundaryPadding is the wall thickness 
        _camera = new Camera2D
        {
            WorldBounds = _world.Bounds,
            BoundaryPadding = 32 * 4
        };
    }

    public override void LoadContent()
    {
        // Enemy sprite loading
        var enemySheet = SpriteSheet.FromFile(Content, "Images/enemy-spritesheet.xml");

        var enemySprite = enemySheet.CreateAnimatedSprite("enemy-animation");
        enemySprite.Scale = new Vector2(4.0f, 4.0f);

        var enemy = new Enemy(enemySprite, new Vector2(600, 380));
        var standingEnemy = new Enemy(enemySprite, new Vector2(300, 300), false);

        // Player (Wizard) sprite loading
        var playerSheet = SpriteSheet.FromFile(Content, "Images/player-spritesheet.xml");

        var playerSprite = playerSheet.CreateAnimatedSprite("wizard-animation");
        playerSprite.Scale = new Vector2(4.0f, 4.0f);

        var player = new Player(playerSprite, Core.InputManager, new Vector2(200, 200), 100.0f, 20.0f);

        var fireballSheet = SpriteSheet.FromFile(Content, "Images/projectiles.xml");

        // GameWorld loading (using the Tilemap)
        var world = Tilemap.FromFile(Content, "Images/world-tilemap.xml");
        world.Scale = new Vector2(4.0f, 4.0f);

        _world = new World(world, player, [enemy, standingEnemy]);

        _world.Player.FireRequested += (position, direction) =>
        {
            var projectileSprite = fireballSheet.CreateSprite("Fireball");
            projectileSprite.Scale = new Vector2(4.0f, 4.0f);

            var fireball = new Projectile(
                projectileSprite,
                position + new Vector2(32, 16),
                direction * 10.0f,
                _world.Player.BaseDamage,
                direction.X < 0
            );

            _world.ProjectileSystem.SpawnProjectile(fireball);
        };
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.InputManager.Keyboard.IsKeyPressed(Keys.Escape))
            Core.PushScene(new PauseScene());

        if (Core.InputManager.Keyboard.IsKeyPressed(Keys.F1)) _fpsCounter.IsVisible = !_fpsCounter.IsVisible;

        _fpsCounter.Text = ((int)Core.FrameCounter.CurrentFramesPerSecond).ToString();

        _world.Update(gameTime);

        // Camera follow
        _camera.Follow(
            _world.Player.Position,
            Core.Graphics.GraphicsDevice.Viewport
        );

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            transformMatrix: _camera.GetViewMatrix()
        );

        _world.Draw(SpriteBatch);

        SpriteBatch.End();

        SystemManagers.Default.Draw();

        base.Draw(gameTime);
    }
}
