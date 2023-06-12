using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    public void CallDeath()
    {
        GameManager.Instance.player.CallDeath();
    }
}
