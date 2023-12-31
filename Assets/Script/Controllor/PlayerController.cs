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

    //TODO: cd is defined other way 
    private float lastAttackTime = 0.5f;
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
        StopAllCoroutines();
        agent.isStopped = false;
        agent.destination = target;
    }

    // attack enemy after we click it
    private void OnMouseClickEnemy(GameObject obj)
    {
        if (obj != null)
        {
            attackTarget = obj;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        

        // TODO: move until close enough, attack distance hard coded to 1, and will get from weapon later.
        while (Vector3.Distance(transform.position, attackTarget.transform.position) > 1)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        transform.LookAt(attackTarget.transform);
        // move 
        agent.isStopped = true;
        //CD is over
        if (lastAttackTime < 0)
        {
            animator.SetTrigger("attack");

            //TODO: reset cd ,here cd will from weapon later
            lastAttackTime = 0.5f;
        }
    }
}
