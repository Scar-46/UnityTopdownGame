using UnityEngine;
using System;

public class RoomLoot : MonoBehaviour
{
    [Serializable]
    public class SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float spawnProbability = 0.5f;
    }

    public SpawnableObject[] spawnableObjects;

    [Header("Spawn Area Settings")]
    public Transform spawnArea;
    public EnemyRoomSpawner enemyRoomSpawner;

    private void Awake()
    {
        enemyRoomSpawner = this.GetComponent<EnemyRoomSpawner>();    
    }

    private void OnEnable()
    {
        EnemyRoomSpawner.OnRoomClean += TrySpawnObjects;
    }

    private void OnDisable()
    {
        EnemyRoomSpawner.OnRoomClean -= TrySpawnObjects;
    }

    private void TrySpawnObjects()
    {
        if (enemyRoomSpawner.waves <= 0 && enemyRoomSpawner._enemiesAlive <= 0)
        {
            foreach (var obj in spawnableObjects)
            {
                if (UnityEngine.Random.value <= obj.spawnProbability)
                {
                    Vector2 spawnPosition = GetRandomPositionInArea();
                    Instantiate(obj.prefab, spawnPosition, Quaternion.identity);
                }
            }
        } 
    }
    private Vector2 GetRandomPositionInArea()
    {
        if (spawnArea == null)
        {
            Debug.LogWarning("Spawn Area is not assigned!");
            return Vector2.zero;
        }

        Vector2 center = spawnArea.position;
        Vector2 size = spawnArea.localScale;

        float randomX = UnityEngine.Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float randomY = UnityEngine.Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        return new Vector2(randomX, randomY);
    }


}
