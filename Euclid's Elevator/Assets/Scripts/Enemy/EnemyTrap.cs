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
    [SerializeField] AudioSource ambiance;
    [SerializeField] AudioSource hit;
    [SerializeField] AudioSource screech;
    [SerializeField] AudioSource exit;

    Quaternion initial;

    float time;
    bool pull;
    bool used;

    private void Start()
    {
        initial = baseBone.rotation;

        GameManager.Instance.OnElevatorDoorClosed += OnClosed;
    }

    void OnClosed(object sender, EventArgs args)
    {
        if (used)
        {
            Destroy(this.gameObject);
            GameManager.Instance.OnElevatorDoorClosed -= OnClosed;
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
        if (pull && Vector3.ProjectOnPlane(transform.position - GameManager.Instance.player.position, Vector3.up).magnitude >= minRadius && !used
            && Physics.Raycast(transform.position, GameManager.Instance.player.position - transform.position, out RaycastHit hitInfo, 20, playerMask) && hitInfo.collider.CompareTag("Player"))
        {
            anim.SetTrigger("Spot");

            baseBone.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(GameManager.Instance.player.position - baseBone.position, Vector3.up).normalized, Vector3.up) * initial;

            GameManager.Instance.playerController.AddForce(transform.position - GameManager.Instance.player.position, forceStrength);
            GameManager.Instance.enemyController.NoiseHeardNav(transform.position);

            if (Vector3.Distance(GameManager.Instance.player.position, transform.position) < deathRadius)
            {
                hit.Play();
                screech.Play();

                anim.SetTrigger("Kill");
                GameManager.Instance.playerController.TrapDie();
                ambiance.Stop();
                used = true;
            }
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
            if (!used)
            {
                anim.SetTrigger("Burrow");
                ambiance.Stop();
                exit.Play();
            }
            pull = false;
            used = true;
        }
    }
}
