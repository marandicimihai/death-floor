using System.Collections;
using UnityEngine.AI;
using UnityEngine;

enum State
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

    [Header("Patrol")]
    [SerializeField] float doorOpenCooldownTime;
    [SerializeField] float maxPatrolStepTime;
    [SerializeField] float patrolStep; 
    [SerializeField] float patrolThreshold;
    [SerializeField] float patrolSpeed;
    [SerializeField] float patrolStopDistance;

    [Header("Inspect")]
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

    NavMeshAgent agent;
    State state;

    bool canMove;
    bool canKill;

    float stepTimeElapsed;
    float inspectTimeElapsed;
    float openDoorTimeElapsed;
    float timeSinceOpenDoor;
    bool patrolling;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        state = State.Patrol;
        canMove = true;
        canKill = true;
    }

    private void Update()
    {
        if (!player.Dead)
        {
            if (canKill && Vector3.Distance(transform.position, player.transform.position) <= killDistance)
            {
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
                }
                if (!Physics.Raycast(transform.position, player.transform.position - transform.position, 
                    Vector3.Distance(player.transform.position, transform.position), solid) || Visible)
                {
                    FaceTarget();
                    Chase();
                }
                else if (state == State.Chase)
                {
                    InspectNoise(player.transform.position);
                }
                else if (state == State.Inspect)
                {
                    inspectTimeElapsed += Time.deltaTime;
                    if (Vector3.Distance(agent.destination, transform.position) <= inspectThreshold || inspectTimeElapsed > maxInspectTime)
                    {
                        state = State.Patrol;
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
            }
        }
    }

    void Chase()
    {
        state = State.Chase;
        agent.destination = player.transform.position;
        agent.speed = chaseSpeed;
        agent.stoppingDistance = chaseStopDistance;
    }

    void InspectNoise(Vector3 noisePos)
    {
        state = State.Inspect;
        agent.destination = noisePos + (noisePos - transform.position).normalized * inspectThreshold;
        inspectTimeElapsed = 0;
        agent.speed = inspectSpeed;
        agent.stoppingDistance = inspectStopDistance;
    }
    
    void Patrol()
    {
        if (!patrolling || state != State.Patrol)
        {
            do
            {
                agent.destination = transform.position + (Vector3.ProjectOnPlane(Random.insideUnitSphere, Vector3.up).normalized) * patrolStep;
            } while (agent.pathStatus == NavMeshPathStatus.PathInvalid);

            state = State.Patrol;
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
            ((state == State.Patrol && timeSinceOpenDoor >= doorOpenCooldownTime) || state != State.Patrol))
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
                    if (state == State.Patrol)
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
                    if (state == State.Patrol)
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

    void FaceTarget()
    {
        Vector3 target = agent.steeringTarget;

        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
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
        state = State.Patrol;

        Invoke(nameof(Enable), spawnFreezeTime);
    }

    void Enable()
    {
        canMove = true;
        canKill = true;
    }
}
