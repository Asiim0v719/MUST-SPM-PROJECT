// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int selectedSlot;    // 当前选中的存档槽位（1~3）
    public int mapSeed;             // 手动输入的地图种子
    public float MapRatio; 
    public float EnemyRatio; 
    public float ItemRatio; 
    public bool isNewGame; // 标记是否是新存档

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void print()
    {
        Debug.Log("selectedSlot:" + selectedSlot);
        Debug.Log("seed:" + mapSeed);
        Debug.Log("isNewGame: "+ isNewGame);
    }
}