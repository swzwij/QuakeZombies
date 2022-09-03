using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] private GameObject AmmoDrop;
    [SerializeField] private int dropRate;

    private EnemyAI enemyAI;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    public void Drop()
    {
        enemyAI._target.GetComponent<Wallet>().AddCredits(50);

        if(Random.Range(0, 100) <= dropRate)
        {
            Instantiate(AmmoDrop, transform.position, Quaternion.identity);
        }
    }
}
