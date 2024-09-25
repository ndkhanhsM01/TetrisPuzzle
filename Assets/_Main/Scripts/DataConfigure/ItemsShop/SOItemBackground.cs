

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
        DataManager.Instance.LocalData.itemsBackground[id] = true;
    }

    public override void Use()
    {
        
    }
}