using UnityEngine;

public class EnemyRigAnim : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] EnemyNavigation enemy;
    [SerializeField] Animator animator;

    private void Update()
    {
        Quaternion rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized, Vector3.up);
        transform.rotation = rotation * Quaternion.Euler(0, -90, 0);
    }

    public void KillAnimation()
    {
        animator.SetInteger("State", -1);
        if (enemy.Visible)
        {
            animator.SetTrigger("Execute2");
        }
        else
        {
            animator.SetTrigger("Execute1");
        }
    }
}
