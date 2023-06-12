using UnityEngine;

[CreateAssetMenu(fileName = "New Line", menuName = "ScriptableObjects/Line")]
public class Line : ScriptableObject
{
    public new string name;
    public string text;
    public float timeLastingAfterTyping;
    public bool oneTime;
}