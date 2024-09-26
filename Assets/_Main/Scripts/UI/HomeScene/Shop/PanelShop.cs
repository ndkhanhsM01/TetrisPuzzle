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
        btnBuy.AddListener(OnClick_Buy);
        OnSelectItem += HandleItemSelect;
    }
    private void OnDisable()
    {
        btnBuy.RemoveListener(OnClick_Buy);
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

        DataManager.Instance.Coin -= itemSelect.Info.Price;
        itemSelect.Info.Unlock();

        if (activeTab) activeTab.Reload();

        itemSelect.Select();
    }
    private void HandleItemSelect(ShopItem item)
    {
        var newItem = item;
        if (!newItem) return;

        if (itemSelect) itemSelect.UnSelect();

        if (newItem.Info.IsUnlock())
        {
            btnBuy.SetActive(false);
        }
        else if (DataManager.Instance.Coin >= newItem.Info.Price)
        {
            btnBuy.SetActive(true);
        }
        else
        {
            btnBuy.SetActive(false);
        }

        //------------
        itemSelect = newItem;
    }

    private void HandleUseItem(SOItemShop item)
    {

    }
}