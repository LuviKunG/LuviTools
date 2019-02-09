using System;
using UnityEngine;

[Serializable]
public struct GridPosition
{
    public Vector3 direction;
    public PivotPosition position;

    public GridPosition(PivotPosition position, Vector3 direction)
    {
        this.direction = direction;
        this.position = position;
    }

    public Vector3 GetPosition(int limit, int index)
    {
        int col = index % limit;
        int row = Mathf.FloorToInt(index / (float)limit);
        return position.GetPosition(limit, col) + row * direction;
    }
}