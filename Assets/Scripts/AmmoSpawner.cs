using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject heavyAmmo;
    [SerializeField] private float refilTime;
    public bool isSpawned;
    private GameObject visual;

    private void Start()
    {
        isSpawned = true;
        visual = Instantiate(heavyAmmo, spawnPoint.position, Quaternion.identity);
    }

    public void Spawn() => StartCoroutine(SpawnDelay());

    private IEnumerator SpawnDelay()
    {
        Destroy(visual);
        isSpawned = false;
        yield return new WaitForSeconds(refilTime);
        isSpawned = true;
        visual = Instantiate(heavyAmmo, spawnPoint.position, Quaternion.identity);
    }
}
