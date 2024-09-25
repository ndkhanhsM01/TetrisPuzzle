
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ShopItem: MonoBehaviour
{
    [SerializeField] protected Image imgPreview;
    [SerializeField] protected TMP_Text tmpPrice;
    [SerializeField] protected Button button;
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

        goSelect.SetActive(false);
        goEquiped.SetActive(IsUsing());
    }
    protected abstract bool IsUsing();
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
    }
    public void UnEquip()
    {
        goEquiped.SetActive(false);
    }

    private void OnClick()
    {
        Select();
        PanelShop.OnSelectItem?.Invoke(this);
    }
}