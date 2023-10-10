using DeathFloor.Player;
using UnityEngine;

namespace DeathFloor.Animation.Triggers
{
    internal class PlayerSpawnTrigger : MonoBehaviour, IPlayerSpawnTrigger
    {
        [SerializeField] private string _parentName;

        private IPlayer _player;

        public void Spawn()
        {
            GameObject obj = GameObject.Find(_parentName);
            
            if (obj != null)
                _player ??= obj.GetComponent<IPlayer>();

            _player?.Spawn();
        }
    }
}