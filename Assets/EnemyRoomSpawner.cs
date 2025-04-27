using System;
using System.Collections;
using UnityEngine;

public class EnemyRoomSpawner : MonoBehaviour
{
    public int waves = 0;

    [SerializeField]
    private GameObject[] spawns;

    public int _enemiesAlive = 0;
    private bool _isSpawning = false;

    [SerializeField]
    private float delayBetweenWaves = 0;

    public static event Action? OnRoomClean;

    void Start()
    {
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
        StartNextWave();
    }

    private void OnDestroy()
    {
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void StartNextWave()
    {
        if (waves > 0)
        {
            waves--;
            foreach (var spawn in spawns)
            {
                _enemiesAlive += spawn.GetComponent<EnemySpawner>().StartSpawn();
            }
        }
    }

    private void HandleEnemyDeath()
    {
        _enemiesAlive--;
        if (_enemiesAlive <= 0 && !_isSpawning && (waves > 0))
        {
            StartCoroutine(SpawnNextWaveWithDelay());
        }
        else if ((waves <= 0) && (_enemiesAlive <= 0)){
            OnRoomClean?.Invoke();
        }
    }

    private IEnumerator SpawnNextWaveWithDelay()
    {
        _isSpawning = true;

        foreach (var spawn in spawns)
        {
            spawn.GetComponent<Animator>().SetTrigger("Spawn");
        }

        yield return new WaitForSeconds(delayBetweenWaves);
        StartNextWave();
        _isSpawning = false;
    }
}
