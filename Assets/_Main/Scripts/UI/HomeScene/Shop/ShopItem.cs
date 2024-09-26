
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem: MonoBehaviour
{
    [SerializeField] protected Image imgPreview;
    [SerializeField] protected TMP_Text tmpPrice;
    [SerializeField] protected Button button;
    [SerializeField] protected GameObject goPrice;
    [SerializeField] protected GameObject goSelect;
    [SerializeField] protected GameObject goEquiped;

    protected SOItemShop info;
    public SOItemShop Info => info;
    private void OnEnable()
    {
        button.onClick.AddListener(OnClick);   
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }

    public void Initialize(SOItemShop info)
    {
        this.info = info;
        imgPreview.sprite = info.Preview;
        tmpPrice.text = $"{info.Price}";

        bool isUnlock = info.IsUnlock();
        goPrice.SetActive(!info.IsUnlock());
        goEquiped.SetActive(info.IsUsing() && info.IsUnlock());
        goSelect.SetActive(false);
    }
    public void Select()
    {
        goSelect.SetActive(true);
    }
    public void UnSelect()
    {
        goSelect.SetActive(false);
    }
    public void Equip()
    {
        goEquiped.SetActive(true);
        info.Equip();
    }
    public void UnEquip()
    {
        goEquiped.SetActive(false);
        info.UnEquip();
    }

    private void OnClick()
    {
        Select();
        PanelShop.OnSelectItem?.Invoke(this);
    }
}