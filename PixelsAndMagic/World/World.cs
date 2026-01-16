using System.Collections.Generic;
using System.Linq;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelsAndMagic.Entities;

namespace PixelsAndMagic.World;

public class World
{
    // TODO: Promote to entities & add an Entity base class
    private readonly List<Enemy> _enemies = [];

    public World(Tilemap tilemap, Player player, IEnumerable<Enemy> enemies)
    {
        Tilemap = tilemap;
        Player = player;

        _enemies.AddRange(enemies);

        Bounds = new Rectangle(
            (int)tilemap.TileWidth,
            (int)tilemap.TileHeight,
            (int)(tilemap.Columns * tilemap.TileWidth) - (int)tilemap.TileWidth * 2,
            (int)(tilemap.Rows * tilemap.TileHeight) - (int)tilemap.TileHeight * 2
        );
    }

    public Player Player { get; }
    public Tilemap Tilemap { get; }

    public Rectangle Bounds { get; }

    public void Update(GameTime gameTime)
    {
        Player.Update(gameTime, Bounds);

        foreach (var enemy in _enemies)
            enemy.Update(gameTime, Bounds);

        HandleCollisions();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Tilemap.Draw(spriteBatch);
        Player.Draw(spriteBatch);

        foreach (var enemy in _enemies)
            enemy.Draw(spriteBatch);
    }

    public void HandleCollisions()
    {
        foreach (var enemy in _enemies.Where(enemy => Player.Collider.Intersects(enemy.Collider)))
            Player.HandleEntityCollision(enemy.Collider);
    }
}
