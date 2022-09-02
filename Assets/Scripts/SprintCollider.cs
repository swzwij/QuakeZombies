using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintCollider : MonoBehaviour
{
    [Range(0,1)]
    [SerializeField] private int startOrEnd;

    private Sprint sprint;

    private void Start()
    {
        sprint = GetComponentInParent<Sprint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("B");
        if (startOrEnd == 0) sprint.StartTimer();
        if (startOrEnd == 1) sprint.EndTimer();
    }
}
