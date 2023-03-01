using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New item", menuName = "ScriptableObjects/Item")]
public class ItemObject : ScriptableObject
{
    public string itemName;
    public Image inventoryIcon;
    public GameObject prefab;
}
