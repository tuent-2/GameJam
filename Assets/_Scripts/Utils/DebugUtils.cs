using UnityEngine;

public static class DebugUtils
{
    public static void DrawRect(Vector3 min, Vector3 max, Color color)
    {
        Debug.DrawLine(min, new Vector3(min.x, max.y), color);
        Debug.DrawLine(new Vector3(min.x, max.y), max, color);
        Debug.DrawLine(max, new Vector3(max.x, min.y), color);
        Debug.DrawLine(min, new Vector3(max.x, min.y), color);
    }

    public static void DrawRect(Vector3 min, Vector3 max, Color color, float duration)
    {
        Debug.DrawLine(min, new Vector3(min.x, max.y), color, duration);
        Debug.DrawLine(new Vector3(min.x, max.y), max, color, duration);
        Debug.DrawLine(max, new Vector3(max.x, min.y), color, duration);
        Debug.DrawLine(min, new Vector3(max.x, min.y), color, duration);
    }

    public static void DrawRect(Rect rect, Color color)
    {
        DrawRect(rect.min, rect.max, color);
    }

    public static void DrawRect(Rect rect, Color color, float duration)
    {
        DrawRect(rect.min, rect.max, color, duration);
    }
}