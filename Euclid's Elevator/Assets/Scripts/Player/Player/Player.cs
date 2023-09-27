using DeathFloor.Insanity;
using DeathFloor.Inventory;
using DeathFloor.Movement;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Player
{
    [SelectionBase]
    internal class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private bool _autoSpawn;

        [SerializeField, RequireInterface(typeof(ICameraController))] private Object _cameraController;
        [SerializeField, RequireInterface(typeof(IFirstPersonController))] private Object _fpsController;
        [SerializeField, RequireInterface(typeof(IInsanityManager))] private Object _insanity;
        [SerializeField, RequireInterface(typeof(IInventoryManager))] private Object _inventory;

        private IInventoryManager _inventoryManager;
        private IInsanityManager _insanityManager;
        private ICameraController _camera;
        private IFirstPersonController _fps;

        private bool _dead;

        private void Start()
        {
            _inventoryManager = _inventory as IInventoryManager;
            _insanityManager = _insanity as IInsanityManager;
            _camera = _cameraController as ICameraController;
            _fps = _fpsController as IFirstPersonController;

            Spawn();
        }

        public void Die()
        {
            if (_dead) return;

            _inventoryManager.ClearInventory();
            _insanityManager.ResetInsanity();
            _camera.Disable();
            _fps.Disable();

            _dead = true;

            if (_autoSpawn) Spawn();
        }

        public void Spawn()
        {
            transform.position = _spawnPoint.position;

            _dead = false;

            _camera.ResetAngle();
            _camera.Enable();
            _fps.Enable();
        }
    }
}