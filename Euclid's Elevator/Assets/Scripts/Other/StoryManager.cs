using UnityEngine;
using System;

/*
 * 0 - Spawn
 * 1 - First death
 * 2 - Second death
 * 3 - Third death
 * 4 - Fourth death
 * 5 - End
 */

[System.Serializable]
struct StoryEvent
{
    public string name;
    public string lineName;
}

[RequireComponent(typeof(GameManager))]
public class StoryManager : MonoBehaviour
{
    [SerializeField] StoryEvent[] events;
    [SerializeField] GameManager manager;

    private void OnEnable()
    {
        manager.OnSpawn += (object sender, EventArgs args) => LineManager.instance.SayLine(events[0].lineName);
        manager.OnDeath += (object sender, DeathArgs args) => LineManager.instance.SayLine(events[args.death].lineName);
        manager.OnEnd += (object sender, EventArgs args) => LineManager.instance.SayLine(events[5].lineName);
    }
}
