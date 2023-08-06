using UnityEngine;

public class Lockpick : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float lockPickTime;
    [SerializeField] float lockHoldTime;
    [SerializeField] float lockTime;

    [Header("Sounds")]
    [SerializeField] string picklock;

    Door target;

    float lockpickMultiplier;
    float timeElapsed;

    bool picking;
    bool locking;

    private void Awake()
    {
        lockpickMultiplier = 1;
    }

    private void Update()
    {
        
    }

    public void PickLock(Door door)
    {
        
    }

    public void Lock(Door door)
    {
        
    }

    public void Stop()
    {
        
    }

    void StopSlider()
    {
        
    }

    public void BoostForTime(float multiplier, float time)
    {
        
    }

    void WearOffBoost()
    {
        lockpickMultiplier = 1;
    }
}
