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
    [SerializeField] Transform rig;

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
    [SerializeField] float chaseStopDistance;
    [SerializeField] float chaseRange;
    [SerializeField] float doorOpenDistance;

    [Header("Animation Properties")]
    [SerializeField] Animator animator;
    [SerializeField] int firstRun;
    [SerializeField] int lastRun;
    [SerializeField] int firstPose;
    [SerializeField] int lastPose;
    [SerializeField] float timeBetweenPoses;

    [System.NonSerialized] public bool visibleToPlayer;

    Transform player;
    CameraController camCon;
    EnemyState state;

    IEnumerator patrolCoroutine, inspectCoroutine;

    Vector3 destination;

    bool fullyStopped;
    bool patrolStep;
    bool chasing;

    private void Start()
    {
        player = GameManager.instance.player;
        camCon = player.GetComponent<CameraController>();
        StartCoroutine(ChangePose());
    }

    private void Update()
    {
        if ((!Physics.Raycast(camCon.Camera.position + Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f + Vector3.up * 0.49f, transform.position - camCon.Camera.position,
            Vector3.Distance(GameManager.instance.player.position, transform.position), GameManager.instance.playerController.settings.visionMask) ||
            !Physics.Raycast(camCon.Camera.position - Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f + Vector3.up * 0.49f, transform.position - camCon.Camera.position,
            Vector3.Distance(GameManager.instance.player.position, transform.position), GameManager.instance.playerController.settings.visionMask) ||
            !Physics.Raycast(camCon.Camera.position + Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f - Vector3.up * 0.49f, transform.position - camCon.Camera.position,
            Vector3.Distance(GameManager.instance.player.position, transform.position), GameManager.instance.playerController.settings.visionMask) ||
            !Physics.Raycast(camCon.Camera.position - Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f - Vector3.up * 0.49f, transform.position - camCon.Camera.position,
            Vector3.Distance(GameManager.instance.player.position, transform.position), GameManager.instance.playerController.settings.visionMask)) &&
            camCon.Camera.TryGetComponent(out Camera camera) && TryGetComponent(out Collider col) &&
            GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), col.bounds))
        {
            Stop();
            visibleToPlayer = true;
        }
        else
        {
            Continue();
            visibleToPlayer = false;
        }

        Debug.DrawRay(camCon.Camera.position + Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f + Vector3.up * 0.49f, transform.position - camCon.Camera.position);
        Debug.DrawRay(camCon.Camera.position - Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f + Vector3.up * 0.49f, transform.position - camCon.Camera.position);
        Debug.DrawRay(camCon.Camera.position + Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f - Vector3.up * 0.49f, transform.position - camCon.Camera.position);
        Debug.DrawRay(camCon.Camera.position - Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f - Vector3.up * 0.49f, transform.position - camCon.Camera.position);

        Quaternion rotation = Quaternion.LookRotation((GameManager.instance.player.position - transform.position).normalized, Vector3.up);
        rig.rotation = rotation * Quaternion.Euler(0, -90, 0);

        if (Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hit, 100, rayMask) && hit.collider.CompareTag("Player") || visibleToPlayer)
        {
            Chase();
        }
        else if (chasing)
        {
            chasing = false;
            patrolStep = false;
            state = EnemyState.Inspect;
            NoiseHeardNav(player.position);
        }
        else if (state != EnemyState.Inspect)
        {
            Patrol();
        }

        if (state != EnemyState.Patrol && Physics.Raycast(transform.position, transform.forward, out RaycastHit doorHit, doorOpenDistance)
            && doorHit.transform.CompareTag("Door"))
        {
            Door door = doorHit.transform.GetComponentInParent<Door>();
            door.ForceOpen();
        }
    }

    private void OnValidate()
    {
        if (chaseRange < chaseStopDistance)
        {
            chaseRange = chaseStopDistance;
        }
    }

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

    void Chase()
    {
        state = EnemyState.Chase;

        agent.speed = chaseSpeed;
        agent.stoppingDistance = chaseStopDistance;

        destination = player.position;
        agent.SetDestination(destination);

        if (!chasing && patrolCoroutine != null)
            StopCoroutine(patrolCoroutine);
        if (!chasing && inspectCoroutine != null)
            StopCoroutine(inspectCoroutine);

        chasing = true;

        if (Vector3.Distance(destination, transform.position) < chaseRange)
        {
            GameManager.instance.playerController.Die(transform.position + (Vector3.up / 2));
        }
    }

    public void NoiseHeardNav(Vector3 noisePosition)
    {
        if (Vector3.Distance(noisePosition, transform.position) > inspectDistance)
        {
            return;
        }

        if (patrolCoroutine != null)
            StopCoroutine(patrolCoroutine);
        if (inspectCoroutine != null)
            StopCoroutine(inspectCoroutine);

        state = EnemyState.Inspect;
        agent.speed = inspectSpeed;

        destination = noisePosition;
        agent.SetDestination(destination);

        inspectCoroutine = NoiseHeard(destination, inspectThreshold);
        StartCoroutine(inspectCoroutine);
    }

    IEnumerator NoiseHeard(Vector3 position, float threshold)
    {
        yield return new WaitUntil(() =>
        {
            return Vector3.Distance(position, transform.position) < threshold;
        });

        patrolStep = false;
        state = EnemyState.Patrol;
    }

    public void Spawn(Vector3 position)
    {
        agent.Warp(position);
        agent.ResetPath();
        state = EnemyState.Patrol;
        patrolStep = false;
        agent.velocity = Vector3.zero;
    }

    // The function, when called without specifying the time, will stop the agent, until it is let to continue;
    // When specifying the time, the function will not allow the enemy to move for the given time
    public void Stop(float time = 0)
    {
        patrolStep = false;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        StopCoroutine(patrolCoroutine);
        if (time != 0)
        {
            fullyStopped = true;
            StartCoroutine(WaitAndExec(time, () =>
            {
                fullyStopped = false;
                Continue();
            }));
        }
    }

    public void Continue()
    {
        if (fullyStopped || !GameManager.instance.ElevatorOpen)
            return;

        agent.isStopped = false;
    }

    IEnumerator ChangePose()
    {
        yield return new WaitUntil(() => 
        {
            return !visibleToPlayer;
        });
        if (state == EnemyState.Chase || state == EnemyState.Inspect)
        {
            animator.SetInteger("State", UnityEngine.Random.Range(firstRun, lastRun + 1));
        }
        else
        {
            animator.SetInteger("State", UnityEngine.Random.Range(firstPose, lastPose + 1));
            yield return new WaitUntil(() =>
            {
                return state != EnemyState.Patrol;
            });
            StartCoroutine(ChangePose());
            yield break;
        }
        yield return new WaitForSeconds(timeBetweenPoses);
        yield return new WaitUntil(() =>
        {
            return visibleToPlayer;
        });
        StartCoroutine(ChangePose());
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
