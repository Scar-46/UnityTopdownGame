using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();
    public float spawnRate = 5.0f;

    private float _x, _y;
    private Vector3 _spawnPosition;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }


    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        _x = Random.Range(-1, 1); // Spawn in diferent points
        _y = Random.Range(-1, 1);

        _spawnPosition = new Vector3(_x, _y, 0);

        Instantiate(Enemies[0], _spawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnEnemy());
    }
}
