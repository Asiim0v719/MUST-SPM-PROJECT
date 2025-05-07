using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;

    public Inventory inventory;
    public GameObject slotGrid;
    public Slot slotPrefab;
    public TextMeshProUGUI itemInfo;
    
    public GameObject confirmPanel; 
    public Text confirmText;       
    // public Button confirmButton;

    
    
    private int selectedIndex = -1; // 当前选中的物品索引

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }
    
    void Start()
    {
        UpdateBagUI();
        confirmPanel.SetActive(false); // 初始隐藏确认框
    }

    private void Update()
    {
        
    }

    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInfo.text = itemDescription;
    }

    public void UpdateBagUI()
    {
        // Debug.Log("Update Bag UI");
        foreach (Transform child in slotGrid.transform)
        {
            Destroy(child.gameObject);
        }

        // 遍历 itemList 并生成槽
        for (int i = 0; i < inventory.itemList.Count; i++)
        {
            Item item = inventory.itemList[i];

            // 实例化槽预制体
            Slot newSlot = Instantiate(slotPrefab, slotGrid.transform.position, Quaternion.identity);
            newSlot.transform.SetParent(slotGrid.transform, false); // 设置父物体，保持本地坐标

            // 设置槽的内容
            newSlot.slotItem = item;
            newSlot.index = i;
            newSlot.slotImage.sprite = item.itemImage; // 假设 Item 有 icon 字段
            newSlot.slotNum.text = item.heldAmount.ToString();

            // 添加点击事件（可选）
            Button slotButton = newSlot.GetComponent<Button>();
            if (slotButton != null)
            {
                // Debug.Log(item.name);
                int slotIndex = i; // 捕获当前索引
                slotButton.onClick.AddListener(() => OnSlotClick(slotIndex));
            }
        }
    }
    
    void OnSlotClick(int index)
    {
        // Debug.Log($"点击槽 {index}");
        Item item = inventory.itemList[index];
        switch (item.itemType)
        {
            case ItemType.Weapon:
                inventory.UseItem(index);
                UpdateBagUI();
                break;

            case ItemType.Potion:
                selectedIndex = index;
                confirmText.text = $"是否使用 {item.itemName}？";
                confirmPanel.SetActive(true);
                // confirmButton.onClick.RemoveAllListeners();
                // confirmButton.onClick.AddListener(() => ConfirmUse());
                
                UpdateBagUI();

                break;

            case ItemType.Material:
                inventory.UseItem(index);
                UpdateBagUI();
                break;

            case ItemType.Medic:
                inventory.UseItem(index);
                UpdateBagUI();
                break;

            case ItemType.Currency:
                inventory.UseItem(index);
                UpdateBagUI();
                break;
        }
    }

    void ConfirmUse()
    {
        if (selectedIndex >= 0 && selectedIndex < inventory.itemList.Count)
        {
            inventory.UseItem(selectedIndex);
            UpdateBagUI(); // 更新背包 UI
        }
        confirmPanel.SetActive(false); // 关闭确认框
    }
    
    
}
