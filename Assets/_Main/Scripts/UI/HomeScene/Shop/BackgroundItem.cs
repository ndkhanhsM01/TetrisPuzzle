

using MLib;

public class BackgroundItem : ShopItem
{
    protected override bool IsUsing()
    {
        return info.ID == DataManager.Instance.usingBackground;
    }
}