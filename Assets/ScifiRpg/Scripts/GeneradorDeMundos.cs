using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDeMundos : MonoBehaviour
{

    public float HMmin = 0;
    public float HM = 40;
    public float divRangeMin = 6;
    public float divRange = 15;
    public Terrain terrain;

    // Start is called before the first frame update
    void OnValidate()
    {
        GenerateTerrain(terrain, HM);
    }
    public void GenerateTerrain(Terrain t, float tileSize)
    {
           
        //Heights For Our Hills/Mountains
        float[,] hts = new float[t.terrainData.heightmapWidth, t.terrainData.heightmapHeight];
        for (int i = 0; i < t.terrainData.heightmapWidth; i++)
        {
            for (int k = 0; k < t.terrainData.heightmapHeight; k++)
            {
                hts[i, k] = Mathf.PerlinNoise(((float)i / (float)t.terrainData.heightmapWidth) * tileSize, ((float)k / (float)t.terrainData.heightmapHeight) * tileSize)/ divRangeMin.RandomMinMax(divRange);
            }
        }
        t.terrainData.SetHeights(0, 0, hts);
    }
}