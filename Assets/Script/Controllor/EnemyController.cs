using UnityEngine;
using UnityEngine.AI;


enum EnemyStatus 
{   
    GURAD, // վ׮��
    PATROL, // Ѳ�߹�
    CHASE, DEAD 
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
public class EnemyController : MonoBehaviour
{
    private EnemyStatus enemyStates;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Basic Settings")]
    public float sightRadius;
    private GameObject attackTarget;

    // speed set in agent.speed, but patrol, chase and return from chase have different speed
    private float speed;
    // is GUARD or PATROL when no enemy
    public bool isGuard;
    // When the PATROL enemy reach its detination, it will stop for a while and walk to another point.
    public float lookAtTime;
    private float remainLookTime;


    [Header("Patrol State")]
    public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 centerPosition;

    // animator
    bool isWalk;

    // entry of Chase Layer
    bool isChase;
    // 
    bool isFollow;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator =  GetComponent<Animator>();
        speed = agent.speed;
        wayPoint = centerPosition = transform.position;
        remainLookTime = lookAtTime;
    }

    private void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStatus.GURAD;
        } 
        else
        {
            enemyStates = EnemyStatus.PATROL;
        }
    }
    private void Update()
    {
        SwitchStates();
        SwitchAnimation();
    }

    void SwitchAnimation()
    {
        animator.SetBool("Walk", isWalk);
        animator.SetBool("ChaseState", isChase);
        animator.SetBool("Follow", isFollow);
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
                isWalk = false;
                break;
            case EnemyStatus.PATROL:
                // this is error, when enemy reach wayPoint, it will stop walking
                // isWalk = true;
                isChase = false;
                isFollow = false;

                if (Vector3.Distance(transform.position, wayPoint) <= agent.stoppingDistance )
                {
                    if (remainLookTime > 0)
                    {
                        remainLookTime -= Time.deltaTime;
                    } 
                    else
                    {
                        wayPoint = GetNewWayPoint();
                    }
                    
                    agent.destination = wayPoint;
                    agent.speed = speed * 0.5f;
                    isWalk = false;
                }
                else
                {
                    isWalk = true;
                }
                break;
            case EnemyStatus.CHASE:

                //TODO: back to gurad or patrol if player far away
                //TODO: attack player
                //TODO: attack animator
                isWalk = false;
                isChase = true;
                if (FoundPlayer())
                {
                    //TODO: chase player
                    agent.destination = attackTarget.transform.position;
                    agent.speed = speed;
                    isFollow = true;
                } 
                else
                {
                    isFollow = false;
                    // stop at the current when lost target
                    agent.destination = transform.position;
                }
                break;
            case EnemyStatus.DEAD:
                break;
        }
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        Debug.Log(colliders.Length);
        for (int i = 0; i < colliders.Length; i++)
        {
            var target = colliders[i];
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    Vector3 GetNewWayPoint ()
    {
        remainLookTime = lookAtTime;

        Vector3 wayPoint = new Vector3(centerPosition.x + Random.Range(-patrolRange, patrolRange), centerPosition.y, centerPosition.z + Random.Range(-patrolRange, patrolRange));
        NavMeshHit hit;
        // sample the nearnet reachable point for enemy to go.
        // if can not find any point, SamplePosition will retturn false and enemy will stand there for next sample next frame.
        wayPoint = NavMesh.SamplePosition(wayPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
        return wayPoint;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        Vector3 size = new Vector3(patrolRange, patrolRange, patrolRange);
        Gizmos.DrawWireCube(centerPosition, size);
    }
}
