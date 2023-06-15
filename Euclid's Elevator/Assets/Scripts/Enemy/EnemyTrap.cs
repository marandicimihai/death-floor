using System.Collections;
using UnityEngine;
using System;

public class EnemyTrap : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform baseBone;
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
    Transform player;

    Quaternion initial;

    float time;
    bool pull;
    bool hasPulled;
    bool used;

    private void Start()
    {
        player = GameManager.Instance.playerTransform;

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
            GameManager.Instance.OnDeath += OnClosed;
            GameManager.Instance.OnStageStart += OnClosed;
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
        if (pull && Vector3.ProjectOnPlane(transform.position - player.position, Vector3.up).magnitude >= minRadius && !used
            && Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hitInfo, 20, playerMask) && hitInfo.collider.CompareTag("Player"))
        {
            anim.SetTrigger("Spot");

            baseBone.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.position - baseBone.position, Vector3.up).normalized, Vector3.up) * initial;

            GameManager.Instance.player.controller.AddForce(transform.position - player.position, forceStrength);
            GameManager.Instance.enemy.InspectNoise(transform.position, true);

            if (Vector3.Distance(player.position, transform.position) < deathRadius)
            {
                AudioManager.Instance.PlayClip(gameObject, groundhit);
                AudioManager.Instance.PlayClip(gameObject, screech);
                AudioManager.Instance.StopClip(ambiencejob);
                anim.SetTrigger("Kill");
                GameManager.Instance.player.Die(false);
                used = true;
            }

            hasPulled = true;
        }

        if (!pull && !used)
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
            }
            pull = false;
            used = true;
        }
    }
}