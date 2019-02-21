using System;
using UnityEngine;

[Serializable]
public struct CirclePosition
{
    public Transform direction;

    public CirclePosition(Transform direction)
    {
        this.direction = direction;
    }

    public Vector3 GetPosition(float radius, int length, int index)
    {
        return GetPosition(radius, length, index, 0);
    }

    public Vector3 GetPosition(float radius, int length, int index, float offsetDegree)
    {
#if DEBUG
        if (direction == null)
            throw new InvalidOperationException("\'direction\' cannot be null.");
        if (length <= 0)
            throw new InvalidOperationException("\'length\' cannot be zero or below.");
        if (index < 0)
            throw new InvalidOperationException("\'index\' cannot below from zero.");
#endif
        if (length > 1)
        {
            float sector = ((index * 360f / (float)length) + offsetDegree) * Mathf.PI / 180;
            float x = radius * Mathf.Cos(sector);
            float y = radius * Mathf.Sin(sector);
            return new Vector3(x, y, 0);
        }
        else
        {
            return Vector3.zero;
        }
    }
}