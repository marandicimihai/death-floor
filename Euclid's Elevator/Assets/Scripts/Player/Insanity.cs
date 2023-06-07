using UnityEngine;

public class Insanity : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] EnemyNavigation enemy;
    [SerializeField] float insanityTime;

    float insanity;

    private void Start()
    {
        GameManager.Instance.OnStageStart += (object caller, System.EventArgs args) => insanity = 0;
        GameManager.Instance.OnDeath += (object caller, System.EventArgs args) => insanity = 0;
    }

    private void Update()
    {
        if (enemy.Visible)
        {
            insanity += Time.deltaTime / insanityTime;
        }
        if (insanity >= 1)
        {
            player.Die(true);
        }
    }
}
