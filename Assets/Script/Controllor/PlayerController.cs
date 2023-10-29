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

    public void OnMouseClickGround(Vector3 target)
    {
        agent.destination = target;
    }
    private void OnMouseClickEnemy(GameObject obj)
    {
        throw new NotImplementedException();
    }
}
