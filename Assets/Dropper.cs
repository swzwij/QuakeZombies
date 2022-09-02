using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] private GameObject AmmoDrop;
    [SerializeField] private int dropRate;

    public void Drop()
    {
        if(Random.Range(0, 100) <= dropRate)
        {
            Instantiate(AmmoDrop, transform.position, Quaternion.identity);
        }
    }
}
