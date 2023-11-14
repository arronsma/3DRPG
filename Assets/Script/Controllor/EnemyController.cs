using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


enum EnemyStatus 
{   
    GURAD, // Õ¾×®¹Ö
    PATROL, // Ñ²Âß¹Ö
    CHASE, DEAD 
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
public class EnemyController : MonoBehaviour
{
    private EnemyStatus enemyStates;
    private NavMeshAgent agent;

    [Header("Basic Settings")]
    public float sightRadius;
    private GameObject attackTarget;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        SwitchStates();
    }

    private void SwitchStates()
    {
        if (FoundPlayer())
        {
            enemyStates = EnemyStatus.CHASE;
        }
        switch(enemyStates)
        {
            case EnemyStatus.GURAD:
                break;
            case EnemyStatus.PATROL:
                break;
            case EnemyStatus.CHASE:
                //TODO: chase player
                agent.destination = attackTarget.transform.position;
                //TODO: back to gurad or patrol if player far away
                //TODO: attack player
                //TODO: attack animator
                break;
            case EnemyStatus.DEAD:
                break;
        }
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }
}
