using MLib;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PanelShop: MPanel
{
    [SerializeField] private Button btnBuy;
    [SerializeField] private ShopTapView[] tapsView;

    public static Action<ShopItem> OnSelectItem;
    public static Action<ShopItem> OnUseItem;
    private ShopItem itemSelect;
    private ShopTapView activeTab;
    private void OnEnable()
    {
        OnSelectItem += HandleItemSelect;
    }
    private void OnDisable()
    {
        OnSelectItem -= HandleItemSelect;
    }
    public override void Show(Action onFinish)
    {
        base.Show(onFinish);
        if(!activeTab) tapsView[0].Select();
    }

    public void OnClick_Close()
    {
        this.Hide();
    }
    public void SetActiveTab(ShopTapView tab)
    {
        this.activeTab = tab;
        btnBuy.SetActive(false);
    }
    private void OnClick_Buy()
    {
        if (!itemSelect) return;

        itemSelect.Info.Unlock();
        itemSelect.Info.Use();
        if (!activeTab) activeTab.Reload();
    }
    private void HandleItemSelect(ShopItem item)
    {
        if (!item) return;

        if (itemSelect) itemSelect.UnSelect();

        itemSelect = item;
        if (item.Info.IsUnlock())
        {
            btnBuy.SetActive(false);
        }
        else if (DataManager.Instance.Coin >= item.Info.Price)
        {
            btnBuy.SetActive(true);
        }
        else
        {
            btnBuy.SetActive(false);
        }
    }
    private void HandleUseItem(SOItemShop item)
    {

    }
}