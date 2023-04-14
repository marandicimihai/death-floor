using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "ScriptableObjects/Item")]
public class ItemObject : ScriptableObject
{
    public string itemName;
    public int uses;
    public Sprite inventoryIcon;
    public GameObject prefab;
    public string[] pickUpSounds;
    public string[] drawSounds;
    public string[] useSounds;
    public bool useSoundsInOrder;
}
