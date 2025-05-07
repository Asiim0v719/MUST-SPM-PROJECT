using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_Craft : MonoBehaviour
{
    public static UI_Craft instanceSelf;
    public Inventory inventory;
    public CraftManager instance;
    public GameObject slotGrid;
    public ForgedItem slotPrefab;
    [Space] public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemInfo;
    public Image PreviewImage;
    public Button BuildButton;
    public QuickBarManager quickBar;
    [Space]
    public GameObject ConsumedslotGrid;
    public ConsumedItem ConsumedslotPrefab;
    
    [Space] 
    private bool isBuilding = false;
    public Image previewImage;
    public GameObject buildPrefab;
    
    [Space] 
    private int currentPointer;
    private bool itemComsumed= true;
    public GameObject itemOnworld;

    private void Awake()
    {
        if (instanceSelf != null)
            Destroy(instanceSelf);
        else
            instanceSelf = this;
    }
    private void Start()
    {
        UpdateCraftrUI();
        UpdateCraftP2();
        // inventory.LoadInventory();
        previewImage.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isBuilding)
        {
            updateBuilding();
        }
        Debug.Log("1");
        PrintDictionary();
    }
    
    public void PrintDictionary()
    {
        string dictContent = inventory.durabilityDict.Count > 0 
            ? string.Join(", ",inventory.durabilityDict.Select(kvp => $"[{kvp.Key}]: {kvp.Value}"))
            : "空";
        Debug.Log($"durabilityDict: {dictContent}");
    }
    public void SetCraftManager(CraftManager craftManager)
    {
        instance = craftManager;
    }
    void OnSlotClick(int index)
    {
        Debug.Log($"index: {index}");
        currentPointer = index;
        UpdateCraftP2();
    }

    public void StartCraft()
    {
        inventory.UpdateList();
        if(instance == null)
            return;
        BuildItem item = instance.BuildItems[currentPointer];
        switch (item.type)
        {
            case BuildType.Item:
                inventory.AddItem(item.forgedItem);
                itemComsumed = false;
                Debug.Log(item.forgedItem.name+" is built");
                break;
            case BuildType.Building:
                // Debug.Log("start building");
                isBuilding = true;
                previewImage.gameObject.SetActive(true); 
                previewImage.sprite = item.image;
                break;
            default:
                Debug.Log("Need type");
                break;
        }
        UpdateCraftP2();
        quickBar.UpdateQuickBarUI();
    }
    #region Update UI
    public void UpdateCraftrUI()
    {
        
        foreach (Transform child in slotGrid.transform)
        {
            Destroy(child.gameObject);
        }

        if (instance != null)
        {
            for (int i = 0; i < instance.BuildItems.Count; i++)
            {
                BuildItem item = instance.BuildItems[i];

                ForgedItem newSlot = Instantiate(slotPrefab, slotGrid.transform.position, Quaternion.identity);
                newSlot.transform.SetParent(slotGrid.transform, false);

                newSlot.Image.sprite = item.image;


                Button slotButton = newSlot.GetComponentInChildren<Button>();
                if (slotButton != null)
                {
                    int slotIndex = i; // 捕获当前索引
                    slotButton.onClick.AddListener(() => OnSlotClick(slotIndex));
                }
            }
        }
    }
    private void UpdateCraftP2()
    {
        if (instance == null)
        {
            Debug.Log("需要工作台");
            BuildButton.interactable = false;
            return;
        }
        BuildItem item = instance.BuildItems[currentPointer];
        
        ItemName.text = item.forgedItem.name;
        ItemInfo.text = item.forgedItem.itemInfo;
        PreviewImage.sprite = item.image;

        // about building
        bool canBuild;
        BuildButton.interactable = false;
        
        foreach (Transform child in ConsumedslotGrid.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < item.consumedItems.Count; i++)
        {
            ConsumedItem newSlot = Instantiate(ConsumedslotPrefab, ConsumedslotGrid.transform.position,
                Quaternion.identity);
            newSlot.transform.SetParent(ConsumedslotGrid.transform, false);

            if (itemComsumed == false)
            {
                Debug.Log("consume material");
                item.consumedItems[i].item.heldAmount -= item.consumedItems[i].amount;
                inventory.inventoryChanged();
            }
            newSlot.amount.text = item.consumedItems[i].item.heldAmount + "/" + item.consumedItems[i].amount;
            newSlot.Image.sprite = item.consumedItems[i].item.itemImage;

            if (item.consumedItems[i].amount > item.consumedItems[i].item.heldAmount)
            {
                canBuild = false;
                BuildButton.interactable = canBuild;
            }
            else
            {
                canBuild = true;
                BuildButton.interactable = canBuild;
            }
            
        }

        itemComsumed = true;
        // Debug.Log(canBuild);
    }
    #endregion

    #region Build structure

    public GameObject bs;
    public GameObject player;
    public Camera miniMapCamera;

    Ray ray;
    void updateBuilding()
    {
        Vector3 mousePosition = Input.mousePosition;
        previewImage.transform.position = mousePosition;
        // previewImage.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

        if (Input.GetMouseButtonDown(0))
        {
            // Build(mousePosition);
            // Vector3? worldPosition = GetMouseWorldPosition();
            // if (worldPosition.HasValue)
                StartBuilding(MouseWorldPosition3D.instance.worldPosition.Value);    

        }
        
        // 右键点击：取消建造
        if (Input.GetMouseButtonDown(1))
        {
            CancelBuilding();
        }
    }
    
    void CancelBuilding()
    {
        isBuilding = false;
        previewImage.gameObject.SetActive(false);
    }
    void StartBuilding(Vector3 position)
    {
        Debug.Log("item built"+position);
        itemComsumed = false;
        GameObject builtObject = Instantiate(buildPrefab, position, Quaternion.identity, ItemManager.instance.transform);
        // builtObject.transform.SetParent(itemOnworld.transform, false);
        SpriteRenderer renderer = builtObject.GetComponentInChildren<SpriteRenderer>();
        Debug.Log($"Renderer 是否为 null: {renderer == null}");
        if (renderer != null)
        {
            renderer.sprite = previewImage.sprite;
            Debug.Log("替换");
        }
        isBuilding = false;
        previewImage.gameObject.SetActive(false);
        UpdateCraftP2();
    }
    
    Vector3? GetMouseWorldPosition()
    {
        Vector3 worldPosition = miniMapCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2, miniMapCamera.nearClipPlane));

        Vector3 rayOrigin = new Vector3(worldPosition.x + PlayerManager.instance.playerTransform.position.x * 10, worldPosition.y + PlayerManager.instance.playerTransform.position.y * 10, 0);

        ray = new Ray(rayOrigin, Vector3.forward);

        Vector3 intersectionPoint = ray.origin;

        return new Vector3(intersectionPoint.x / 10, intersectionPoint.y / 10, 0f);
    }
    #endregion
    

    
}