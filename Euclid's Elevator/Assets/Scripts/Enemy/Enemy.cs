using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using System;

enum EnemyState
{
    Patrol,
    Inspect,
    Chase,
}

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] LayerMask rayMask;

    [Header("Patrol")]
    [SerializeField] float patrolSpeed;
    [SerializeField] float patrolStepDistance;
    [SerializeField] float patrolStepTime;
    [Header("Inspect")]
    [SerializeField] float inspectSpeed;
    [SerializeField] float inspectDistance;
    [SerializeField] float inspectToPatrolTransitionTime;
    [SerializeField] float inspectThreshold;
    [Header("Chase")]
    [SerializeField] float chaseSpeed;
    [SerializeField] float chaseVisualContactTime;
    [SerializeField] float chaseStopDistance;
    [SerializeField] float chaseRange;

    Transform player;
    EnemyState state;

    IEnumerator patrolCoroutine, inspectCoroutine;

    Vector3 destination;

    float eyeContact;

    bool paralized;
    bool patrolStep;
    bool chasing;

    private void Start()
    {
        player = GameManager.instance.player;
    }

    private void Update()
    {
        if (!GameManager.instance.elevatorOpen)
        {
            state = EnemyState.Patrol;
            chasing = false;
            return;
        }

        if (Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hit, 100, rayMask) && hit.collider.CompareTag("Player"))
        {
            if (eyeContact < chaseVisualContactTime)
            {
                eyeContact += Time.deltaTime;
                NoiseHeardNav(player.position);
            }
            else
            {
                Chase();
            }
        }
        else if (chasing)
        {
            NoiseHeardNav(player.position, true);
            eyeContact = 0;
        }
        else if (state != EnemyState.Inspect)
        {
            Patrol();
        }
    }

    private void OnValidate()
    {
        if (chaseRange < chaseStopDistance)
        {
            chaseRange = chaseStopDistance;
        }
    }

    #region PATROL

    void Patrol()
    {
        if (patrolStep)
            return;

        agent.speed = patrolSpeed;

        do
        {
            destination = transform.position + NewDirection();
            agent.SetDestination(destination);
        }
        while (agent.pathStatus == NavMeshPathStatus.PathInvalid);

        state = EnemyState.Patrol;

        patrolCoroutine = PatrolStep(patrolStepTime);
        StartCoroutine(patrolCoroutine);
    }

    Vector3 NewDirection()
    {
        return Vector3.Scale(UnityEngine.Random.insideUnitSphere, new Vector3(1, 0, 1)).normalized * patrolStepDistance;
    }

    IEnumerator PatrolStep(float time)
    {
        patrolStep = true;
        yield return new WaitForSeconds(time);
        patrolStep = false;
    }

    #endregion

    void Chase()
    {
        state = EnemyState.Chase;

        agent.speed = chaseSpeed;
        agent.stoppingDistance = chaseStopDistance;

        destination = player.position;
        agent.SetDestination(destination);

        if (!chasing && patrolCoroutine != null)
            StopCoroutine(patrolCoroutine);

        chasing = true;

        if (Vector3.Distance(destination, transform.position) < chaseRange)
        {
            GameManager.instance.playerController.Die(transform.position + (Vector3.up / 2));
        }
    }

    #region INSPECT

    public void NoiseHeardNav(Vector3 noisePosition, bool overrideAttack = false)
    {
        if ((!overrideAttack && state == EnemyState.Chase) || Vector3.Distance(noisePosition, transform.position) > inspectDistance)
        {
            return;
        }

        chasing = false;

        if (patrolCoroutine != null)
            StopCoroutine(patrolCoroutine);
        if (inspectCoroutine != null)
            StopCoroutine(inspectCoroutine);

        state = EnemyState.Inspect;
        agent.speed = inspectSpeed;
        patrolStep = false;

        destination = noisePosition;
        agent.SetDestination(destination);

        bool partial;

        if (agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid)
            partial = true;
        else
            partial = false;

        inspectCoroutine = NoiseHeard(destination, inspectThreshold, partial);
        StartCoroutine(inspectCoroutine);
    }

    IEnumerator NoiseHeard(Vector3 position, float threshold, bool partial)
    {
        if (!partial)
        {
            yield return new WaitUntil(() =>
            {
                return Vector3.Distance(position, transform.position) < threshold;
            });
        }
        yield return new WaitForSeconds(inspectToPatrolTransitionTime);

        state = EnemyState.Patrol;
    }

    #endregion

    public void Respawn(Vector3 position)
    {
        agent.Warp(position);
        agent.ResetPath();
        state = EnemyState.Patrol;
        agent.velocity = Vector3.zero;
        agent.isStopped = false;
    }

    // The function, when called without specifying the time, will stop the agent, until it is let to continue;
    // When specifying the time, the function will not allow the enemy to move for the given time
    public void Stop(float time = 0)
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        if (time != 0)
        {
            paralized = true;
            StartCoroutine(WaitAndExec(time, () =>
            {
                paralized = false;
                Continue();
            }));
        }
    }

    public void Continue()
    {
        if (paralized)
            return;

        agent.isStopped = false;
    }

    IEnumerator WaitAndExec(float time, Action exec, bool repeat = false)
    {
        yield return new WaitForSeconds(time);
        exec?.Invoke();

        if (repeat)
        {
            StartCoroutine(WaitAndExec(time, exec, repeat));
        }
    }
}
