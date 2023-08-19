using UnityEngine;

public class CoffeeTrail : MonoBehaviour
{
    [System.NonSerialized] public float stunTime;
    [SerializeField] string spilldrink;
    [SerializeField] int drops;
    [SerializeField] float dropRadius;
    [SerializeField] GameObject coffeeImpact;
    [SerializeField] GameObject coffeeDrop;
    [SerializeField] float lifetime;

    bool done;
    Collider other;
    EnemyNavigation enemy;

    private void Start()
    {
        enemy = FindObjectOfType<EnemyNavigation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
        if (!done)
        {
            done = true;
            gameObject.AddComponent<Lifetime>().Initiate(lifetime);
            Instantiate(coffeeImpact, transform.position, Quaternion.identity).AddComponent<Lifetime>().Initiate(lifetime);
            
            AudioManager.Instance.PlayClip(GameManager.Instance.enemy.gameObject, spilldrink);

            this.other = other;
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                for (int i = 0; i < drops; i++)
                {
                    Invoke(nameof(SpawnParticle), Random.Range(0f, 2f));
                }
                if (enemy != null)
                {
                    enemy.StopForTime(stunTime);
                }
                else
                {
                    Debug.Log("No enemy.");
                }
            }
        }
    }
    
    void SpawnParticle()
    {
        GameObject instance = Instantiate(coffeeDrop, other.transform);
        instance.transform.localPosition = Random.insideUnitSphere * dropRadius + Vector3.up * 0.6f;
        instance.AddComponent<Lifetime>().Initiate(stunTime);
    }
}
