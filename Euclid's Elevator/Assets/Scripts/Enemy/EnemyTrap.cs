using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    [SerializeField] float triggerRadius;
    [SerializeField] float enemyTPRadius;
    [SerializeField] float forceStrength;

    bool pull;

    private void OnValidate()
    {
        if (TryGetComponent(out SphereCollider coll))
        {
            coll.radius = triggerRadius;
        }
    }

    private void Update()
    {
        if (pull)
        {
            GameManager.Instance.playerController.AddForce(transform.position - GameManager.Instance.player.position, forceStrength);
            GameManager.Instance.enemyController.NoiseHeardNav(transform.position);
            if (Vector3.Distance(GameManager.Instance.player.position, transform.position) < enemyTPRadius)
            {
                GameManager.Instance.playerController.Die();
            }
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
            pull = false;
        }
    }
}
