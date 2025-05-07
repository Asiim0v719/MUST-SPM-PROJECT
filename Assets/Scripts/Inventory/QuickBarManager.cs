using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class QuickBarManager : MonoBehaviour
{
    private static QuickBarManager instance;

    public Inventory inventory;
    public GameObject slotGridQuickBar;
    public Slot slotPrefabQuickBar;

    public void SubscribeToInventoryChanges(Inventory inventory)
    {
        inventory.OnInventoryChanged += UpdateQuickBarUI;  // 订阅事件
        inventory.OnInventoryChanged += this.inventory.UpdateList;
    }
    
    // private int selectedIndex = -1; // 当前选中的物品索引

    private void Awake()
    {
        UpdateQuickBarUI();
        if (instance != null)
            Destroy(this);
        instance = this;
    }
    
    void Start()
    {
        SubscribeToInventoryChanges(inventory);
        UpdateQuickBarUI();
    }
    
    void Update()
    {
        inventory.UpdateList();
        Vector3 mousePosition = Input.mousePosition;
        // Debug.Log($"鼠标世界位置: {mousePosition}");
        // 检查暂停状态
        if (Time.timeScale == 0) return;

        for (int i = 0; i < 10; i++)
        {
            // KeyCode.Alpha1 到 KeyCode.Alpha0 对应数字键 1 到 0
            KeyCode key = (KeyCode)((int)KeyCode.Alpha1 + i);
            if (Input.GetKeyDown(key))
            {
                inventory.UseItem(i); // 使用对应槽位的物品
                break; // 找到按下的按键后退出循环
            }
        }
        
    }
    
    
    
    public void UpdateQuickBarUI()
    {
        // Debug.Log("Update QuickBar UI");
        foreach (Transform child in slotGridQuickBar.transform)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < Math.Min(9, inventory.itemList.Count); i++)
        {
            Item item = inventory.itemList[i];
            
            Slot newSlot = Instantiate(slotPrefabQuickBar, slotGridQuickBar.transform.position, Quaternion.identity);
            newSlot.transform.SetParent(slotGridQuickBar.transform, false); // 设置父物体，保持本地坐标
            
            newSlot.slotItem = item;
            newSlot.index = i;
            newSlot.slotImage.sprite = item.itemImage; // 假设 Item 有 icon 字段
            newSlot.slotNum.text = item.heldAmount.ToString();
        }


    }
    
    
    
}
