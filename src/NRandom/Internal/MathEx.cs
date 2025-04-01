namespace NRandom;

internal static class MathEx
{
#if !NET6_0_OR_GREATER
    public static double BitDecrement(double x)
    {
        long bits = BitConverter.DoubleToInt64Bits(x);

        if (((bits >> 32) & 0x7FF00000) >= 0x7FF00000)
        {
            // NaN returns NaN
            // -Infinity returns -Infinity
            // +Infinity returns double.MaxValue
            return (bits == 0x7FF00000_00000000) ? double.MaxValue : x;
        }

        if (bits == 0x00000000_00000000)
        {
            // +0.0 returns -double.Epsilon
            return -double.Epsilon;
        }

        // Negative values need to be incremented
        // Positive values need to be decremented

        bits += ((bits < 0) ? +1 : -1);
        return BitConverter.Int64BitsToDouble(bits);
    }

    public static float BitDecrement(float x)
    {
        int bits = BitConverter.SingleToInt32Bits(x);

        if ((bits & 0x7F800000) >= 0x7F800000)
        {
            // NaN returns NaN
            // -Infinity returns -Infinity
            // +Infinity returns float.MaxValue
            return (bits == 0x7F800000) ? float.MaxValue : x;
        }

        if (bits == 0x00000000)
        {
            // +0.0 returns -float.Epsilon
            return -float.Epsilon;
        }

        // Negative values need to be incremented
        // Positive values need to be decremented

        bits += ((bits < 0) ? +1 : -1);
        return BitConverter.Int32BitsToSingle(bits);
    }

#endif
}