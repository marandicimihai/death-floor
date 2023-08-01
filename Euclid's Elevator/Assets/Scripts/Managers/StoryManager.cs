using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogue;
    [SerializeField] Journal journal;
    [SerializeField] PopUpHUD popUp;

    [Header("First keycard pickup")]
    [SerializeField] JournalPage objective;
    [SerializeField] PopUpProperties newObjective;
    [SerializeField] Line line1;

    [Header("First room explored")]
    [SerializeField] Line line2;
    bool happenedOnce;

    [Header("Door locked")]
    [SerializeField] Line line3;

    [Header("Stage locked door")]
    [SerializeField] JournalPage stageLockedPage;
    [SerializeField] Line line4;

    [Header("First death")]
    [SerializeField] Line line5;

    private void Start()
    {
        //Inventory.OnPickUpKeycard += OnPickUpKeycard;
        Door.OnOpenAnyDoor += OnOpenAnyDoor;
        Door.OnTryUnlockAnyDoor += DoorLocked;
        Door.OnTryUnlockStageDoor += StageDoor;
        GameManager.Instance.OnDeath += FirstDeath;
    }

    void OnPickUpKeycard(object caller, System.EventArgs args)
    {
        journal.AddPage(objective);
        popUp.PopUp(newObjective);
        dialogue.SayLine(line1);
        //Inventory.OnPickUpKeycard -= OnPickUpKeycard;
    }

    void OnOpenAnyDoor(object caller, System.EventArgs args)
    {
        if (happenedOnce)
        {
            dialogue.SayLine(line2);
            Door.OnOpenAnyDoor -= OnOpenAnyDoor;
        }
        else
        {
            happenedOnce = true;
        }
    }

    void DoorLocked(object caller, System.EventArgs args)
    {
        dialogue.SayLine(line3);
        Door.OnTryUnlockAnyDoor -= DoorLocked;
    }

    void StageDoor(object caller, System.EventArgs args)
    {
        journal.AddPage(stageLockedPage);
        dialogue.SayLine(line4);
        Door.OnTryUnlockStageDoor -= StageDoor;
    }

    void FirstDeath(object caller, System.EventArgs args)
    {
        dialogue.SayLine(line5);
        GameManager.Instance.OnDeath -= FirstDeath;
    }
}
