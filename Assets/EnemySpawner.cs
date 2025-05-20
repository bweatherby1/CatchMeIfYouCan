using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnInterval = 3f;
    public float spawnRadius = 8f;

    // These define the rectangle area where enemies are allowed to spawn
    public Vector2 spawnBoundsMin = new Vector2(-15f, -15f); // bottom-left
    public Vector2 spawnBoundsMax = new Vector2(15f, 15f);   // top-right

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos;
        int attempts = 10;
        int tries = 0;

        do
        {
            Vector2 spawnDir = Random.insideUnitCircle.normalized;
            spawnPos = (Vector2)player.position + spawnDir * spawnRadius;
            tries++;
        }
        while (!IsInsideBounds(spawnPos) && tries < attempts);

        if (tries < attempts)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            EnemyFollow enemyScript = newEnemy.GetComponent<EnemyFollow>();
            enemyScript.player = player;
        }
    }

    bool IsInsideBounds(Vector2 position)
    {
        return position.x >= spawnBoundsMin.x && position.x <= spawnBoundsMax.x &&
               position.y >= spawnBoundsMin.y && position.y <= spawnBoundsMax.y;
    }
}
