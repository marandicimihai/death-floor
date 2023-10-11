using DeathFloor.Player;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Animation.Triggers
{
    internal class PlayerSpawnTriggers : MonoBehaviour, IPlayerSpawnTrigger
    {
        [SerializeField] private string _parentName;

        private IPlayer _player;

        public void Spawn()
        {
            if (_player == null)
            {
                var obj = GameObject.Find(_parentName);

                if (obj != null)
                    _player = obj.GetComponent<IPlayer>();
            }

            _player.Spawn();
        }
    }
}