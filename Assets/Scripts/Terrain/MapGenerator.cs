using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh, Falloff};
    public DrawMode drawMode;


    public int mapWidth;
    public int mapHeight;
    private Vector2Int mapSize;
    public float noiseScale;

    public int octaves;
    [Range(0f,1f)]
    public float persistance;
    public float lacunarity;
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public int seed;
    public Vector2 offset;

    public bool useFallOff;

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    

    public bool autoUpdate;

    public TerrainType[] regions;

    float[,] falloffMap;
    
    [Range(0f,1f)] public float falloffStart;
    [Range(0f, 1f)] public float falloffEnd;


    private void Awake()
    {
        mapSize = new Vector2Int(mapWidth, mapHeight);
        falloffMap = FallOffGenerator.GenerateFalloffMap(mapSize,falloffStart,falloffEnd);
    }


    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (useFallOff)
                {
                    noiseMap[x,y] = Mathf.Clamp01(noiseMap[x,y] - falloffMap[x,y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        if (drawMode == DrawMode.NoiseMap)
        {
            DrawTexture(TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            DrawTexture(TextureFromColormap(colorMap, mapWidth, mapHeight));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureFromColormap(colorMap, mapWidth, mapHeight));
        }
        else if (drawMode == DrawMode.Falloff)
        {
            DrawTexture(TextureFromHeightMap(FallOffGenerator.GenerateFalloffMap(mapSize, falloffStart, falloffEnd)));
        }

        

        

    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshRenderer.sharedMaterial.mainTexture  = texture;
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.GetComponent<MeshCollider>().sharedMesh = meshFilter.sharedMesh;
        meshRenderer.transform.localScale = new Vector3(mapWidth,mapHeight,mapHeight);
    }


    public Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);


        Color[] colormap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colormap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }
        return TextureFromColormap(colormap, width, height);
    }


    public Texture2D TextureFromColormap(Color[] colorMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width,height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

    public void DrawTexture(Texture2D texture)
    {
        meshRenderer.sharedMaterial.mainTexture = texture;
        meshRenderer.transform.localScale = new Vector3(texture.width, meshRenderer.transform.localScale.y, texture.height);
    }




    private void OnValidate()
    {
        if(mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1) { 
            mapHeight = 1;
        }
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 0)
        {
            octaves = 0;
        }
        mapSize = new Vector2Int(mapWidth, mapHeight);
        falloffMap = FallOffGenerator.GenerateFalloffMap(mapSize, falloffStart, falloffEnd);

    }

}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;

}