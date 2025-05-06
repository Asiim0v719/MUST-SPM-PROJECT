using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Serialization;

public class Save : MonoBehaviour
{
    public Inventory inventory;
    public Player player;
    public PlayerStats playerStats;
    public MapGenerator mapGenerator;
    public Motor motor;
    public ItemDatabase IDB;

    private string SaveDirectory => $"{Application.persistentDataPath}/Saves/";
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        DontDestroyOnLoad(gameObject);
    }

    
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.print();
        if (gameManager.isNewGame)
        {
            // 初始化新存档
            InitializeNewGameData();
        
            // 应用手动输入的地图参数
            mapGenerator.seed = gameManager.mapSeed;
            mapGenerator.map.ratio = gameManager.MapRatio;
            mapGenerator.item.ratio = gameManager.ItemRatio;
            mapGenerator.dustEnemy.ratio = gameManager.EnemyRatio;
            mapGenerator.InitMap();
        
            // 保存初始存档
            SaveAllData(gameManager.selectedSlot);
        
            // 重置标记
            gameManager.isNewGame = false;
        }
        else
        {
            // 正常加载存档
            LoadAllData(gameManager.selectedSlot);
        }
    }

    // 动态生成文件名
    public string GetSaveFileName(int slot) => $"save{slot}";

    public void SaveAllData(int slot)
    {
        try
        {
            if (!Directory.Exists(SaveDirectory))
                Directory.CreateDirectory(SaveDirectory);

            var saveData = new GameSaveData
            {
                inventory = GetInventoryData(),
                player = GetPlayerData(),
                motor = GetMotorData(),
                map = GetMapData(),
                items = GetItemData()
            };

            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText($"{SaveDirectory}{GetSaveFileName(slot)}.json", json);
            Debug.Log($"存档 {slot} 已保存");
        }
        catch (Exception ex)
        {
            Debug.LogError($"保存失败: {ex.Message}");
        }
    }

    public void LoadAllData(int slot)
    {
        try
        {
            string path = $"{SaveDirectory}{GetSaveFileName(slot)}.json";
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

            // 加载数据
            LoadInventory(data.inventory);
            LoadPlayer(data.player);
            LoadMotor(data.motor);
            LoadMap(data.map);
            LoadItems(data.items);
        }
        catch (Exception ex)
        {
            Debug.LogError($"加载失败: {ex.Message}");
        }
    }

    #region Data Preparation
    private InventorySaveData GetInventoryData() => new InventorySaveData
    {
        durabilityEntries = inventory.durabilityDict.Select(kvp => new DurabilityEntry
        {
            slotIndex = kvp.Key,
            durability = kvp.Value
        }).ToList(),
        items = inventory.itemList
    };

    private PlayerSaveData GetPlayerData() => new PlayerSaveData
    {
        health = playerStats.currentHealth,
        position = new Vector3Serializable(player.transform.position)
    };

    private MotorSaveData GetMotorData() => new MotorSaveData
    {
        position = new Vector3Serializable(motor.transform.position)
    };

    private MapSaveData GetMapData() => new MapSaveData
    {
        seed = mapGenerator.seed,
        mapRatio = mapGenerator.map.ratio,
        itemRatio = mapGenerator.item.ratio,
        enemyRatio = mapGenerator.dustEnemy.ratio,
    };

    private List<ItemSaveData> GetItemData() => IDB.allItems
        .Select(item => new ItemSaveData
        {
            instanceID = item.GetInstanceID(),
            heldAmount = item.heldAmount
        }).ToList();
    #endregion

    #region Data Loading
    private void LoadInventory(InventorySaveData data)
    {
        inventory.durabilityDict = data.durabilityEntries
            .ToDictionary(e => e.slotIndex, e => e.durability);
        inventory.itemList = new List<Item>(data.items);
    }

    private void LoadPlayer(PlayerSaveData data)
    {
        playerStats.currentHealth = data.health;
        player.transform.position = data.position.ToVector3();
    }

    private void LoadMotor(MotorSaveData data) => motor.transform.position = data.position.ToVector3();

    private void LoadMap(MapSaveData data)
    {
        // 优先使用手动输入参数（新存档）
        mapGenerator.seed = gameManager.mapSeed != 0 ? gameManager.mapSeed : data.seed;
        mapGenerator.map.ratio = gameManager.MapRatio != 0 ? 
            gameManager.MapRatio : data.mapRatio;
        mapGenerator.item.ratio = gameManager.ItemRatio != 0 ? 
            gameManager.ItemRatio : data.itemRatio;
        mapGenerator.dustEnemy.ratio = gameManager.EnemyRatio != 0 ? 
            gameManager.EnemyRatio : data.enemyRatio;
        mapGenerator.InitMap();
    }

    private void LoadItems(List<ItemSaveData> items)
    {
        foreach (var itemData in items)
        {
            Item item = IDB.allItems.FirstOrDefault(i => i.GetInstanceID() == itemData.instanceID);
            if (item != null) item.heldAmount = itemData.heldAmount;
        }
    }
    #endregion

    private void InitializeNewGameData()
    {
        inventory.itemList.Clear();
        // 重置物品数据
        foreach (var item in IDB.allItems)
            item.heldAmount = 0;

        // 初始化玩家坐标
        player.transform.position = new Vector3(0, 1, 0);
        playerStats.currentHealth = playerStats.maxHealth;
    }

    public bool IsSaveFileExists(int slot) => 
        File.Exists($"{SaveDirectory}{GetSaveFileName(slot)}.json");
}

#region Data Structures
[Serializable]
public class GameSaveData
{
    public InventorySaveData inventory;
    public PlayerSaveData player;
    public MotorSaveData motor;
    public MapSaveData map;
    public List<ItemSaveData> items;
}

[Serializable]
public class InventorySaveData
{
    public List<DurabilityEntry> durabilityEntries;
    public List<Item> items;
}

[Serializable]
public class ItemSaveData
{
    public int instanceID;
    public int heldAmount;
}

[Serializable]
public class DurabilityEntry
{
    public int slotIndex;
    public int durability;
}

[Serializable]
public class PlayerSaveData
{
    public Stat health;
    public Vector3Serializable position;
}

[Serializable]
public class MotorSaveData
{
    public Vector3Serializable position;
}

[Serializable]
public class MapSaveData
{
    public int seed;
    [FormerlySerializedAs("waterRatio")] public float mapRatio;
    public float itemRatio;
    public float enemyRatio;
}

[Serializable]
public class Vector3Serializable
{
    public float x, y, z;

    public Vector3Serializable(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}
#endregion