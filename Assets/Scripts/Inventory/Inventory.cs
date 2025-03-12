using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> itemList = new List<Item>(); // 物品列表
    public Item armedWeapon { get; private set; }
    public InventoryManager instance;
    // 添加物品到背包
    public void AddItem(Item item)
    {
        // 检查是否已有相同物品
        foreach (Item existingItem in itemList)
        {
            if (existingItem.name == item.name && existingItem.amount < existingItem.maxStackSize)
            {
                int spaceLeft = existingItem.maxStackSize - existingItem.amount;
                // existingItem.amount += Mathf.Min(item.amount, spaceLeft);
                existingItem.amount += 1;
                Debug.Log($"{item.itemName}，当前数量: {existingItem.amount}");
                return;
            }
        }

        // 如果没有找到相同物品或堆叠已满，添加新物品
        itemList.Add(item);
        Debug.Log($"添加了新物品: {item.itemName}，数量: {item.amount}");
    }

    // 使用指定索引的物品
    public void UseItem(int index)
    {
        if (index >= 0 && index < itemList.Count)
        {
            Item item = itemList[index];
            item.Use();

            // 武器不需要减少数量，其他类型在使用后减少
            if (item.itemType != ItemType.Weapon)
            {
                item.amount--;
                if (item.amount <= 0)
                {
                    itemList.RemoveAt(index);
                    Debug.Log($"{item.itemName} 已用尽，从背包移除");
                }
            }
            else
                isArmed(item);
        }
    }

    public void isArmed(Item item) 
    {
        if (item is Gun gun) 
        {
            if(gun.isEquipped)
                armedWeapon = gun;
            else
                armedWeapon = null;
        }
    }

    public void Clear()
    {
        itemList.Clear();
        Debug.Log("背包已清空");
    }
}