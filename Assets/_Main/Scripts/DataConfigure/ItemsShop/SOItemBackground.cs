

using MLib;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Background", menuName = "Tetris Setup/Data Configure/Item Background")]
public class SOItemBackground : SOItemShop
{
    private Dictionary<int, bool> saveData => DataManager.Instance.ItemsBackground;
    public override bool IsUnlock()
    {
        if (saveData.ContainsKey(id)) return saveData[id];
        return false;
    }

    public override void Unlock()
    {
        saveData[id] = true;
    }

    public override void Equip()
    {
        DataManager.Instance.LocalData.usingBackground = id;
    }

    public override void UnEquip()
    {
        DataManager.Instance.LocalData.usingBackground = -1;
    }
    public override bool IsUsing()
    {
        return DataManager.Instance.usingBackground == id;
    }
}