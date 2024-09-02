using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    #region Vector
    public static Vector3 ToCeil(this Vector3 value)
    {
        return new Vector3(Mathf.CeilToInt(value.x), Mathf.CeilToInt(value.y), Mathf.CeilToInt(value.z));
    }
    public static Vector3 ToFloor(this Vector3 value)
    {
        return new Vector3(Mathf.FloorToInt(value.x), Mathf.FloorToInt(value.y), Mathf.FloorToInt(value.z));
    }

    public static Vector2 ToCeil(this Vector2 value)
    {
        return new Vector2(Mathf.CeilToInt(value.x), Mathf.CeilToInt(value.y));
    }

    public static Vector2 ToFloor(this Vector2 value)
    {
        return new Vector2(Mathf.FloorToInt(value.x), Mathf.FloorToInt(value.y));
    }
    #endregion
}
