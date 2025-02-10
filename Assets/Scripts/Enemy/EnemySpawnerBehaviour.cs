using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawnerBehaviour : MonoBehaviour
{
    [SerializeField] private List<Transform> possibleSpawns;
    [SerializeField] private float spawnTimeInterval;
    [SerializeField] private GameObject enemyToSpawn;

    [Header("At what wave this spawner starts to work?")]
    public int waveNumber;

    [Header("Cost to instantiate the unit")]
    [SerializeField] private int spawnCost;

    private bool spawn;

    private void Start()
    {
        spawn = true;
        StartCoroutine(SpawnEnemies());
    }

    private void OnEnable()
    {
        EventManager.StopAllSpawns += StopSpawn;
        EventManager.StopSpawn += StopSpawn;
    }

    private void OnDisable()
    {
        EventManager.StopAllSpawns -= StopSpawn;
        EventManager.StopSpawn -= StopSpawn;
    }

    private void StopSpawn(string typeOfEnemy)
    {
        if (typeOfEnemy == enemyToSpawn.name)
        {
            spawn = false;
        }
    }

    private void StopSpawn()
    {
        spawn = false;
    }

    private IEnumerator SpawnEnemies()
    {
        while (spawn)
        {
            if (GameManager.Instance.CurrentWave >= waveNumber)
            {
                var cost = GameManager.Instance.CurrentSpawnCurrency - spawnCost;

                if (cost >= 0)
                {
                    var go = Instantiate(enemyToSpawn, possibleSpawns[Random.Range(0, possibleSpawns.Count)].position, possibleSpawns[Random.Range(0, possibleSpawns.Count)].rotation);
                    go.GetComponent<Enemy>().UnitCost = spawnCost;
                    GameManager.Instance.CurrentSpawnCurrency = Mathf.Clamp(cost, 0, GameManager.Instance.CurrentSpawnCurrency);
                }
            }
            yield return new WaitForSeconds(spawnTimeInterval);
        }
    }
}
