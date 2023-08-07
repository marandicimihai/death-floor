using System.Collections;
using UnityEngine;
using System;

public class EnemyTrap : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform baseBone;
    [SerializeField] Transform ray;
    [SerializeField] LayerMask playerMask;
    [SerializeField] float triggerRadius;
    [SerializeField] float deathRadius;
    [SerializeField] float forceStrength;
    [SerializeField] float minRadius;
    [SerializeField] float lookAroundTimeInterval;
    [SerializeField] float turnTime;

    [Header("Audio")]
    [SerializeField] string groundhit;
    [SerializeField] string screech;
    [SerializeField] string ambience;
    [SerializeField] string whoosh;

    Transform cam;
    AudioJob ambiencejob;
    Player player;
    EnemyNavigation nav;
    FirstPersonController controller;

    Quaternion initial;

    float time;
    bool hasPulled;
    bool used;

    private void Start()
    {
        controller = FindObjectOfType<FirstPersonController>();
        nav = FindObjectOfType<EnemyNavigation>();
        cam = FindObjectOfType<Camera>().transform;
        player = FindObjectOfType<Player>();

        initial = baseBone.rotation;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDeath += OnClosed;
            GameManager.Instance.OnElevatorDoorClosed += OnClosed;
        }
        else
        {
            Debug.Log("Game manager is absent");
        }

        ambiencejob = AudioManager.Instance.PlayClip(gameObject, ambience);
    }

    private void Update()
    {
        if (used)
            return;

        if (Physics.Raycast(ray.position, cam.position - ray.position, out RaycastHit hitInfo, triggerRadius, playerMask) 
            && hitInfo.collider.CompareTag("Player"))
        {
            anim.SetTrigger("Spot");

            baseBone.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(cam.position - baseBone.position, Vector3.up).normalized, Vector3.up) * initial;

            if (Vector3.ProjectOnPlane(ray.position - cam.position, Vector3.up).magnitude >= minRadius)
            {
                if (controller != null)
                {
                    controller.AddForce(transform.position - cam.position, forceStrength);
                }
                else
                {
                    Debug.Log("No controller.");
                }
            }

            if (nav != null)
            {
                nav.InspectNoise(transform.position, true);
            }
            else
            {
                Debug.Log("No enemy navigation.");
            }

            if (!hasPulled)
            {
                AudioManager.Instance.PlayClip(gameObject, screech);
            }

            if (player != null && Vector3.Distance(cam.position, ray.position) < deathRadius && !player.Dead)
            {
                AudioManager.Instance.PlayClip(gameObject, groundhit);
                AudioManager.Instance.StopClip(ambiencejob);

                player.Die(false);
                anim.SetTrigger("Kill");
            }
            else if (player == null)
            {
                Debug.Log("No player.");
            }

            hasPulled = true;
        }
        else if (hasPulled)
        {
            anim.SetTrigger("Burrow");
            AudioManager.Instance.StopClip(ambiencejob);
            AudioManager.Instance.PlayClip(gameObject, whoosh);
            used = true;
        }
        else
        {
            time += Time.deltaTime;

            if (time >= lookAroundTimeInterval)
            {
                time = 0;

                StartCoroutine(Turn(baseBone.rotation, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f) * initial));
            }
        }
    }

    IEnumerator Turn(Quaternion start, Quaternion final)
    {
        float t = 0;

        while (t <= 1)
        {
            t += Time.deltaTime / turnTime;

            baseBone.rotation = Quaternion.Slerp(start, final, t);

            yield return null;
        }
    }

    void OnClosed(object sender, EventArgs args)
    {
        if (used)
        {
            Destroy(this.gameObject);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnDeath -= OnClosed;
                GameManager.Instance.OnElevatorDoorClosed -= OnClosed;
            }
            else
            {
                Debug.Log("No game manager.");
            }
        }
    }

    private void OnDestroy()
    {
        try
        {
            TrapManager.RemoveFromTraps(transform);
        } catch { Debug.Log("Trap manager issue"); }
    }
}