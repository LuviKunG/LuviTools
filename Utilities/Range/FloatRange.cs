using System;

[Serializable]
public struct FloatRange
{
    public float min;
    public float max;

    public FloatRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public bool IsInRange(float val)
    {
        return val > min && val < max;
    }

    public bool IsInRangeEqual(float val)
    {
        return val >= min && val <= max;
    }

    public float SetInRange(float val)
    {
        if (val < min) return min;
        else if (val > max) return max;
        else return val;
    }
}
