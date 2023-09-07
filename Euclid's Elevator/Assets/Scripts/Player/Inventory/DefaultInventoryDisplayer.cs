using System;
using UnityEngine;
using UnityEngine.UI;

namespace DeathFloor.Inventory
{
    internal class DefaultInventoryDisplayer : MonoBehaviour, IInventoryDisplayer
    {
        [SerializeField] private Sprite _unselectedFrame;
        [SerializeField] private Sprite _selectedFrame;
        [SerializeField] private Image[] _frames;
        [SerializeField] private Image[] _items;
        private bool _canDisplay;

        private void Start()
        {
            if (_frames.Length != _items.Length)
            {
                Debug.LogError("Frames and items are not the same length. This can cause errors.");
            }
            Enable();
        }

        public void Disable()
        {
            for (int i = 0; i < _frames.Length; i++)
            {
                _items[i].enabled = false;
                _frames[i].enabled = false;
            }
            _canDisplay = false;
        }

        public void Enable()
        {
            for (int i = 0; i < _frames.Length; i++)
            {
                _frames[i].enabled = true;
            }
            _canDisplay = true;
        }

        public void RefreshView(CollectableItem[] items, int selectedIndex)
        {
            if (!_canDisplay) return;

            for (int i = 0; i < _frames.Length; i++)
            {
                if (i == selectedIndex)
                {
                    _frames[i].sprite = _selectedFrame;
                }
                else
                {
                    _frames[i].sprite = _unselectedFrame;
                }

                if (items[i] == null)
                {
                    _items[i].enabled = false;
                }
                else
                {
                    _items[i].enabled = true;
                    _items[i].sprite = items[i].Properties.Icon;
                }
            }
        }
    }
}