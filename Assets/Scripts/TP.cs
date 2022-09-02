using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP : MonoBehaviour
{
    [SerializeField] private Transform exit;
    public Transform GetDestination()
    {
        return exit;
    }
}
