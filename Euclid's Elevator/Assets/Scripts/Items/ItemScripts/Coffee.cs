using UnityEngine;

public class Coffee : Item, IUsable
{
    [SerializeField] [SyncValue] string spilldrink;
    [SerializeField] [SyncValue] float throwDistance;
    [SerializeField] [SyncValue] float stunTime;

    public bool OnUse(Player player)
    {
        if (Vector3.Distance(GameManager.Instance.enemy.transform.position, transform.position) <= throwDistance &&
            GameManager.Instance.enemy.Visible)
        {
            GameManager.Instance.enemy.StopForTime(stunTime);
            AudioManager.Instance.PlayClip(GameManager.Instance.enemy.gameObject, spilldrink);
        }
        return true;
    }
}
