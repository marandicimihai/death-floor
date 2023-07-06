using UnityEngine.UI;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Text text;

    public void AssignProperties(PopUpProperties properties)
    {
        image.sprite = properties.image;
        text.text = properties.text;
    }
}
