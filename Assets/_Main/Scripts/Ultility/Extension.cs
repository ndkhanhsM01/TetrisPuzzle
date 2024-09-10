using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public static void SetAlpha(this Image image, float value)
    {
        Color baseColor = image.color;
        baseColor.a = value;
        image.color = baseColor;
    }
    public static void SetBaseColor(this Image image, Color newColor)
    {
        Color baseColor = image.color;
        baseColor.r = newColor.r;
        baseColor.g = newColor.g;
        baseColor.b = newColor.b;
        image.color = baseColor;
    }

    public static void SetActive(this MonoBehaviour component, bool active)
    {
        component.gameObject.SetActive(active);
    }

    public static T GetRandom<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}
