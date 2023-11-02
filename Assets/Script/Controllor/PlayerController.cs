using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;


    private GameObject attackTarget;

    // cd is defined other way 
    private float lastAttackTime;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); 
        // you can't put it here
        // MouseManager.Instance.MouseEventClickGround += OnMouseClick;
    }

    private void Start()
    {
        MouseManager.Instance.MouseEventClickGround += OnMouseClickGround;
        MouseManager.Instance.MouseEventClickEnemy += OnMouseClickEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        lastAttackTime -= Time.fixedDeltaTime;
    }

    // move to destinatiion
    public void OnMouseClickGround(Vector3 target)
    {
        agent.destination = target;
    }

    // attack enemy after we click it
    private void OnMouseClickEnemy(GameObject obj)
    {
        if (obj != null)
        {
            attackTarget = obj;
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        transform.LookAt(attackTarget.transform);

        // move until close enough, attack distance hard coded to 1, and will get from weapon later.
        while (Vector3.Distance(transform.position, attackTarget.transform.position) > 1)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        // move 
        agent.isStopped = true;
        //CD is over
        if (lastAttackTime < 0)
        {

        }
    }
}
