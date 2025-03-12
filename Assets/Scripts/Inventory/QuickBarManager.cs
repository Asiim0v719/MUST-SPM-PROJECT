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

    
    
    private int selectedIndex = -1; // 当前选中的物品索引

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }
    
    void Start()
    {
    }
    
    void Update()
    {
        
        UpdateQuickBarUI();
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
        Debug.Log("Update QuickBar UI");
        foreach (Transform child in slotGridQuickBar.transform)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < Math.Min(8, inventory.itemList.Count); i++)
        {
            Item item = inventory.itemList[i];
            
            Slot newSlot = Instantiate(slotPrefabQuickBar, slotGridQuickBar.transform.position, Quaternion.identity);
            newSlot.transform.SetParent(slotGridQuickBar.transform, false); // 设置父物体，保持本地坐标
            
            newSlot.slotItem = item;
            newSlot.slotImage.sprite = item.itemImage; // 假设 Item 有 icon 字段
            newSlot.slotNum.text = item.amount.ToString();
        }
    }
    
    
    
}
