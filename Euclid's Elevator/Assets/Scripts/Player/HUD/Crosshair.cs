using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.HUD
{
    internal class Crosshair : MonoBehaviour, IToggleable
    {
        public void Disable()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        public void Enable()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}