using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "ScriptableObjects/Item")]
public class ItemProperties : ScriptableObject
{
    public new string name;
    public GameObject physicalObject;
    public int uses;
    public Sprite icon;

    [Header("Hold Properties")]
    public GameObject inHandObject;
    public Vector3 offset;
    public Vector3 holdAngles;

    [Header("Sounds")]
    public string[] pickup;
    public string[] holster;
    public string[] use;
    public string[] drop;
    public bool useUseSoundsInOrder;
}
