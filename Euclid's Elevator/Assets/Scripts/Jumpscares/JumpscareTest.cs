using UnityEngine;
using System.Linq;

public class JumpscareTest : JumpscareBase
{
    [SerializeField] new BoxCollider collider;
    [SerializeField] Vector3 enemySpawn;

    protected override bool CheckContext()
    {
        if (Physics.OverlapSphere(GameManager.Instance.player.position, 0f).ToList().Contains(collider))
        {
            return true;
        }
        return false;
    }

    protected override void OnJumpscareTriggered()
    {
        GameManager.Instance.enemyController.Spawn(enemySpawn);
        Debug.Log("Boo!");
    }
}
