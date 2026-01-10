using System;
using Microsoft.Xna.Framework;

namespace GameLibrary.Utilities;

public readonly struct Circle : IEquatable<Circle>
{
    public Circle(float x, float y, float radius)
    {
        X = x;
        Y = y;
        Radius = radius;
    }

    public Circle(Point center, float radius)
    {
        X = center.X;
        Y = center.Y;
        Radius = radius;
    }

    public readonly float X;
    public readonly float Y;
    public readonly float Radius;

    public readonly Vector2 Center => new(X, Y);

    public static Circle Empty { get; } = new();

    public readonly bool IsEmpty => X == 0 && Y == 0 && Radius == 0;

    public readonly float Top => Y - Radius;
    public readonly float Bottom => Y + Radius;
    public readonly float Left => X - Radius;
    public readonly float Right => X + Radius;

    /// <summary>
    ///     Method that determines if the two Circles intersect
    /// </summary>
    /// <param name="target">The Circle with which the intersection is being checked, i.e. the "other" Circle</param>
    /// <returns></returns>
    public bool Intersects(Circle target)
    {
        var radiiSquared = (Radius + target.Radius) * (Radius + target.Radius);
        var distanceSquared = Vector2.DistanceSquared(Center, target.Center);

        return distanceSquared <= radiiSquared;
    }

    // TODO: Rewrite without the IEquatable<T>

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
        return Center == target.Center && Radius == target.Radius;
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
