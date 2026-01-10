using System;
using Microsoft.Xna.Framework;

namespace GameLibrary.Utilities;

public readonly struct Circle : IEquatable<Circle>
{
    public Circle(int x, int y, int radius)
    {
        X = x;
        Y = y;
        Radius = radius;
    }

    public Circle(Point center, int radius)
    {
        X = center.X;
        Y = center.Y;
        Radius = radius;
    }

    public readonly int X;
    public readonly int Y;
    public readonly int Radius;

    public readonly Point Center => new(X, Y);

    public static Circle Empty { get; } = new();

    public readonly bool IsEmpty => X == 0 && Y == 0 && Radius == 0;

    public readonly int Top => Y - Radius;
    public readonly int Bottom => Y + Radius;
    public readonly int Left => X - Radius;
    public readonly int Right => X + Radius;

    /// <summary>
    ///     Method that determines if the two Circles intersect
    /// </summary>
    /// <param name="target">The Circle with which the intersection is being checked, i.e. the "other" Circle</param>
    /// <returns></returns>
    public bool Intersects(Circle target)
    {
        var radiiSquared = (Radius + target.Radius) * (Radius + target.Radius);
        var distanceSquared = Vector2.DistanceSquared(Center.ToVector2(), target.Center.ToVector2());

        return distanceSquared <= radiiSquared;
    }

    public readonly override bool Equals(object obj)
    {
        return obj is Circle target && Equals(target);
    }

    /// <summary>
    ///     Method that checks whether the two Circles are equal.
    /// </summary>
    /// <param name="target">The Circle to compare with the current Circle, i.e. the "other" Circle</param>
    /// <returns></returns>
    public readonly bool Equals(Circle target)
    {
        return X == target.X && Y == target.Y && Radius == target.Radius;
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Radius);
    }

    public static bool operator ==(Circle left, Circle right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Circle left, Circle right)
    {
        return !left.Equals(right);
    }
}
