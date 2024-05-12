using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 10f;
    public void KickOff()
    {
        Vector3 direction = (attackTarget.transform.position - transform.position).normalized;

        attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
        attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
        attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

        attackTarget.GetComponent<CharacterStats>().TakeDagame(characterStats);
    }
}
