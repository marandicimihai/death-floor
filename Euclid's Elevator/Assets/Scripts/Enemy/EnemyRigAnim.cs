using UnityEngine;

public class EnemyRigAnim : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] EnemyNavigation enemy;
    [SerializeField] Animator animator;

    [Header("Animation Properties")]
    [SerializeField] int firstRun;
    [SerializeField] int lastRun;
    [SerializeField] int firstPose;
    [SerializeField] int lastPose;
    [SerializeField] float timeBetweenPoses;

    [SerializeField] string deathback;
    [SerializeField] string deathfront;

    float cooldown;

    public void RigUpdate()
    {
        if (!enemy.Visible)
        {
            Quaternion rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized, Vector3.up);
            transform.rotation = rotation * Quaternion.Euler(0, -90, 0);

            cooldown += Time.deltaTime;
            if (cooldown >= timeBetweenPoses)
            {
                if (enemy.State == State.Chase || enemy.State == State.Inspect)
                {
                    animator.SetInteger("State", UnityEngine.Random.Range(firstRun, lastRun + 1));
                }
                else
                {
                    animator.SetInteger("State", UnityEngine.Random.Range(firstPose, lastPose + 1));
                }
                cooldown = 0;
            }
        }
        else
        {
            cooldown = 0;
        }
    }

    public void KillAnimation()
    {
        animator.SetInteger("State", -1);
        if (enemy.Visible)
        {
            AudioManager.Instance.PlayClip(deathfront);
            animator.SetTrigger("Execute2");
        }
        else
        {
            AudioManager.Instance.PlayClip(deathback);
            animator.SetTrigger("Execute1");
        }
    }
}
