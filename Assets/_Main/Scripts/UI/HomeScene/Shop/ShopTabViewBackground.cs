

using MLib;

public class ShopTabViewBackground : ShopTapView
{
    public override void RegistDatasItem()
    {
        if (DataManager.Instance.ItemsBackground != null && DataManager.Instance.ItemsBackground.Count > allDatas.Length) return;

        DataManager.Instance.LocalData.Initialize_ItemsBackground(allDatas);
    }
}