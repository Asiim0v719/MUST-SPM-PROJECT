using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]

public class DynamicMapGenerator : MonoBehaviour
{
    public Tilemap landTilemap;
    public Tilemap waterTilemap;

    public int width;
    public int height;

    public int seed;
    public bool isSeedRandom;

    [Range(0, 1f)]
    public float lacunarity;

    [Range(0, 1f)]
    public float waterRatio;

    public TileBase landTile;
    public TileBase waterTile;

    private float randomOffset;

    private bool[,] mapData; // Ture:land; Flase:water
    
    private Transform playerTransform;
    
    private bool isMapCleaned;

    private void Start()
    {
        InitializeMap();
    }

    private void Update()
    {
        UpdateTilemap();
    }

    #region Item
    private bool IsInMapRange(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;

    private bool IsLand(int x, int y) => mapData[x, y] == false;

    private int GetEigthNeighborsGroundCount(int x, int y)
    {
        int count = 0;

        if (IsInMapRange(x, y + 1) && IsLand(x, y + 1)) 
            count += 1;

        if (IsInMapRange(x, y - 1) && IsLand(x, y - 1)) 
            count += 1;

        if (IsInMapRange(x - 1, y) && IsLand(x - 1, y)) 
            count += 1;

        if (IsInMapRange(x + 1, y) && IsLand(x + 1, y)) 
            count += 1;


        if (IsInMapRange(x - 1, y + 1) && IsLand(x - 1, y + 1)) 
            count += 1;

        if (IsInMapRange(x + 1, y + 1) && IsLand(x + 1, y + 1)) 
            count += 1;

        if (IsInMapRange(x - 1, y - 1) && IsLand(x - 1, y - 1)) 
            count += 1;

        if (IsInMapRange(x + 1, y - 1) && IsLand(x + 1, y - 1)) 
            count += 1;
        
        return count;
    }
    #endregion

    #region Map
    #region Initialization
    private void InitializeMap()
    {
        InitializePlayerTransform();

        InitializeMapData();

        InitializeTilemap();
    }

    private void InitializePlayerTransform() => playerTransform = PlayerManager.instance.playerTransform;

    private void InitializeMapData()
    {
        if (!isSeedRandom) 
            seed = Time.time.GetHashCode();

        UnityEngine.Random.InitState(seed);

        mapData = new bool[width, height];

        randomOffset = UnityEngine.Random.Range(-100000, 100000);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise(x * lacunarity + randomOffset, y * lacunarity + randomOffset);
                mapData[x, y] = noiseValue < waterRatio ? false : true;
            }
        }
    }

    private void InitializeTilemap() 
    {
       CleanTilemap();

        for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++) 
            {
                TileBase tile;

                if (mapData[x, y])
                {
                    tile = landTile;
                    landTilemap.SetTile(new Vector3Int(x + (int)playerTransform.position.x - width / 2, y + (int)playerTransform.position.y - height / 2), tile);
                }
                else
                {
                    tile = waterTile;
                    waterTilemap.SetTile(new Vector3Int(x + (int)playerTransform.position.x - width / 2, y + (int)playerTransform.position.y - height / 2), tile);
                }
            

                //TileBase tile = mapData[x, y] ? landTile : waterTile;
                //landTilemap.SetTile(new Vector3Int(x + (int)playerTransform.position.x - width / 2, y + (int)playerTransform.position.y - height / 2), tile);
            }
        }
    }

    private void CleanTilemap() 
    {
        landTilemap.ClearAllTiles();
        waterTilemap.ClearAllTiles();
    } 
    #endregion

    #region Update
    private void UpdateTilemap() 
    {
        CleanUnseenMap();

        bool[,] updatedMapData = new bool[width, height];

        int playerOffsetX = (int)playerTransform.position.x - width / 2;
        int playerOffsetY = (int)playerTransform.position.y - height / 2;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise((playerOffsetX + x) * lacunarity + randomOffset, (playerOffsetY + y) * lacunarity + randomOffset);
                updatedMapData[x, y] = noiseValue < waterRatio ? false : true;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileBase tile;

                if (updatedMapData[x, y])
                {
                    tile = landTile;
                    landTilemap.SetTile(new Vector3Int(x + (int)playerTransform.position.x - width / 2, y + (int)playerTransform.position.y - height / 2), tile);
                }
                else 
                {
                    tile = waterTile;
                    waterTilemap.SetTile(new Vector3Int(x + (int)playerTransform.position.x - width / 2, y + (int)playerTransform.position.y - height / 2), tile);
                }

                //TileBase tile = updatedMapData[x, y] ? landTile : waterTile;
                //landTilemap.SetTile(new Vector3Int(x + (int)playerTransform.position.x - width / 2, y + (int)playerTransform.position.y - height / 2), tile);
            }
        }
    }

    private void CleanUnseenMap() 
    {
        if (!Input.anyKey && isMapCleaned == false)
        {
            CleanTilemap();
            isMapCleaned = true;
        }
        else if (Input.anyKey)
            isMapCleaned = false;
    }
    #endregion

    #endregion
}
