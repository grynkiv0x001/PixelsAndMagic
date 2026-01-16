using System;
using GameLibrary.Utilities;
using Microsoft.Xna.Framework;
using PixelsAndMagic.Entities;

namespace PixelsAndMagic.GameWorld;

public class Trigger
{
    public Rectangle Bounds;
    public Action OnEnter;

    public void Update(Player player)
    {
        if (CollisionHelper.Intersects(player.Collider, Bounds))
            OnEnter?.Invoke();
    }
}
