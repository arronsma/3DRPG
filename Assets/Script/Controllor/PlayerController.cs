using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    private CharacterStats characterStats;
    private GameObject attackTarget;

    bool isDead;

    //cd is defined other way 
    private float lastAttackTime = 0;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); 
        characterStats = GetComponent<CharacterStats>();

        lastAttackTime = characterStats.attackData.coolDown;
        // you can't put it here
        // MouseManager.Instance.MouseEventClickGround += OnMouseClick;
    }

    private void Start()
    {
        MouseManager.Instance.MouseEventClickGround += OnMouseClickGround;
        MouseManager.Instance.MouseEventClickEnemy += OnMouseClickEnemy;

        GameManager.Instance.RegisterPlayer(this.characterStats);
    }

    // Update is called once per frame
    void Update()
    {
        isDead = characterStats.CurrentHealth <= 0;
        // Notify when Player is dead, Enemy win
        if (isDead)
        {
            GameManager.Instance.Notify();
        }
        SwitchAnimation();
        lastAttackTime -= Time.fixedDeltaTime;
    }

    void SwitchAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        animator.SetBool("Die", isDead);
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
        while (Vector3.Distance(transform.position, attackTarget.transform.position) > characterStats.attackData.attackRange)
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
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            animator.SetTrigger("Attack");
            animator.SetBool("Critical", characterStats.isCritical);
            //reset cd ,here cd will from weapon later
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }

    // animator event
    void Hit()
    {
        var targetStats = attackTarget.GetComponent<CharacterStats>();
        targetStats.TakeDagame(this.characterStats);
    }
}
