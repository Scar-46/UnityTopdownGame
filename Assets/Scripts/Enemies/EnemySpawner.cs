using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();
    public float spawnRate = 5.0f;
    public int spawnMax;
    public Tilemap floorTilemap = null;

    private int spawnCounter = 0;

    private Vector2 _spawnPosition;

    private void Awake()
    {
        floorTilemap = transform.parent.parent.Find("Floor")?.gameObject.GetComponent<Tilemap>();
    }

    public int StartSpawn()
    {
        for (int i = 0; i < spawnMax; i++)
        {
            _spawnPosition = this.transform.position;

            int enemyIndex = Random.Range(0, Enemies.Count);
            GameObject enemy = Instantiate(Enemies[enemyIndex], _spawnPosition, Quaternion.identity);

            EnemySpawner children = enemy.GetComponent<EnemySpawner>();

            if (children != null)
            {
                children.floorTilemap = floorTilemap;
            }
            RoamingState roamingState = enemy.GetComponent<RoamingState>();
            if (roamingState != null)
            {
                roamingState.tilemap = floorTilemap;
            }
        }
        return spawnMax;
    }

    public void Spawn()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        _spawnPosition = this.transform.position;

        int enemyIndex = Random.Range(0, Enemies.Count);
        GameObject enemy = Instantiate(Enemies[enemyIndex], _spawnPosition, Quaternion.identity);

        EnemySpawner children = enemy.GetComponent<EnemySpawner>();
        if (children != null)
        {
            children.floorTilemap = floorTilemap;
        }

        RoamingState roamingState = enemy.GetComponent<RoamingState>();
        if (roamingState != null)
        {
            roamingState.tilemap = floorTilemap;
        }
        spawnCounter++;

        yield return new WaitForSeconds(spawnRate);

        if (spawnCounter < spawnMax) {
            StartCoroutine(SpawnEnemy());
        }
    }
}
