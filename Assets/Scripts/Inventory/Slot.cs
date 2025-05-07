using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;


public class Slot : MonoBehaviour
{
    public Inventory inventory;
    public Item slotItem;
    public Image slotImage;
    public TextMeshProUGUI slotNum;

    public Slider slider; // Slider 组件
    public Image fillImage; // 填充区域的 Image 组件

    
    private Color greenColor = Color.green; // 0.8 以上
    private Color yellowColor = Color.yellow; // 0.4 到 0.8
    private Color redColor = Color.red; // 0 到 0.4
    public int index; // 用于标识槽位的索引

    void Start()
    {
        // 如果物品不是武器，隐藏耐久度 slider，反之隐藏数量
        if (slotItem.itemType != ItemType.Weapon)
        {
            slider.gameObject.SetActive(false);
        }
        else
        {
            slotNum.gameObject.SetActive(false);
        }

        if (!inventory.durabilityDict.ContainsKey(index)&&slotItem.itemType == ItemType.Weapon)
        {
            updateDuraDict();
        }
    }

    private void Update()
    {
        // 如果字典中包含当前槽位的索引，更新耐久度显示
        if (inventory.durabilityDict.ContainsKey(index))
        {
            slider.value = inventory.durabilityDict[index] * 0.01f;
            UpdateColor(slider.value);
        }
    }

    // 更新槽位颜色显示
    void UpdateColor(float value)
    {
        if (value >= 0.8f)
        {
            fillImage.color = greenColor; // 绿色
        }
        else if (value >= 0.4f)
        {
            fillImage.color = yellowColor; // 黄色
        }
        else
        {
            fillImage.color = redColor; // 红色
        }
    }

    // 在点击槽位时调用，更新物品信息
    public void ItemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotItem.itemInfo);
    }

    private void updateDuraDict()
    {
        if (inventory.durabilityDict.ContainsKey(index + 1))
        {       
            int value = inventory.durabilityDict[index + 1];
                 inventory.durabilityDict.Remove(index + 1);
                 inventory.durabilityDict.Add(index, value);
        }


    }
}
    
    
        

