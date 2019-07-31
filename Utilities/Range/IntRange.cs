using System;

[Serializable]
public struct IntRange
{
    public int min;
    public int max;

    public IntRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public bool IsInRange(int val)
    {
        return val > min && val < max;
    }

    public bool IsInRangeEqual(int val)
    {
        return val >= min && val <= max;
    }

    public int SetInRange(int val)
    {
        if (val < min) return min;
        else if (val > max) return max;
        else return val;
    }
}
