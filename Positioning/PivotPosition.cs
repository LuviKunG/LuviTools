using System;
using UnityEngine;

[Serializable]
public struct PivotPosition
{
    public enum Pivot { Order, Middle }
    public Pivot pivot;
    public Vector3 size;

    public PivotPosition(Pivot pivot) : this()
    {
        this.pivot = pivot;
    }

    public PivotPosition(Pivot pivot, Vector3 size) : this(pivot)
    {
        this.size = size;
    }

    public Vector3 GetPosition(int length, int index)
    {
#if DEBUG
        if (length <= 0)
            throw new InvalidOperationException("\'length\' cannot be zero or below.");
        if (index < 0)
            throw new InvalidOperationException("\'index\' cannot below from zero.");
#endif
        switch (pivot)
        {
            case Pivot.Order: return size * index;
            case Pivot.Middle: return size * index - (size / 2.0f) * (length - 1);
            default: return Vector3.zero;
        }
    }
}