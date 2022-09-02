using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpin : MonoBehaviour
{
    [SerializeField] private float speed;

    void FixedUpdate() => transform.Rotate(0, speed, 0);
}
