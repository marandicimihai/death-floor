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

    AudioJob ambiencejob;
    Player player;

    Quaternion initial;

    float time;
    bool pull;
    bool hasPulled;
    bool used;

    private void Start()
    {
        player = GameManager.Instance.player;

        initial = baseBone.rotation;

        GameManager.Instance.OnDeath += OnClosed;
        GameManager.Instance.OnStageStart += OnClosed;

        ambiencejob = AudioManager.Instance.PlayClip(gameObject, ambience);
    }

    void OnClosed(object sender, EventArgs args)
    {
        if (used)
        {
            Destroy(this.gameObject);
            GameManager.Instance.OnDeath -= OnClosed;
            GameManager.Instance.OnStageStart -= OnClosed;
        }
    }

    private void OnValidate()
    {
        if (TryGetComponent(out SphereCollider coll))
        {
            coll.radius = triggerRadius;
        }
    }

    private void Update()
    {
        if (used)
            return;

        Transform cam = player.cameraController.camera;


        if (pull && Vector3.ProjectOnPlane(ray.position - cam.position, Vector3.up).magnitude >= minRadius && 
            Physics.Raycast(ray.position, cam.position - ray.position, out RaycastHit hitInfo, 20, playerMask) && hitInfo.collider.CompareTag("Player"))
        {
            anim.SetTrigger("Spot");

            baseBone.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(cam.position - baseBone.position, Vector3.up).normalized, Vector3.up) * initial;

            GameManager.Instance.player.controller.AddForce(transform.position - cam.position, forceStrength);
            GameManager.Instance.enemy.InspectNoise(transform.position, true);

            if (!hasPulled)
            {
                AudioManager.Instance.PlayClip(gameObject, screech);
            }

            if (Vector3.Distance(cam.position, ray.position) < deathRadius && !GameManager.Instance.player.Dead)
            {
                AudioManager.Instance.PlayClip(gameObject, groundhit);
                AudioManager.Instance.StopClip(ambiencejob);
                anim.SetTrigger("Kill");
                GameManager.Instance.player.Die(false);
                used = true;
            }

            hasPulled = true;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pull = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pull = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!used && hasPulled)
            {
                anim.SetTrigger("Burrow");
                AudioManager.Instance.StopClip(ambiencejob);
                AudioManager.Instance.PlayClip(gameObject, whoosh);
                pull = false;
                used = true;
            }
        }
    }
}