using System.Collections.Generic;
using System.Linq;
using GameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelsAndMagic.Entities;
using PixelsAndMagic.Systems;

namespace PixelsAndMagic.GameWorld;

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

        ProjectileSystem = new ProjectileSystem(Bounds);
    }

    public Player Player { get; }
    public Tilemap Tilemap { get; }

    public Rectangle Bounds { get; }

    public ProjectileSystem ProjectileSystem { get; }

    public void Update(GameTime gameTime)
    {
        Player.Update(gameTime, Bounds);

        foreach (var enemy in _enemies)
            enemy.Update(gameTime, Bounds);

        ProjectileSystem.Update(gameTime);

        HandleCollisions();
        HandleProjectileEnemyCollision();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Tilemap.Draw(spriteBatch);
        Player.Draw(spriteBatch);

        foreach (var enemy in _enemies)
            enemy.Draw(spriteBatch);

        ProjectileSystem.Draw(spriteBatch);
    }

    public void HandleCollisions()
    {
        foreach (var enemy in _enemies.Where(enemy => Player.Collider.Intersects(enemy.Collider)))
            Player.HandleEntityCollision(enemy.Collider);
    }

    public void HandleProjectileEnemyCollision()
    {
        foreach (var projectile in ProjectileSystem.Projectiles)
        {
            if (!projectile.IsAlive)
                continue;

            var projectileCollider = projectile.CircleCollider;

            foreach (var enemy in _enemies)
            {
                if (!projectileCollider.Intersects(enemy.Collider))
                    continue;

                enemy.TakeDamage(projectile.Damage);
                projectile.IsAlive = false;

                break;
            }
        }
    }
}
