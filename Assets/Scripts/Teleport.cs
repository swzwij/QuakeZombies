using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TP")
        {
            var destination = other.GetComponent<TP>().GetDestination();
            characterController.enabled = false;
            transform.position = destination.position;
            characterController.enabled = true;
        }
    }
}
