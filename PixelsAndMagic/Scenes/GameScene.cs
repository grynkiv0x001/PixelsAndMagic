using System;
using System.Collections.Generic;
using System.Linq;
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
    private SpriteFont _font;
    private Label _fpsCounter;
    private SpriteSheet _projectileSheet;
    private Texture2D _spellPanel;

    private Dictionary<SpellType, TextureRegion> _spellTextureRegions = [];

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

        _spellPanel = new Texture2D(Core.GraphicsDevice, 1, 1);
        _spellPanel.SetData([
            Color.Black
        ]);

        // BoundaryPadding is the wall thickness 
        _camera = new Camera2D
        {
            WorldBounds = _world.Bounds,
            BoundaryPadding = 32 * 4
        };
    }

    public override void LoadContent()
    {
        _font = Content.Load<SpriteFont>("Fonts/Jacquard_12");

        // Enemy sprite loading
        var enemySheet = SpriteSheet.FromFile(Content, "Images/enemy-spritesheet.xml");

        var enemySprite = enemySheet.CreateAnimatedSprite("enemy-animation");
        enemySprite.Scale = new Vector2(4.0f, 4.0f);

        var ratSprite = enemySheet.CreateAnimatedSprite("rat-animation");
        ratSprite.Scale = new Vector2(4.0f, 4.0f);

        var enemy = new Enemy(ratSprite, new Vector2(600, 380), 80f, 20f);
        var standingEnemy = new Enemy(enemySprite, new Vector2(300, 300), 100f, 15f, false);
        var rat1 = new Enemy(ratSprite, new Vector2(300, 400), 60f, 10f, false);

        // Player (Wizard) sprite loading
        var playerSheet = SpriteSheet.FromFile(Content, "Images/player-spritesheet.xml");

        var playerSprite = playerSheet.CreateAnimatedSprite("wizard-animation");
        playerSprite.Scale = new Vector2(4.0f, 4.0f);

        var player = new Player(playerSprite, Core.InputManager, new Vector2(200, 200), 100.0f, 20.0f);

        _projectileSheet = SpriteSheet.FromFile(Content, "Images/projectiles.xml");

        _spellTextureRegions = Enum.GetValues<SpellType>().ToDictionary(
            spell => spell,
            spell => _projectileSheet.CreateSprite(spell.ToString()).Region
        );

        // GameWorld loading (using the Tilemap)
        var world = Tilemap.FromFile(Content, "Images/world-tilemap.xml");
        world.Scale = new Vector2(4.0f, 4.0f);

        _world = new World(world, player, [enemy, standingEnemy, rat1]);

        _world.Player.FireRequested += (position, direction, activeSpell) =>
        {
            var projectileSprite = _projectileSheet.CreateSprite(activeSpell.ToString());
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

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // TODO: Move spell panel to a separate UI component
        var region = _spellTextureRegions[_world.Player.ActiveSpell];

        const float scale = 4f;
        const int padding = 16;

        var spriteWidth = region.SourceRectangle.Width * scale;
        var spriteHeight = region.SourceRectangle.Height * scale;

        var viewport = Core.Graphics.GraphicsDevice.Viewport;

        var position = new Vector2(
            spriteWidth - padding + 15,
            viewport.Height - (spriteHeight + 15) - padding
        );

        SpriteBatch.Draw(
            _spellPanel,
            new Rectangle(
                padding,
                viewport.Height - 60 - padding,
                180,
                60
            ),
            Color.Black * 0.6f
        );

        SpriteBatch.DrawString(
            _font,
            _world.Player.ActiveSpell.ToString(),
            new Vector2(position.X + padding + 35, position.Y + 7),
            Color.FloralWhite
        );

        region.Draw(
            SpriteBatch,
            position,
            Color.White,
            0f,
            Vector2.Zero,
            scale,
            SpriteEffects.None,
            0f
        );

        SpriteBatch.End();

        SystemManagers.Default.Draw();

        base.Draw(gameTime);
    }
}
