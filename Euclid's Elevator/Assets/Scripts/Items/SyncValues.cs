using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

public abstract class SyncValues : MonoBehaviour
{
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

    public List<object> GetSaveVariables()
    {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        List<object> values = new();

        foreach (FieldInfo field in fields)
        {
            if (field.GetCustomAttribute<SaveValueAttribute>() != null)
            {
                values.Add(field.GetValue(this));
            }
        }

        return values;
    }

    public void SetValues(SyncValues variables)
    {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        List<object> values = variables.GetValues();
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

    public List<object> GetValues()
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