using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BuildType
{
    Item,       // 普通物品
    Building    // 建筑物
}
[System.Serializable]
public class BuildItem
{
    public Sprite image;
    public Item forgedItem;
    public BuildType type;                   // 新增字段：类型（建筑物或物品）

    public List<ConsumedItemInfo> consumedItems;
}

[System.Serializable]
public class ConsumedItemInfo
{
    public Item item;
    public int amount;
}

public class CraftManager : MonoBehaviour
{
    public List<BuildItem> BuildItems;
    //[SerializeField] private UI_Craft uiCraft;
    [SerializeField] private GameObject hint;

    public void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player"))
        if (collision.GetComponent<Player>())
        {
            if (UI_Craft.instanceSelf!=null)
            {
                UI_Craft.instanceSelf.SetCraftManager(this); 
                UI_Craft.instanceSelf.UpdateCraftrUI();
            }

            if (hint.activeSelf == false)
                hint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 玩家离开时清空或隐藏 UI
        //if (collision.CompareTag("Player"))
        if (collision.GetComponent<Player>())
        {
            if (UI_Craft.instanceSelf != null)
            {
                UI_Craft.instanceSelf.SetCraftManager(null); 
                UI_Craft.instanceSelf.UpdateCraftrUI();
            }

            if(hint.activeSelf == true)
                hint.SetActive(false);
        }
    }
}