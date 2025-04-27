using UnityEngine;

public class RoomLoot : MonoBehaviour
{
    public SpawnableObject[] spawnableObjects;

    [Header("Spawn Area Settings")]
    public Transform spawnArea;

    private EnemyRoomSpawner enemyRoomSpawner;

    private void Awake()
    {
        enemyRoomSpawner = GetComponent<EnemyRoomSpawner>();
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
            Vector2 spawnCenter = spawnArea != null ? (Vector2)spawnArea.position : (Vector2)transform.position;
            Vector2 spawnSize = spawnArea != null ? spawnArea.localScale : Vector2.zero;

            LootSpawner.SpawnLoot(spawnableObjects, spawnCenter, spawnSize);
            Destroy(gameObject);
        }
    }
}
