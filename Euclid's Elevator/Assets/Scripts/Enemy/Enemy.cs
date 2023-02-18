using System.Collections;
using UnityEngine.AI;
using UnityEngine;

enum EnemyState
{
    Patrol,
    Attack,
    NoiseHeard,
}

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] LayerMask rayMask;

    [SerializeField] float noiseHearingDistance;

    [SerializeField] float patrolStepDistance;
    [SerializeField] float patrolStepTime;

    [SerializeField] float noiseHeardToPatrolTransitionTime;
    [SerializeField] float noiseHeardThreshold;

    [SerializeField] float attackStopDistance;
    [SerializeField] float attackRange;

    [SerializeField] float distancePerFootstep;
    [SerializeField] AudioSource footstepSource;

    NavMeshAgent agent;
    Transform player;
    EnemyState state;

    Vector3 destination;

    float walked;
    uint steps;

    bool patrolling;
    bool wasAttacking;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameManager.instance.player;
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hit, 100, rayMask) && hit.collider.CompareTag("Player"))
        {
            state = EnemyState.Attack;
            wasAttacking = true;
        }
        else
        {
            agent.stoppingDistance = 0;

            if (wasAttacking)
            {
                NoiseHeardNav(destination, true);
                wasAttacking = false;
            }
            else if (state != EnemyState.NoiseHeard)
            {
                state = EnemyState.Patrol;
            }
        }
        
        switch (state)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }

        /*walked += agent.velocity.magnitude * Time.deltaTime;

        float prev = steps;
        steps = (uint)Mathf.RoundToInt(walked / distancePerFootstep);

        if (steps > prev)
        {
            footstepSource.Play();
        }*/
    }

    private void OnValidate()
    {
        if (attackRange < attackStopDistance)
        {
            attackRange = attackStopDistance;
        }
    }

    void Patrol()
    {
        if (patrolling)
            return;

        do
        {
            destination = transform.position + NewDirection();
            agent.SetDestination(destination);
        }
        while (agent.pathStatus == NavMeshPathStatus.PathInvalid
            || agent.pathStatus == NavMeshPathStatus.PathPartial);

        if (!patrolling)
            StartCoroutine(PatrolStep(patrolStepTime));
    }

    Vector3 NewDirection()
    {
        return Vector3.Scale(Random.insideUnitSphere, new Vector3(1, 0, 1)).normalized * patrolStepDistance;
    }

    IEnumerator PatrolStep(float time)
    {
        patrolling = true;
        yield return new WaitForSeconds(time);
        patrolling = false;
    }

    void Attack()
    {
        agent.stoppingDistance = attackStopDistance;
        destination = player.position;
        agent.SetDestination(destination);

        if (Vector3.Distance(destination, transform.position) < attackRange 
            && GameManager.instance.player.TryGetComponent(out FpsController cont))
        {
            cont.Die(transform.position + (Vector3.up / 2));
            agent.isStopped = true;
        }
    }

    public void NoiseHeardNav(Vector3 noisePosition, bool overrideAttack = false)
    {
        if (!overrideAttack && state == EnemyState.Attack)
            return;
        else if (Vector3.Distance(noisePosition, transform.position) > noiseHearingDistance)
            return;

        StopAllCoroutines();
        patrolling = false;

        state = EnemyState.NoiseHeard;

        destination = noisePosition;
        agent.SetDestination(destination);

        StartCoroutine(NoiseHeard(destination, noiseHeardThreshold));
    }

    IEnumerator NoiseHeard(Vector3 position, float threshold)
    {
        yield return new WaitUntil(() => 
        { 
            return Vector3.Distance(position, transform.position) < threshold; 
        });

        yield return new WaitForSeconds(noiseHeardToPatrolTransitionTime);

        state = EnemyState.Patrol;
    }
}
