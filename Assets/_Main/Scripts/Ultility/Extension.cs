using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    #region Vector
    public static Vector3Int ToIntRound(this Vector3 value)
    {
        return new Vector3Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y), Mathf.RoundToInt(value.z));
    }

    public static Vector2Int ToIntRound(this Vector2 value)
    {
        return new Vector2Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
    }
    #endregion

    #region Gizmos
    public static void DrawBlankCell(this Vector3 position, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(position, 0.3f);
    }
    #endregion
}
