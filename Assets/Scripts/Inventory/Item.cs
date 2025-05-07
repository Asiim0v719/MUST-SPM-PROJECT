using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    Weapon,
    Material,
    Potion,
    Medic,
    Currency
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int maxStackSize = 99;
    [FormerlySerializedAs("amount")] public int heldAmount = 1;
    public ItemType itemType;
    public string itemInfo;
    public abstract void Use();
}