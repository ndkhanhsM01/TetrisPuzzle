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
        EventsCenter.OnSceneLoaded += RegistDatasItem;
    }

    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= RegistDatasItem;
    }
    public SOItemBackground GetByID(int id)
    {
        return all[id];
    }
    public void RegistDatasItem()
    {
        if (saveDatas != null && saveDatas.Count == all.Length) return;

        DataManager.Instance.LocalData.Initialize_ItemsBackground(all);
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