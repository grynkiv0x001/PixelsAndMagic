using System.Collections.Generic;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelsAndMagic.Entities;

namespace PixelsAndMagic.GameWorld;

public class Area
{
    private readonly List<Enemy> _enemies = [];
    private readonly List<Trigger> _triggers = [];

    public Area(Tilemap tilemap)
    {
        Tilemap = tilemap;

        GameplayBounds = new Rectangle(
            (int)tilemap.TileWidth,
            (int)tilemap.TileHeight,
            (int)(tilemap.Columns * tilemap.TileWidth) - (int)tilemap.TileWidth * 2,
            (int)(tilemap.Rows * tilemap.TileHeight) - (int)tilemap.TileHeight * 2
        );
    }

    public Tilemap Tilemap { get; }
    public Rectangle GameplayBounds { get; }

    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void AddTrigger(Trigger trigger)
    {
        _triggers.Add(trigger);
    }

    public void Update(GameTime gameTime, Player player)
    {
        foreach (var enemy in _enemies)
            enemy.Update(gameTime, GameplayBounds);

        foreach (var trigger in _triggers)
            trigger.Update(player);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Tilemap.Draw(spriteBatch);

        foreach (var enemy in _enemies)
            enemy.Draw(spriteBatch);
    }
}
