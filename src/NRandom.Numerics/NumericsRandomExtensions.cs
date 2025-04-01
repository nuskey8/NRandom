using System.Numerics;
using System.Runtime.CompilerServices;

namespace NRandom.Numerics;

public static class NumericsRandomExtensions
{
    /// <summary>
    /// Returns a random Vector2 value with all components in [0, 1).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 NextVector2(this IRandom random)
    {
        return new Vector2(random.NextFloat(), random.NextFloat());
    }

    /// <summary>
    /// Returns a random Vector2 value with all components in [0, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 NextVector2(this IRandom random, Vector2 max)
    {
        return new Vector2(random.NextFloat(max.X), random.NextFloat(max.Y));
    }

    /// <summary>
    /// Returns a random Vector2 value with all components in [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 NextVector2(this IRandom random, Vector2 min, Vector2 max)
    {
        return new Vector2(random.NextFloat(min.X, max.X), random.NextFloat(min.Y, max.Y));
    }

    /// <summary>
    /// Returns a unit length Vector2 vector representing a uniformly random 2D direction.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 NextVector2Direction(this IRandom random)
    {
        float angle = random.NextFloat() * MathF.PI * 2.0f;
#if NET6_0_OR_GREATER
        (float s, float c) = MathF.SinCos(angle);
#else
        float s = MathF.Sin(angle);
        float c = MathF.Cos(angle);
#endif
        return new Vector2(c, s);
    }

    /// <summary>
    /// Returns a random point inside a unit circle.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 NextVector2InsideCircle(this IRandom random)
    {
        return NextVector2Direction(random) * random.NextFloat();
    }

    /// <summary>
    /// Returns a random Vector3 value with all components in [0, 1).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 NextVector3(this IRandom random)
    {
        return new Vector3(random.NextFloat(), random.NextFloat(), random.NextFloat());
    }

    /// <summary>
    /// Returns a random Vector3 value with all components in [0, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 NextVector3(this IRandom random, Vector3 max)
    {
        return new Vector3(random.NextFloat(max.X), random.NextFloat(max.Y), random.NextFloat(max.Z));
    }

    /// <summary>
    /// Returns a random Vector3 value with all components in [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 NextVector3(this IRandom random, Vector3 min, Vector3 max)
    {
        return new Vector3(random.NextFloat(min.X, max.X), random.NextFloat(min.Y, max.Y), random.NextFloat(min.Z, max.Z));
    }

    /// <summary>
    /// Returns a unit length Vector2 vector representing a uniformly random 2D direction.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 NextVector3Direction(this IRandom random)
    {
        var rnd = NextVector2(random);
        var z = rnd.X * 2.0f - 1.0f;
        var r = MathF.Sqrt(MathF.Max(1.0f - z * z, 0.0f));
        var angle = rnd.Y * MathF.PI * 2.0f;

#if NET6_0_OR_GREATER
        (float s, float c) = MathF.SinCos(angle);
#else
        float s = MathF.Sin(angle);
        float c = MathF.Cos(angle);
#endif

        return new Vector3(c * r, s * r, z);
    }

    /// <summary>
    /// Returns a random point inside a unit sphere.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 NextVector3InsideSphere(this IRandom random)
    {
        return NextVector3Direction(random) * random.NextFloat();
    }

    /// <summary>
    /// Returns a random Vector4 value with all components in [0, 1).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 NextVector4(this IRandom random)
    {
        return new Vector4(random.NextFloat(), random.NextFloat(), random.NextFloat(), random.NextFloat());
    }

    /// <summary>
    /// Returns a random Vector4 value with all components in [0, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 NextVector4(this IRandom random, Vector4 max)
    {
        return new Vector4(random.NextFloat(max.X), random.NextFloat(max.Y), random.NextFloat(max.Z), random.NextFloat(max.W));
    }

    /// <summary>
    /// Returns a random Vector4 value with all components in [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 NextVector4(this IRandom random, Vector4 min, Vector4 max)
    {
        return new Vector4(random.NextFloat(min.X, max.X), random.NextFloat(min.Y, max.Y), random.NextFloat(min.Z, max.Z), random.NextFloat(min.W, max.W));
    }

    /// <summary>
    /// Returns a unit length quaternion representing a uniformly 3D rotation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion NextQuaternionRotation(this IRandom random)
    {
        var rnd = random.NextVector3(new Vector3(2.0f * MathF.PI, 2.0f * MathF.PI, 1.0f));
        var u1 = rnd.Z;
        var theta_rho = new Vector2(rnd.X, rnd.Y);

        var i = MathF.Sqrt(1.0f - u1);
        var j = MathF.Sqrt(u1);

        var sin_theta_rho = new Vector2(MathF.Sin(theta_rho.X), MathF.Sin(theta_rho.Y));
        var cos_theta_rho = new Vector2(MathF.Cos(theta_rho.X), MathF.Cos(theta_rho.Y));

        var q = new Quaternion(i * sin_theta_rho.X, i * cos_theta_rho.X, j * sin_theta_rho.Y, j * cos_theta_rho.Y);
        return q.W < 0.0f ? q : -q;
    }
}
