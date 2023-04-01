using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New item", menuName = "ScriptableObjects/Item")]
public class ItemObject : ScriptableObject
{
    public string itemName;
    public int uses;
    public Sprite inventoryIcon;
    public GameObject prefab;
}
