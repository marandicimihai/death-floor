using UnityEngine;

namespace DeathFloor.Utilities
{
    internal class GameObjectTrigger : MonoBehaviour
    {
        public void DisableGameObject()
        {
            gameObject.SetActive(false);
        }
    }
}