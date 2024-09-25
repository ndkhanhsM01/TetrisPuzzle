

using MLib;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShopTapView: MonoBehaviour
{
    [SerializeField] protected Transform content;
    [SerializeField] protected ShopItem itemPrefab;

    [SerializeField] protected SOItemShop[] allDatas;

    [SerializeField] protected List<ShopItem> all_Items;

    private void OnEnable()
    {
        EventsCenter.OnSceneLoaded += RegistDatasItem;
    }

    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= RegistDatasItem;
    }
    public void Select()
    {
        MUIManager.Instance.GetPanel<PanelShop>().SetActiveTab(this);
        ShowContent();
    }
    public void Reload()
    {
        ShowContent();
    }
    public void ShowContent()
    {
        MakeSureEnoughItems();

        for (int i = 0; i < all_Items.Count; i++)
        {
            ShopItem item = all_Items[i];
            bool needShow = i < allDatas.Length;
            if (!needShow)
            {
                item.SetActive(false);
                continue;
            }

            item.Initialize(allDatas[i]);
            item.SetActive(true);
        }
    }
    private void MakeSureEnoughItems()
    {
        int spawnNewRequire = allDatas.Length - all_Items.Count;

        if (spawnNewRequire <= 0) return;

        for(int i = 0; i < spawnNewRequire; i++)
        {
            ShopItem itemClone = Instantiate(itemPrefab, content);
            itemClone.SetActive(false);
            all_Items.Add(itemClone);
        }
    }
    public abstract void RegistDatasItem();
#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        for (int i = 0; i < allDatas.Length; i++)
        {
            allDatas[i].SetIdDirty(i);
        }
    }
#endif
}