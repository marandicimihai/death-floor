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
    [SerializeField] Transform player;
    [SerializeField] LayerMask rayMask;

    [SerializeField] float footstepHearingDistance;
    [SerializeField] float patrolStepDistance;
    [SerializeField] float patrolStepTime;

    [SerializeField] float noiseHeardToPatrolTransitionTime;
    [SerializeField] float noiseHeardThreshold;

    [SerializeField] float attackStopDistance;

    NavMeshAgent agent;
    EnemyState state;

    Vector3 destination;

    bool patrolling;
    bool wasAttacking;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        NoiseHeardNav(new Vector3(10, 1, 0));
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
    }

    void NoiseHeardNav(Vector3 noisePosition, bool overrideAttack = false)
    {
        if (!overrideAttack && state == EnemyState.Attack)
            return;

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
