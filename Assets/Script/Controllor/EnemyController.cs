using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyStatus 
{   
    GURAD, // Õ¾×®¹Ö
    PATROL, // Ñ²Âß¹Ö
    CHASE, DEAD 
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
public class EnemyController : MonoBehaviour
{
    public EnemyStatus enemyStates;
    private NavMeshAgent agent;

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
        switch(enemyStates)
        {
            case EnemyStatus.GURAD:
                break;
            case EnemyStatus.PATROL:
                break;
            case EnemyStatus.CHASE:
                break;
            case EnemyStatus.DEAD:
                break;
        }
    }
}
