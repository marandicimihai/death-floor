using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void CallDeath()
    {
        if (player == null)
        {
            Debug.Log("No player.");
            return;
        }
        player.CallDeath();
    }
}
