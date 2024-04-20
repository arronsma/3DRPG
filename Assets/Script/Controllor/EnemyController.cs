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
    private Quaternion initialOrientation;

    // animator
    bool isWalk;
    // entry of Chase Layer,meaning enemy in Chase State
    bool isChase;
    // In Chase State, isFollow = true means the enemy is truly chase player.
    // isFollow = false means the enemy lost player but still in Chase State
    // and it is going to enter other state. Maybe it is unwilling to return, and stand there for a moment
    bool isFollow;

    private CharacterStats characterStats;

    float lastAttackTime = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator =  GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        speed = agent.speed;
        wayPoint = centerPosition = transform.position;
        remainLookTime = lookAtTime;
        initialOrientation = transform.rotation;
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
        // lastAttackTime should be update here, since every behavior will reduce CD
        lastAttackTime -= Time.deltaTime;
    }

    void SwitchAnimation()
    {
        animator.SetBool("Walk", isWalk);
        animator.SetBool("ChaseState", isChase);
        animator.SetBool("Follow", isFollow);
        animator.SetBool("Critical", characterStats.isCritical);
    }

    private void SwitchStates()
    {
        if (FoundPlayer())
        {
            enemyStates = EnemyStatus.CHASE;
        }
        
        switch (enemyStates)
        {
            case EnemyStatus.GURAD:
                isChase = false;
                if (Vector3.SqrMagnitude(transform.position - centerPosition) > agent.stoppingDistance)
                {
                    agent.isStopped = false;
                    isWalk = true;
                    agent.destination = centerPosition;
                } else
                {
                    isWalk = false;
                    transform.rotation = Quaternion.Lerp(transform.rotation, initialOrientation, 0.01f);
                }
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
                    isWalk = false;
                }
                else
                {
                    // move the last to line from if block to else block
                    // since PATROL can return from CHASE, if enemy lost player in CHASE state, 
                    // agent.destinaion is the position of the enemy at the moment
                    // when it loses the player's vision.
                    agent.destination = wayPoint;
                    agent.speed = speed * 0.5f;
                    isWalk = true;
                }
                break;
            case EnemyStatus.CHASE:

                //TODO: back to gurad or patrol if player far away
                //TODO: attack player
                //TODO: attack animator
                isWalk = false;
                isChase = true;
                agent.isStopped = false;
                if (FoundPlayer())
                {
                    //chase player
                    agent.destination = attackTarget.transform.position;
                    agent.speed = speed;
                    isFollow = true;
                    
                } 
                else
                {
                    isFollow = false;
                    // when lost target, the enemy will stand at the current point for lookAtTime
                    // Then return to patrol or guard status.
                    if (remainLookTime > 0)
                    {
                        remainLookTime -= Time.deltaTime;
                    }
                    else if (isGuard)
                    {
                        enemyStates = EnemyStatus.GURAD;
                    } 
                    else
                    {
                        enemyStates = EnemyStatus.PATROL;
                    } 
                    // stop at the current when lost target
                    agent.destination = transform.position;
                }

                if (TargetInAttachRange() || TargetInSkillRange() )
                {
                    // If the character is within the enemy's attack range, the enemy should stop moving and attack.
                    // isFollow controls the movement animation,
                    // agent.isStopped controls the movement of game objects.
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime <= 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;

                        // judge if critical hit
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
                        // perform Attack
                        Attack();

                    }
                }
                break;
            case EnemyStatus.DEAD:
                break;
        }
    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform.position);
        if (TargetInAttachRange())
        {
            // attack animator
            animator.SetTrigger("Attack");
        } 

        if (TargetInSkillRange())
        {
            // skill animator
        }
    }
    bool TargetInAttachRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        return false;
    }

    bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        return false;
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
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

    public void Hit()
    {
        if (attackTarget != null)
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDagame(this.characterStats);
        }
    }
}
