using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
    public Item armedWeapon { get; private set; }
    public int slotIndex;
    [SerializeField] public Dictionary<int, int> durabilityDict = new Dictionary<int, int>();

    // 事件：当背包数据变化时触发
    public event Action OnInventoryChanged;

    // 保存路径
    // private string SavePath => $"{Application.persistentDataPath}/inventory.json";
    public void AddItem(Item item)
    {
        
        foreach (Item existingItem in itemList)
        {
            if (existingItem.name == item.name && existingItem.heldAmount < existingItem.maxStackSize)
            {
                int spaceLeft = existingItem.maxStackSize - existingItem.heldAmount;
                existingItem.heldAmount += 1;
                Debug.Log($"{item.itemName}，当前数量: {existingItem.heldAmount}");
                OnInventoryChanged?.Invoke(); // 通知更新
                return;
            }
        }
        

        itemList.Add(item);
        
        if (item.itemType == ItemType.Weapon)
        {
            durabilityDict.Add(itemList.Count - 1, 100);
            Debug.Log($"添加武器到槽位 {itemList.Count - 1}，耐久值: {durabilityDict[itemList.Count - 1]}");
        }
        else
        {
            item.heldAmount += 1;
        }

        Debug.Log($"添加了新物品: {item.itemName}，数量: {item.heldAmount}");
        OnInventoryChanged?.Invoke(); // 通知更新
    }

    public void UseItem(int index)
    {
        if (index >= 0 && index < itemList.Count)
        {
            Item item = itemList[index];
            item.Use();

            if (item.itemType != ItemType.Weapon && item.itemType != ItemType.Currency)
            {
                item.heldAmount--;
                if (item.heldAmount <= 0)
                {
                    itemList.RemoveAt(index);
                    Debug.Log($"{item.itemName} 已用尽，从背包移除");
                }
            }
            else if (item.itemType == ItemType.Weapon)
            {
                IsArmed(item);
                slotIndex = index;
            }
            OnInventoryChanged?.Invoke(); // 通知更新
        }
    }

    public void IsArmed(Item item)
    {
        if (item is Gun gun)
        {
            if (gun.isEquipped)
            {
                armedWeapon = gun;
            }
            else
            {
                armedWeapon = null;
            }
            OnInventoryChanged?.Invoke(); // 通知更新
        }
    }

    public void reduceDurability()
    {
        if (durabilityDict.ContainsKey(slotIndex))
        {
            durabilityDict[slotIndex]--;
            if (durabilityDict[slotIndex] <= 0)
            {
                durabilityDict.Remove(slotIndex);
                itemList.RemoveAt(slotIndex);
                Debug.Log($"武器 {slotIndex} 已损坏，从背包移除");
            }
            OnInventoryChanged?.Invoke(); // 通知更新
        }
    }

    public void Clear()
    {
        itemList.Clear();
        durabilityDict.Clear();
        armedWeapon = null;
        Debug.Log("背包已清空");
        OnInventoryChanged?.Invoke(); // 通知更新
    }

    public void inventoryChanged()
    {
        OnInventoryChanged?.Invoke();
    }
    public void UpdateList()
    {
        for (int i = itemList.Count - 1; i >= 0; i--)
        {
            var item = itemList[i];
            if (item.heldAmount == 0 && item.itemType != ItemType.Weapon)
            {
                itemList.RemoveAt(i); // 直接按索引移除
                Debug.Log(item.itemName);
            }
        }
    }

    #region savedata

    // 保存背包数据
    // public void SaveInventory()
    // {
    //
    //     string json = JsonConvert.SerializeObject(durabilityDict);
    //     File.WriteAllText(SavePath, json);
    //
    //     if (string.IsNullOrEmpty(json) || json == "{}")
    //     {
    //         Debug.LogWarning("生成的 JSON 数据为空，可能是 durabilityDict 没有内容");
    //         return;
    //     }
    //     // Debug.Log($"背包数据已保存到: {SavePath}");
    //     
    // }

    // 加载背包数据
    // public void LoadInventory()
    // {
    //     if (File.Exists(SavePath))
    //     {
    //         string json = File.ReadAllText(SavePath);
    //         durabilityDict = JsonConvert.DeserializeObject<Dictionary<int, int>>(json);
    //         
    //         Debug.Log("背包数据已加载");
    //         OnInventoryChanged?.Invoke();
    //     }
    //     else
    //     {
    //         Debug.Log("没有找到保存文件");
    //     }
    // }
    
    

    // 用于序列化的辅助类
    [Serializable]
    private class InventorySaveData
    {
        public Dictionary<int, int> durabilityDict = new Dictionary<int, int>();
    }

    #endregion
}