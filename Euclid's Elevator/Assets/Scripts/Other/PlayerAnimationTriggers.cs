using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    [SerializeField] Player player;

    public void CallDeath()
    {
        player.CallDeath();
    }
}
