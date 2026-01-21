using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelsAndMagic.Entities;

namespace PixelsAndMagic.Systems;

public class ProjectileSystem(Rectangle worldBounds)
{
    private readonly List<Projectile> _projectiles = [];
    private Rectangle _worldBounds = worldBounds;

    public IEnumerable<Projectile> Projectiles => _projectiles;

    public void SpawnProjectile(Projectile projectile)
    {
        _projectiles.Add(projectile);
    }

    public void Update(GameTime gameTime)
    {
        foreach (var projectile in _projectiles)
        {
            projectile.Update(gameTime);

            if (!_worldBounds.Contains(projectile.Position.ToPoint()))
                projectile.IsAlive = false;
        }

        _projectiles.RemoveAll(projectile => !projectile.IsAlive);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var projectile in _projectiles) projectile.Draw(spriteBatch);
    }
}
