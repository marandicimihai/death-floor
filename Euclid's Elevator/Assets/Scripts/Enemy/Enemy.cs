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
    [SerializeField] float openDoorTime;
    [SerializeField] float doorOpenDistance;

    [Header("Animation Properties")]
    [SerializeField] Animator animator;
    [SerializeField] int firstRun;
    [SerializeField] int lastRun;
    [SerializeField] int firstPose;
    [SerializeField] int lastPose;
    [SerializeField] float timeBetweenPoses;

    [Header("Sound")]
    [SerializeField] AudioSource drag;
    [SerializeField] float dragFadeTime; 

    public bool Visible;
    public bool CanKill;

    Transform player;
    CameraController camCon;
    EnemyState state;

    IEnumerator patrolCoroutine, inspectCoroutine, doorCoroutine;

    Vector3 destination;

    float dragVol;

    bool ambianceStarted, ambianceMid;
    bool fullyStopped;
    bool patrolStep;
    bool chasing;

    private void Start()
    {
        player = GameManager.Instance.player;
        camCon = player.GetComponent<CameraController>();
        GameManager.MakePausable(this);
        StartCoroutine(ChangePose());
        drag.Play();
    }

    private void Update()
    {
        if (CanKill && Vector3.Distance(player.position, transform.position) < chaseRange &&
            !GameManager.Instance.playerController.Dead)
        {
            CanKill = false;
            animator.SetInteger("State", -1);
            if (Visible)
            {
                animator.SetTrigger("Execute2");
            }
            else
            {
                animator.SetTrigger("Execute1");
            }
            GameManager.Instance.playerController.JumpscareDie();
        }

        if (agent.velocity.magnitude >= 0.1f)
        {
            dragVol += Time.deltaTime / dragFadeTime;
        }
        else
        {
            dragVol -= Time.deltaTime / dragFadeTime;
        }

        if (state == EnemyState.Patrol)
        {
            dragVol = 0;
        }

        dragVol = Mathf.Clamp01(dragVol);
        drag.volume = dragVol;

        CheckVisible();
        PathFindingLogic();
        DoorCheck();
    }

    void CheckVisible()
    {
        if ((!Physics.Raycast(camCon.Camera.position + Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f + Vector3.up * 0.49f, transform.position - camCon.Camera.position,
            Vector3.Distance(GameManager.Instance.player.position, transform.position), GameManager.Instance.playerController.settings.visionMask) ||
            !Physics.Raycast(camCon.Camera.position - Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f + Vector3.up * 0.49f, transform.position - camCon.Camera.position,
            Vector3.Distance(GameManager.Instance.player.position, transform.position), GameManager.Instance.playerController.settings.visionMask) ||
            !Physics.Raycast(camCon.Camera.position + Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f - Vector3.up * 0.49f, transform.position - camCon.Camera.position,
            Vector3.Distance(GameManager.Instance.player.position, transform.position), GameManager.Instance.playerController.settings.visionMask) ||
            !Physics.Raycast(camCon.Camera.position - Vector3.Cross(Vector3.up, transform.position - camCon.Camera.position).normalized * 0.24f - Vector3.up * 0.49f, transform.position - camCon.Camera.position,
            Vector3.Distance(GameManager.Instance.player.position, transform.position), GameManager.Instance.playerController.settings.visionMask)) &&
            camCon.Camera.TryGetComponent(out Camera camera) && TryGetComponent(out Collider col) &&
            GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), col.bounds))
        {
            Stop();
            Visible = true;
        }
        else
        {
            Continue();
            Visible = false;
            Quaternion rotation = Quaternion.LookRotation((GameManager.Instance.player.position - transform.position).normalized, Vector3.up);
            rig.rotation = rotation * Quaternion.Euler(0, -90, 0);
        }
    }

    void PathFindingLogic()
    {
        if ((Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hit, 100, rayMask) && 
            hit.collider.CompareTag("Player")) || Visible)
        {
            Chase();
        }
        else if (chasing)
        {
            chasing = false;
            NoiseHeardNav(player.position);
        }
        else if (state != EnemyState.Inspect)
        {
            if (!SoundManager.Instance.GetSound("MusicalStart").isPlaying && ambianceStarted)
            {
                SoundManager.Instance.StopSoundLoop("MusicalMid", () =>
                {
                    SoundManager.Instance.PlaySound("MusicalEnd");
                });
                ambianceStarted = false;
                ambianceMid = false;
            }
            Patrol();
        }
    }

    void DoorCheck()
    {
        if (state != EnemyState.Patrol && Physics.Raycast(transform.position, transform.forward, out RaycastHit doorHit, doorOpenDistance)
            && doorHit.transform.CompareTag("Door") && !agent.isStopped)
        {
            Door door1 = doorHit.transform.GetComponentInParent<Door>();
            if (door1.locked && doorCoroutine == null)
            {
                doorCoroutine = WaitAndExec(openDoorTime, () =>
                {
                    if (state != EnemyState.Patrol && Physics.Raycast(transform.position, transform.forward, out RaycastHit rayHit, doorOpenDistance)
                        && rayHit.transform.CompareTag("Door") && !agent.isStopped)
                    {
                        Door door = rayHit.transform.GetComponentInParent<Door>();
                        if (door != door1)
                        {
                            return;
                        }
                        door.ForceOpen();
                    }
                    doorCoroutine = null;
                });

                StartCoroutine(doorCoroutine);
            }
            else if (doorCoroutine == null)
            {
                door1.ForceOpen();
            }
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

        SoundManager.Instance.StopSound("MusicalEnd");
        if (!ambianceStarted)
        {
            SoundManager.Instance.PlaySound("MusicalStart");
            ambianceStarted = true;
        }
        else
        {
            if (!SoundManager.Instance.GetSound("MusicalStart").isPlaying && !ambianceMid)
            {
                SoundManager.Instance.PlaySound("MusicalMid");
                ambianceMid = true;
            }
        }
        
        chasing = true;
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
        patrolStep = false;

        destination = noisePosition + (noisePosition + transform.position).normalized * 2;
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

    public void Reset(Vector3 position, float time)
    {
        agent.Warp(position);
        agent.ResetPath();
        Stop(time);
        StartCoroutine(WaitAndExec(time, ()=>
        {
            agent.Warp(position);
            agent.ResetPath();
            state = EnemyState.Patrol;
            patrolStep = false;
            CanKill = true;
        }));
    }

    // The function, when called without specifying the time, will stop the agent, until it is let to continue;
    // When specifying the time, the function will not allow the enemy to move for the given time
    public void Stop(float time = 0)
    {
        patrolStep = false;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        if (patrolCoroutine != null)
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
        if (fullyStopped)
            return;

        agent.isStopped = false;
    }

    IEnumerator ChangePose()
    {
        yield return new WaitUntil(() => 
        {
            return !Visible;
        });

        if (GameManager.Instance.playerController.Dead)
        {
            StartCoroutine(ChangePose());
            yield break;
        }

        if (state == EnemyState.Chase || state == EnemyState.Inspect)
        {
            animator.SetInteger("State", UnityEngine.Random.Range(firstRun, lastRun + 1));
            yield return new WaitForSeconds(timeBetweenPoses);
            yield return new WaitUntil(() =>
            {
                return Visible || state == EnemyState.Patrol;
            });
            StartCoroutine(ChangePose());
            yield break;
        }
        else
        {
            animator.SetInteger("State", UnityEngine.Random.Range(firstPose, lastPose + 1));
            yield return new WaitForSeconds(timeBetweenPoses);
            yield return new WaitUntil(() =>
            {
                return Visible || state != EnemyState.Patrol;
            });
            StartCoroutine(ChangePose());
            yield break;
        }
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