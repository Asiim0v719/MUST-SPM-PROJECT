using UnityEngine;

public enum ItemType
{
    Weapon,
    Material,
    Potion
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int maxStackSize = 99;
    public int amount = 1;
    public ItemType itemType;
    public string itemInfo;

    // 抽象方法，每个子类必须实现
    public abstract void Use();
}