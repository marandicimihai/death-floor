using UnityEngine;

public class Lockpick : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float lockPickTime;
    [SerializeField] float lockHoldTime;
    [SerializeField] float lockTime;

    [Header("Sounds")]
    [SerializeField] string picklock;

    float lockpickMultiplier;

    public void BoostForTime(float multiplier, float time)
    {
        
    }

    void WearOffBoost()
    {
        lockpickMultiplier = 1;
    }
}
