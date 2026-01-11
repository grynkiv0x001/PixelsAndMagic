using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Scenes;

public abstract class Scene : IDisposable
{
    public Scene()
    {
        Content = new ContentManager(Core.Content.ServiceProvider);
        Content.RootDirectory = Core.Content.RootDirectory;
    }

    protected ContentManager Content { get; }

    protected GraphicsDevice GraphicsDevice => Core.GraphicsDevice;
    protected SpriteBatch SpriteBatch => Core.SpriteBatch;

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Dispose(bool IsDisposing)
    {
        if (IsDisposed)
            return;

        if (IsDisposing)
        {
            UnloadContent();
            Content.Dispose();
        }

        IsDisposed = true;
    }

    ~Scene()
    {
        Dispose(false);
    }

    public virtual void Initialize()
    {
        LoadContent();
    }

    public virtual void LoadContent()
    {
    }

    public virtual void UnloadContent()
    {
        Content.Unload();
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(GameTime gameTime)
    {
    }
}
