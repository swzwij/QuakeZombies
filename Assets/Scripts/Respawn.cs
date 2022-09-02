using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject player;

    private CharacterController characterController;
    private GameObject[] spawnPoints;

    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SPAWNPOINT");
        characterController = player.GetComponent<CharacterController>();      
    }

    public void RespawnPlayer()
    {
        print("Respawn Player");
        var a = Random.Range(0, spawnPoints.Length);
        characterController.enabled = false;
        player.transform.position = spawnPoints[a].transform.position;
        characterController.enabled = true;
    }
}
