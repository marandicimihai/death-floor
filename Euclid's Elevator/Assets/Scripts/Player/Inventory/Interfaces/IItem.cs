using System.Collections.Generic;
using UnityEngine;

namespace DeathFloor.Inventory
{
    public interface IItem
    {
        public ItemProperties Properties { get; }
        public void DecreaseDurability(out bool destroyed);
        public GameObject GetRoot();
        public void LoadValues(object[] values);
        public List<string> GetValues();
        public void SetValuesRuntime(IItem valueSource);
        public List<object> GetValuesRuntime();
    }
}