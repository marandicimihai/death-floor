using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New item", menuName = "ScriptableObjects/Item")]
public class ItemObject : ScriptableObject
{
    public new string name;
    public Image inventoryIcon;
    public GameObject prefab;
}
