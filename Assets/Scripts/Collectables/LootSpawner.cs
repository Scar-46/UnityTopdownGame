using System;
using UnityEngine;

[Serializable]
public class SpawnableObject
{
    public GameObject prefab;
    [Range(0f, 1f)] public float spawnProbability = 0.5f;
}

public static class LootSpawner
{
    public static void SpawnLoot(SpawnableObject[] lootTable, Vector2 basePosition, Vector2? areaSize = null)
    {
        foreach (var obj in lootTable)
        {
            if (UnityEngine.Random.value <= obj.spawnProbability)
            {
                Vector2 spawnPosition = basePosition;

                if (areaSize.HasValue)
                {
                    float randomX = UnityEngine.Random.Range(-areaSize.Value.x / 2, areaSize.Value.x / 2);
                    float randomY = UnityEngine.Random.Range(-areaSize.Value.y / 2, areaSize.Value.y / 2);
                    spawnPosition += new Vector2(randomX, randomY);
                }

                UnityEngine.Object.Instantiate(obj.prefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
