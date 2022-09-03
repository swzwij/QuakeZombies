using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    private GameObject[] _spawnPoints;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnRate;

    private void Start()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("ENEMYSPAWNPOINT");
        StartCoroutine(SpawnTimer());
    }

    private void SpawnEnemy()
    {
        var r = Random.Range(0, _spawnPoints.Length);
        Instantiate(enemy, _spawnPoints[r].transform.position, Quaternion.identity);   
    }

    private IEnumerator SpawnTimer()
    {
        SpawnEnemy();
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnTimer());
    }
}
