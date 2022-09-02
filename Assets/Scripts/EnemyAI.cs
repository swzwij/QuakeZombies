using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float killDistance = .5f;
    [SerializeField] private float distanceAway;

    private Transform thisOjb;
    private Transform _target;

    private NavMeshAgent _nav;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _nav = this.gameObject.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float dist = Vector3.Distance(_target.position, transform.position);

        //print(dist);

        if(_target)
        {
            _nav.SetDestination(_target.position);
        }

        if(dist <= killDistance)
        {
            _target.GetComponent<HealthSystem>().TakeDamage(.2f);
        }
    }
}
