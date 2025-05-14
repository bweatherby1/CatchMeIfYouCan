using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnInterval = 3f;
    public float spawnRadius = 8f;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
{
    Vector2 spawnDir = Random.insideUnitCircle.normalized;
    Vector2 spawnPos = (Vector2)player.position + spawnDir * spawnRadius;

    GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

    EnemyFollow enemyScript = newEnemy.GetComponent<EnemyFollow>();
    enemyScript.player = player;
}

}
