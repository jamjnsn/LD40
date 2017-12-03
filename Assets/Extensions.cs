using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{
    public static RectTransform GetRectTransform(this MonoBehaviour monoBehaviour)
    {
        return monoBehaviour.GetComponent<RectTransform>();
    }

    public static void TryInvoke(this System.Action action)
    {
        if (action != null)
        {
            action();
        }
    }

    /// <summary>
    /// Casts a Vector2 to Vector2Int.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector2Int ToInt(this Vector2 vector)
    {
        return new Vector2Int((int)vector.x, (int)vector.y);
    }

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector2 RoundToInt(this Vector2 vector)
    {
        return new Vector2(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }

    public static Vector3 ToVector3(this Vector2 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }

    public static Vector3 ToVector3(this Vector2 vector)
    {
        return vector.ToVector3(0);
    }
}
