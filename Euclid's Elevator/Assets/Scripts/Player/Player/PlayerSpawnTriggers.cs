using DeathFloor.Player;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Animation.Triggers
{
    enum ReferenceType
    {
        ParentName,
        ObjectReference
    }

    internal class PlayerSpawnTriggers : MonoBehaviour
    {
        [SerializeField] private ReferenceType _referenceType;
        [SerializeField] private string _parentName;
        [SerializeField, RequireInterface(typeof(IPlayer))] private Object _playerObject;

        public void InvokeSpawn(float delay)
        {

        }
    }
}