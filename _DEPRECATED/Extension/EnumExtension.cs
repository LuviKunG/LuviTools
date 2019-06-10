using System;

public static class EnumExtension
{
    public static bool HasFlag(this Enum mask, Enum flags)
    {
#if DEBUG
        if (mask.GetType() != flags.GetType())
            throw new ArgumentException(string.Format("The argument type, '{0}', is not the same as the enum type '{1}'.", flags.GetType(), mask.GetType()));
#endif
        return ((int)(IConvertible)mask & (int)(IConvertible)flags) == (int)(IConvertible)flags;
    }

    public static bool IsNoFlag(this Enum flags)
    {
        return (int)(IConvertible)flags == 0;
    }

    public static bool TryParse<TEnum>(this Enum item, string value, out TEnum target)
    {
        try
        {
            target = (TEnum)Enum.Parse(typeof(TEnum), value);
            return true;
        }
        catch
        {
            target = default(TEnum);
            return false;
        }
    }
}