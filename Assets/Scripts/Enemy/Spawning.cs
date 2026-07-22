using UnityEngine;
using System.Collections;

public class Spawning : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxEnemies = 5;
    
    private bool isSpawning = true;
    private int currentEnemyCount = 0;

    private void Start()
    {
         if (spawnPoints.Length > 0 && enemyPrefab != null)
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
            else
            {
                StopSpawning();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        currentEnemyCount++;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }
}
