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

            if (!spawned || !gameObject.activeInHierarchy)
            {
                return false;
            }

            return (
            !Physics.Raycast(cam.position, transform.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f - cam.position,
            Vector3.Distance(cam.transform.position, transform.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f), visionMask) ||
            !Physics.Raycast(cam.position, transform.position - Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f - cam.position,
            Vector3.Distance(cam.transform.position, transform.position - Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f), visionMask) ||
            !Physics.Raycast(cam.position, transform.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f - Vector3.up * 0.49f - cam.position,
            Vector3.Distance(cam.transform.position, transform.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f - Vector3.up * 0.49f), visionMask) ||
            !Physics.Raycast(cam.position, transform.position - Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f - Vector3.up * 0.49f - cam.position,
            Vector3.Distance(cam.transform.position, transform.position - Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f - Vector3.up * 0.49f), visionMask) ||
            !Physics.Raycast(transform.position, cam.transform.position - transform.position, Vector3.Distance(cam.transform.position, transform.position), visionMask)) &&
            cam.TryGetComponent(out Camera camera) && TryGetComponent(out Collider col) &&
            GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), col.bounds);
        }
    }
    [SerializeField] Player player;
    [SerializeField] EnemyRigAnim rigAnim;
    [SerializeField] LayerMask visionMask;
    [SerializeField] LayerMask solidMask;
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
    [SerializeField] float boxCameraTransitionTime;
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

    bool playerInspect;
    bool ambiencestarted;
    bool ambiencemid;
    bool canMove = true;
    bool canKill = true;

    float stepTimeElapsed;
    float inspectTimeElapsed;
    float openDoorTimeElapsed;
    float timeSinceOpenDoor;
    bool patrolling;
    bool spawned;

    private void Awake()
    {
        State = State.Patrol;
    }

    private void Start()
    {
        if (SaveSystem.Instance.currentSaveData != null)
        {
            if (SaveSystem.Instance.currentSaveData.EnemyPosition.Length != 0)
            {
                Spawn(new Vector3(SaveSystem.Instance.currentSaveData.EnemyPosition[0],
                                  SaveSystem.Instance.currentSaveData.EnemyPosition[1],
                                  SaveSystem.Instance.currentSaveData.EnemyPosition[2]));
            }
            spawned = SaveSystem.Instance.currentSaveData.enemySpawned;
        }

        SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
        {
            data.EnemyPosition = new float[]
            {
                transform.position.x,
                transform.position.y,
                transform.position.z
            };
            data.enemySpawned = spawned;
            SaveSystem.Instance.CanSave = !Visible && State == State.Patrol;
        };
    }

    private void Update()
    {
        if (!spawned)
        {
            dragloop.source.volume = 0;
            agent.isStopped = true;
            return;
        }

        Transform cam = player.cameraController.camera;
        
        /*Debug.DrawRay(cam.position, (transform.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f - cam.position).normalized *
            Vector3.Distance(cam.transform.position, transform.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f));
        Debug.DrawRay(cam.position, (transform.position - Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f - cam.position).normalized *
            Vector3.Distance(cam.transform.position, transform.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f));
        Debug.DrawRay(cam.position, (transform.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f - Vector3.up * 0.49f - cam.position).normalized *
            Vector3.Distance(cam.transform.position, transform.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f));
        Debug.DrawRay(cam.position, (transform.position - Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f - Vector3.up * 0.49f - cam.position).normalized *
            Vector3.Distance(cam.transform.position, transform.position + Vector3.Cross(Vector3.up, transform.position - cam.position).normalized * 0.24f + Vector3.up * 0.49f));*/
        
        rigAnim.RigUpdate();
        if (!player.Dead)
        {
            if (canKill && Vector3.Distance(transform.position, cam.transform.position) <= killDistance &&
                Physics.Raycast(transform.position, cam.transform.position - transform.position, out RaycastHit hit,
                    Vector3.Distance(cam.transform.position, transform.position), solidMask) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerKill();
            }
            if (canMove)
            {
                if (Visible)
                {
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    transform.rotation = stopRot;
                }

                if (Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hit2,
                    Vector3.Distance(player.transform.position, transform.position), solidMask) && hit2.transform.gameObject.layer == LayerMask.NameToLayer("Player")
                    || Visible)
                {
                    Chase();
                    playerInspect = false;
                }
                else if (State == State.Chase)
                {
                    InspectNoise(cam.transform.position, playerInspect: true);
                }
                else if (State == State.Inspect)
                {
                    if (playerInspect)
                    {
                        /*Debug.DrawRay(transform.position, (cam.transform.position - transform.position).normalized * 
                        Vector3.Distance(cam.transform.position, transform.position), Color.red);*/
                        if (Physics.Raycast(transform.position, cam.transform.position - transform.position, out RaycastHit hit4,
                        killDistance, solidMask) && hit4.collider.CompareTag("HidingBox") &&
                        hit4.collider.TryGetComponent(out HidingBox box) && box.hasPlayer)
                        {
                            canMove = false;
                            box.TriggerDeath(transform.position);
                            Invoke(nameof(PlayerKill), boxCameraTransitionTime);
                        }
                    }
                    inspectTimeElapsed += Time.deltaTime;
                    if (Vector3.Distance(agent.destination, transform.position) <= inspectThreshold)
                    {
                        State = State.Patrol;
                    }
                }
                else if (State == State.Patrol)
                {
                    playerInspect = false;
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
            
            #region Sound

            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hit3,
                    Vector3.Distance(player.transform.position, transform.position), solidMask) && hit3.transform.gameObject.layer == LayerMask.NameToLayer("Player")
                    || Visible)
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
                    if (midamb != null)
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
            }
            else
            {
                if (ambiencestarted && ambiencemid)
                {
                    if (midamb != null)
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
            }

            if (dragloop != null)
            {
                if (agent.velocity.magnitude >= 0.1f)
                {
                    dragloop.source.volume += Time.deltaTime;
                }
                else
                {
                    dragloop.source.volume -= Time.deltaTime;
                }
                dragloop.source.volume = Mathf.Clamp01(dragloop.source.volume);
            }

            #endregion
        }
        else
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            transform.rotation = stopRot;
            AudioManager.Instance.StopClipsWithName(ambstart);
            AudioManager.Instance.StopClipsWithName(ambmid);
            AudioManager.Instance.StopClipsWithName(ambend);
            AudioManager.Instance.StopClip(dragloop);
        }
        stopRot = transform.rotation;
    }

    void PlayerKill()
    {
        if (canKill)
        {
            dragloop.StopPlaying();
            rigAnim.KillAnimation();
            player.Die(false);
            canKill = false;
        }
    }

    void Chase()
    {
        State = State.Chase;
        agent.destination = player.cameraController.camera.transform.position;
        agent.speed = chaseSpeed;
        agent.stoppingDistance = chaseStopDistance;
    }

    public void InspectNoise(Vector3 noisePos, bool ignoreDistance = false, bool playerInspect = false)
    {
        if (this.playerInspect || !gameObject.activeInHierarchy)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= inspectDistance || ignoreDistance)
        {
            State = State.Inspect;
            if (!playerInspect)
            {
                agent.stoppingDistance = inspectStopDistance;
                agent.destination = noisePos + (noisePos - transform.position).normalized * inspectThreshold;
            }
            else
            {
                agent.stoppingDistance = 0;
                agent.destination = noisePos;
            }
            inspectTimeElapsed = 0;
            agent.speed = inspectSpeed;
            this.playerInspect = playerInspect;
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
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, openDoorDistance, door))
        {
            Door door = hit.collider.GetComponentInParent<Door>();
            if (door != null && !door.Open && !door.StageLocked)
            {
                if ((State == State.Patrol && timeSinceOpenDoor >= doorOpenCooldownTime) || State != State.Patrol)
                {
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
                    return;
                }
            }
        }
        agent.isStopped = false;
        openDoorTimeElapsed = 0;
        timeSinceOpenDoor += Time.deltaTime;
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
        spawned = true;
        canMove = false;
        agent.Warp(position);
        agent.ResetPath();
        State = State.Patrol;

        AudioManager.Instance.StopClip(dragloop);
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
