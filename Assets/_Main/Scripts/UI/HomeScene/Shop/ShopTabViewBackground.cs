

using MLib;
using System.Collections.Generic;

public class ShopTabViewBackground : ShopTapView
{
    private Dictionary<int, bool> saveDatas => DataManager.Instance.ItemsBackground;
    public override void RegistDatasItem()
    {
        if (saveDatas != null && saveDatas.Count == allDatas.Length) return;

        DataManager.Instance.LocalData.Initialize_ItemsBackground(allDatas);
    }
}