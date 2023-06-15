using UnityEngine;

[CreateAssetMenu(fileName = "New Journal Page", menuName = "ScriptableObjects/Journal Page")]
public class JournalPage : ScriptableObject
{
    public new string name;
    public GameObject pageHUDPrefab;
}