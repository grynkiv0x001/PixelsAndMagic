using System;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary;

public class Core : Game
{
    /// <summary>
    ///     Creates a new GameLibrary instance.
    /// </summary>
    /// <param name="title">The title to display in the title bar of the game window.</param>
    /// <param name="width">The initial width, in pixels, of the game window.</param>
    /// <param name="height">The initial height, in pixels, of the game window.</param>
    /// <param name="fullScreen">Indicates if the game should start in fullscreen mode.</param>
    protected Core(string title, int width, int height, bool fullScreen)
    {
        // Ensure that multiple cores are not created.
        if (Instance != null) throw new InvalidOperationException("Only a single GameLibrary instance can be created");

        // Store reference to engine for global member access.
        Instance = this;

        // Create a new graphics device manager.
        Graphics = new GraphicsDeviceManager(this);

        // Set the graphics defaults.
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = fullScreen;

        // Apply the graphic presentation changes.
        Graphics.ApplyChanges();

        // Set the window title.
        Window.Title = title;

        // Set the core's content manager to a reference of the base Game's
        // content manager.
        Content = base.Content;

        // Set the root directory for content.
        Content.RootDirectory = "Content";

        // Mouse is visible by default.
        IsMouseVisible = true;
    }

    /// <summary>
    ///     Gets a reference to the GameLibrary instance.
    /// </summary>
    public static Core Instance { get; private set; }

    /// <summary>
    ///     Gets the graphics device manager to control the presentation of graphics.
    /// </summary>
    public static GraphicsDeviceManager Graphics { get; private set; }

    /// <summary>
    ///     Gets the graphics device used to create graphical resources and perform primitive rendering.
    /// </summary>
    public new static GraphicsDevice GraphicsDevice { get; private set; }

    /// <summary>
    ///     Gets the sprite batch used for all 2D rendering.
    /// </summary>
    public static SpriteBatch SpriteBatch { get; private set; }

    /// <summary>
    ///     Gets the content manager used to load global assets.
    /// </summary>
    public new static ContentManager Content { get; private set; }

    public static InputManager InputManager { get; private set; }

    protected override void Initialize()
    {
        // Set the core's graphics device to a reference of the base Game's graphics device.
        GraphicsDevice = base.GraphicsDevice;

        // Create the sprite batch instance.
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        InputManager = new InputManager();

        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        InputManager.Update();

        base.Update(gameTime);
    }
}
