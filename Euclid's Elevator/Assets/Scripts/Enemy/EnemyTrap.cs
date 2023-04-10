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
            GameManager.instance.playerController.AddForce(transform.position - GameManager.instance.player.position, forceStrength);
            GameManager.instance.enemyController.NoiseHeardNav(transform.position);
            if (Vector3.Distance(GameManager.instance.player.position, transform.position) < enemyTPRadius)
            {
                GameManager.instance.playerController.Die();
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
