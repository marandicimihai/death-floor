using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public enum State
{
    Patrol,
    Inspect,
    Chase
}

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavigation : MonoBehaviour
{
    public bool Visible
    {
        get
        {
            Transform cam = player.cameraController.camera;
            return (!Physics.Raycast(cam.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f, transform.position - cam.position,
            Vector3.Distance(player.transform.position, transform.position), solid) ||
            !Physics.Raycast(cam.position - Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f, transform.position - cam.position,
            Vector3.Distance(player.transform.position, transform.position), solid) ||
            !Physics.Raycast(cam.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f - Vector3.up * 0.49f, transform.position - cam.position,
            Vector3.Distance(player.transform.position, transform.position), solid) ||
            !Physics.Raycast(cam.position - Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f - Vector3.up * 0.49f, transform.position - cam.position,
            Vector3.Distance(player.transform.position, transform.position), solid) ||
            !Physics.Raycast(transform.position, player.transform.position - transform.position, Vector3.Distance(player.transform.position, transform.position), solid)) &&
            cam.TryGetComponent(out Camera camera) && TryGetComponent(out Collider col) &&
            GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), col.bounds);
        }
    }
    [SerializeField] Player player;
    [SerializeField] EnemyRigAnim rigAnim;
    [SerializeField] LayerMask solid;
    [SerializeField] NavMeshAgent agent;

    [Header("Patrol")]
    [SerializeField] float doorOpenCooldownTime;
    [SerializeField] float maxPatrolStepTime;
    [SerializeField] float patrolStep; 
    [SerializeField] float patrolThreshold;
    [SerializeField] float patrolSpeed;
    [SerializeField] float patrolStopDistance;

    [Header("Inspect")]
    [SerializeField] float inspectDistance;
    [SerializeField] float inspectThreshold;
    [SerializeField] float maxInspectTime;
    [SerializeField] float inspectSpeed;
    [SerializeField] float inspectStopDistance;

    [Header("Chase")]
    [SerializeField] float killDistance;
    [SerializeField] float chaseSpeed;
    [SerializeField] float chaseStopDistance;

    [Header("Other")]
    [SerializeField] float spawnFreezeTime;
    [SerializeField] float openDoorDistance;
    [SerializeField] float openUnlockedDoorTime;
    [SerializeField] float openLockedDoorTime;
    [SerializeField] float closeDoorTime;
    [SerializeField] LayerMask door;

    [Header("Audio")]
    [SerializeField] string ambstart;
    [SerializeField] string ambmid;
    [SerializeField] string ambend;
    [SerializeField] string drag;

    AudioJob dragloop;
    AudioJob midamb;
    Quaternion stopRot;

    public State State { get; private set; }

    bool ambiencestarted;
    bool ambiencemid;
    bool canMove = true;
    bool canKill = true;

    float stepTimeElapsed;
    float inspectTimeElapsed;
    float openDoorTimeElapsed;
    float timeSinceOpenDoor;
    bool patrolling;

    private void Awake()
    {
        State = State.Patrol;
    }

    private void Update()
    {
        rigAnim.RigUpdate();
        if (!player.Dead)
        {
            if (canKill && Vector3.Distance(transform.position, player.transform.position) <= killDistance)
            {
                dragloop.StopPlaying();
                rigAnim.KillAnimation();
                player.Die(false);
                canKill = false;
                canMove = false;
            }
            if (canMove)
            {
                if (Visible)
                {
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    transform.rotation = stopRot;
                }
                if (agent.velocity.magnitude >= 0.1f && !Visible)
                {
                    dragloop.source.volume += Time.deltaTime;
                }
                else
                {
                    dragloop.source.volume -= Time.deltaTime;
                }
                if (!Physics.Raycast(transform.position, player.transform.position - transform.position, 
                    Vector3.Distance(player.transform.position, transform.position), solid) || Visible)
                {
                    Chase();
                }
                else if (State == State.Chase)
                {
                    InspectNoise(player.transform.position);
                }
                else if (State == State.Inspect)
                {
                    inspectTimeElapsed += Time.deltaTime;
                    if (Vector3.Distance(agent.destination, transform.position) <= inspectThreshold || inspectTimeElapsed > maxInspectTime)
                    {
                        State = State.Patrol;
                    }
                }
                else
                {
                    Patrol();
                }
                DoorOpen();
            }   
            else
            {
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                transform.rotation = stopRot;
            }
            stopRot = transform.rotation;

            #region Sound

            if (!Physics.Raycast(transform.position, player.transform.position - transform.position,
                    Vector3.Distance(player.transform.position, transform.position), solid) || Visible)
            {
                if (!ambiencestarted)
                {
                    AudioManager.Instance.PlayClip(ambstart).StopOnClipEnd(() =>
                    {
                        midamb = AudioManager.Instance.PlayClip(ambmid);
                        ambiencemid = true;
                    });
                    ambiencestarted = true;
                }
            }
            else if (State == State.Inspect)
            {
                if (ambiencestarted && ambiencemid)
                {
                    midamb.StopOnClipEnd(() =>
                    {
                        AudioManager.Instance.PlayClip(ambend).StopOnClipEnd(() =>
                        {
                            ambiencestarted = false;
                        });
                    });
                    ambiencemid = false;
                }
            }
            else
            {
                if (ambiencestarted && ambiencemid)
                {
                    midamb.StopOnClipEnd(() =>
                    {
                        AudioManager.Instance.PlayClip(ambend).StopOnClipEnd(() =>
                        {
                            ambiencestarted = false;
                        });
                    });
                    ambiencemid = false;
                }
            }

            #endregion
        }
        else
        {
            AudioManager.Instance.StopClipsWithName(ambstart);
            AudioManager.Instance.StopClipsWithName(ambmid);
            AudioManager.Instance.StopClipsWithName(ambend);
            AudioManager.Instance.StopClip(dragloop);
        }
    }

    void Chase()
    {
        State = State.Chase;
        agent.destination = player.transform.position;
        agent.speed = chaseSpeed;
        agent.stoppingDistance = chaseStopDistance;
    }

    public void InspectNoise(Vector3 noisePos, bool ignoreDistance = false)
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= inspectDistance || ignoreDistance)
        {
            State = State.Inspect;
            agent.destination = noisePos + (noisePos - transform.position).normalized * inspectThreshold;
            inspectTimeElapsed = 0;
            agent.speed = inspectSpeed;
            agent.stoppingDistance = inspectStopDistance;
        }
    }
    
    void Patrol()
    {
        if (!patrolling || State != State.Patrol)
        {
            do
            {
                agent.destination = transform.position + (Vector3.ProjectOnPlane(Random.insideUnitSphere, Vector3.up).normalized) * patrolStep;
            } while (agent.pathStatus == NavMeshPathStatus.PathInvalid);

            State = State.Patrol;
            patrolling = true;
            stepTimeElapsed = 0;
        }

        agent.speed = patrolSpeed;
        agent.stoppingDistance = patrolStopDistance;

        if (Vector3.Distance(agent.destination, transform.position) <= patrolThreshold || stepTimeElapsed > maxPatrolStepTime)
        {
            patrolling = false;
        }
        stepTimeElapsed += Time.deltaTime;
    }

    void DoorOpen()
    {
        //checks for a door, if the door is open, if the player is patrolling and the cooldown is gone or if the player is not patrolling
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, openDoorDistance, door) && 
            hit.collider.GetComponentInParent<Door>() != null && !hit.collider.GetComponentInParent<Door>().Open &&
            !hit.collider.GetComponentInParent<Door>().StageLocked && 
            ((State == State.Patrol && timeSinceOpenDoor >= doorOpenCooldownTime) || State != State.Patrol))
        {
            Door door = hit.collider.GetComponentInParent<Door>();
            openDoorTimeElapsed += Time.deltaTime;
            if (door.Locked)
            {
                if (openDoorTimeElapsed >= openLockedDoorTime)
                {
                    openDoorTimeElapsed = 0;
                    timeSinceOpenDoor = 0;
                    door.OpenDoor(true);
                    if (State == State.Patrol)
                    {
                        StartCoroutine(CloseDoor(door, true, closeDoorTime));
                    }
                }
            }
            else
            {
                if (openDoorTimeElapsed >= openUnlockedDoorTime)
                {
                    openDoorTimeElapsed = 0;
                    timeSinceOpenDoor = 0;
                    door.OpenDoor(true);
                    if (State == State.Patrol)
                    {
                        StartCoroutine(CloseDoor(door, false, closeDoorTime));
                    }
                }
            }

            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            openDoorTimeElapsed = 0;
            timeSinceOpenDoor += Time.deltaTime;
        }
    }

    IEnumerator CloseDoor(Door door, bool locked, float time)
    {
        yield return new WaitForSeconds(time);
        door.CloseDoor();
        if (locked)
        {
            door.LockDoor();
        }
    }

    public void Spawn(Vector3 position)
    {
        canMove = false;
        agent.Warp(position);
        agent.ResetPath();
        State = State.Patrol;

        dragloop = AudioManager.Instance.PlayClip(gameObject, drag);

        Invoke(nameof(Enable), spawnFreezeTime);
    }

    void Enable()
    {
        canMove = true;
        canKill = true;
    }

    public void StopForTime(float time)
    {
        canMove = false;

        Invoke(nameof(Enable), time);
    }
}
