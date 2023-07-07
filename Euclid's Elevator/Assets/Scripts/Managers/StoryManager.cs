using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogue;
    [SerializeField] Journal journal;
    [SerializeField] PopUpHUD popUp;

    [Header("First keycard pickup")]
    [SerializeField] JournalPage objective;
    [SerializeField] JournalPage keycardDescription;
    [SerializeField] PopUpProperties newObjective;
    [SerializeField] PopUpProperties newItemKeycard;
    [SerializeField] Line line1;

    private void Start()
    {
        Inventory.OnPickUpKeycard += OnPickUpKeycard;
    }

    void OnPickUpKeycard(object caller, System.EventArgs args)
    {
        //journal.AddPage(objective);
        //journal.AddPage(keycardDescription);
        popUp.PopUp(newObjective);
        popUp.PopUp(newItemKeycard);
        dialogue.SayLine(line1);
        Inventory.OnPickUpKeycard -= OnPickUpKeycard;
    }
}
