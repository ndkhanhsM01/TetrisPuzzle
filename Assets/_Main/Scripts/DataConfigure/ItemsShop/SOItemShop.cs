


using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class SOItemShop: ScriptableObject
{
    [SerializeField] protected int id;
    [SerializeField] protected Sprite preview;
    [SerializeField] protected int price;

    public int ID => id;
    public Sprite Preview => preview;
    public int Price => price;
    public abstract void Equip();
    public abstract void UnEquip();
    public abstract void Unlock();
    public abstract bool IsUnlock();
    public abstract bool IsUsing();

#if UNITY_EDITOR
    public void SetIdDirty(int id)
    {
        this.id = id;
        EditorUtility.SetDirty(this);
    }
#endif
}