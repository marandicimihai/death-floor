using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

namespace DeathFloor.Inventory
{
    internal abstract class SyncValues : MonoBehaviour
    {
        /// <summary>
        /// Strings also work.
        /// </summary>
        /// <param name="values"></param>
        public void LoadValues(object[] values)
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            int i = 0;
            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttribute<SaveValueAttribute>() != null)
                {
                    field.SetValue(this, Convert.ChangeType(values[i], field.FieldType));
                    i++;
                }
            }
        }

        public List<string> GetValues()
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            List<string> values = new();

            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttribute<SaveValueAttribute>() != null)
                {
                    values.Add(field.GetValue(this).ToString());
                }
            }

            return values;
        }

        public void SetValuesRuntime(SyncValues variables)
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            List<object> values = variables.GetValuesRuntime();
            int i = 0;
            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttribute<SyncValueAttribute>() != null)
                {
                    field.SetValue(this, values[i]);
                }
                i++;
            }
        }

        public List<object> GetValuesRuntime()
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            List<object> values = new();

            foreach (FieldInfo field in fields)
            {
                values.Add(field.GetValue(this));
            }

            return values;
        }
    }
}