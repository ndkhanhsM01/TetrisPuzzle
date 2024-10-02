using MLib;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PanelShop: MPanel
{
    [SerializeField] private Button btnBuy;
    [SerializeField] private Button btnEquip;
    [SerializeField] private Image imgPreview;
    [SerializeField] private ShopTapView[] tapsView;

    public static Action<ShopItem> OnSelectItem;
    public static Action<ShopItem> OnUseItem;
    private ShopItem itemSelect;
    private ShopTapView activeTab;
    private void OnEnable()
    {
        btnBuy.AddListener(OnClick_Buy);
        btnEquip.AddListener(OnClick_Equip);
        OnSelectItem += HandleItemSelect;
    }
    private void OnDisable()
    {
        btnBuy.RemoveListener(OnClick_Buy);
        btnEquip.RemoveListener(OnClick_Equip);
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
        itemSelect = null;
        btnBuy.SetActive(false);
        btnEquip.SetActive(false);
    }
    private void OnClick_Buy()
    {
        if (!itemSelect) return;

        if (MAudioManager.Instance) MAudioManager.Instance.PlaySFX(MSoundType.EarnCoin);
        DataManager.Instance.Coin -= itemSelect.Info.Price;
        itemSelect.Info.Unlock();

        if (activeTab) activeTab.Reload();
        HandleItemSelect(itemSelect);
        itemSelect.Select();
    }

    private void OnClick_Equip()
    {
        if(!itemSelect) return;

        DataManager.Instance.LocalData.usingBackground = itemSelect.Info.ID;
        if (activeTab) activeTab.Reload();

        itemSelect.Equip();
    }

    private void HandleItemSelect(ShopItem item)
    {
        var newItem = item;
        if (!newItem) return;

        if (itemSelect) itemSelect.UnSelect();

        if (newItem.Info.IsUnlock())
        {
            btnBuy.SetActive(false);
            btnEquip.SetActive(true);
        }
        else if (DataManager.Instance.Coin >= newItem.Info.Price)
        {
            btnBuy.SetActive(true);
            btnEquip.SetActive(false);
        }
        else
        {
            btnBuy.SetActive(false);
            btnEquip.SetActive(false);
        }

        //------------
        itemSelect = newItem;
    }

    private void HandleUseItem(SOItemShop item)
    {

    }
}