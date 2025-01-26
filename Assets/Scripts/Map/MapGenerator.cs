using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap mainTilemap;

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

    private bool[,] mapData; // Ture:ground，Flase:water


    public void GenerateMap()
    {
        GenerateMapData();

        GenerateTilemap();
    }

    private void GenerateMapData()
    {
        if (!isSeedRandom) seed = Time.time.GetHashCode();
        Random.InitState(seed);

        mapData = new bool[width, height];

        float randomOffset = Random.Range(-100000, 100000);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise(x * lacunarity + randomOffset, y * lacunarity + randomOffset);
                mapData[x, y] = noiseValue < waterRatio ? false : true;
            }
        }
    }


    private void GenerateTilemap()
    {
        CleanMap();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileBase tile = mapData[x, y] ? landTile : waterTile;
                mainTilemap.SetTile(new Vector3Int(x, y), tile);
            }
        }
    }


    public void CleanMap() => mainTilemap.ClearAllTiles();
    
}
