using UnityEngine;

public static class Gizmos2D
{
    public static void DrawWireBox(Vector3 center, Vector2 size)
    {
        Gizmos.DrawLine(Combine2D(center, new Vector2(size.x, size.y)), Combine2D(center, new Vector2(-size.x, size.y)));
        Gizmos.DrawLine(Combine2D(center, new Vector2(-size.x, size.y)), Combine2D(center, new Vector2(-size.x, -size.y)));
        Gizmos.DrawLine(Combine2D(center, new Vector2(-size.x, -size.y)), Combine2D(center, new Vector2(size.x, -size.y)));
        Gizmos.DrawLine(Combine2D(center, new Vector2(size.x, -size.y)), Combine2D(center, new Vector2(size.x, size.y)));
    }

    private static Vector3 Combine2D(Vector3 a, Vector2 b)
    {
        return new Vector3(a.x + b.x, a.y + b.y, a.z);
    }
}