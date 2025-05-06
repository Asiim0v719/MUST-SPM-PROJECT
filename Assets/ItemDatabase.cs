using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> allItems; // 所有物品的列表
    
    public Dictionary<int, Item> itemIdToItemMap = new Dictionary<int, Item>();
    
    private void Awake()
    {
        // 将所有物品的 ID 与物品本身映射
        foreach (var item in allItems)
        {
            int itemId = item.GetInstanceID(); // 获取物品的唯一 ID
            item.heldAmount = 0;
            if (!itemIdToItemMap.ContainsKey(itemId))
            {
                itemIdToItemMap.Add(itemId, item);
                // Debug.Log(itemId + item.itemName);
            }
        }
    }


    public Item GetItemById(int itemId)
    {
        if (itemIdToItemMap.ContainsKey(itemId))
        {
            // Debug.Log("zhaodaole");
            return itemIdToItemMap[itemId];
        }
        // Debug.Log("没找到"+itemId);
        return null; // 返回 null 如果未找到对应的物品
    }
}