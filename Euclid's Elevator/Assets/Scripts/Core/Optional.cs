using System;
using UnityEngine;

namespace DeathFloor.Utilities
{
    [Serializable]
    public struct Optional<T>
    {
        public readonly bool Enabled => _enabled;
        public readonly T Value => _enabled ? _value : default;

        [SerializeField] bool _enabled;
        [SerializeField] T _value;

        public Optional(T value, bool enabled)
        {
            _value = value;
            _enabled = enabled;
        }
    }
}