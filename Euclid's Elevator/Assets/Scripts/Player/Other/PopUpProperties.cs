using UnityEngine;

[CreateAssetMenu(fileName = "New Pop Up", menuName = "ScriptableObjects/PopUp")]
public class PopUpProperties : ScriptableObject
{
    public new string name;
    public Sprite image;
    public string text;
    public bool oneTime;
}