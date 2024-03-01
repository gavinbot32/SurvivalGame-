using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FallOffGenerator
{
    public static float[,] GenerateFalloffMap(Vector2Int size, float falloffStart, float falloffEnd)
    {
        float[,] heightmap = new float[size.x, size.y];

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector2 position = new Vector2(
                    (float)x / size.x * 2 - 1,
                    (float)y / size.y * 2 - 1
                  );

                //find wihich value is closer to the edge;
                float t = Mathf.Max(Mathf.Abs(position.x), Mathf.Abs(position.y));

                if (t < falloffStart)
                {
                    heightmap[x, y] = 0;
                }
                else if (t > falloffEnd)
                {
                    heightmap[x, y] = 1;
                }
                else
                {
                    heightmap[x, y] = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(falloffStart, falloffEnd, t));

                }

            }
        }

        return heightmap;
    }
    static float Evaluate(float value)
    {
        float a = 3f;
        float b = 2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}



