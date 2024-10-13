using UnityEngine;
using MLib;
using System.Collections.Generic;

public class BackgroundInGameConfig: MSingleton<BackgroundInGameConfig>
{
    [SerializeField] private SOItemBackground[] all;

    public SOItemBackground[] All => all;
    private Dictionary<int, bool> saveDatas => DataManager.Instance.ItemsBackground;
    private void OnEnable()
    {
        DataManager.OnLoadLocalSuccess += RegistDatasItem;
    }

    private void OnDisable()
    {
        DataManager.OnLoadLocalSuccess -= RegistDatasItem;
    }
    public SOItemBackground GetByID(int id)
    {
        return all[id >= 0 ? id : 0];
    }
    public void RegistDatasItem(LocalData localData)
    {
        if (saveDatas != null && saveDatas.Count == all.Length) return;

        localData.Initialize_ItemsBackground(all);
    }
#if UNITY_EDITOR

    [MButton]
    private void ValidateIdItems()
    {
        for(int i=0; i<all.Length; i++)
        {
            all[i].SetIdDirty(i);
        }
    }
#endif
}