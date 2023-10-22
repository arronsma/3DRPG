using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); 
        // you can't put it here
        // MouseManager.Instance.MouseEventClick += OnMouseClick;
    }

    private void Start()
    {
        MouseManager.Instance.MouseEventClick += OnMouseClick;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    public void OnMouseClick(Vector3 target)
    {
        agent.destination = target;
    }
}
