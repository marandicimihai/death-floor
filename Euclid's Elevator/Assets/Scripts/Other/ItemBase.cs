using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public void SetValues(ItemBase variables)
    {
        List<object> values = variables.GetValues();
        int i = 0;
        foreach (FieldInfo field in GetFields())
        {
            field.SetValue(this, values[i]);
            i++;
        }
    }

    //gets values as objects, depends on GetFields()
    public List<object> GetValues()
    {
        FieldInfo[] fields = GetType().GetFields();
        List<object> values = new();

        foreach (FieldInfo field in fields)
        {
            values.Add(field.GetValue(this));
        }

        return values;
    }

    //gets values as FieldInfos
    List<FieldInfo> GetFields()
    {
        FieldInfo[] fields = GetType().GetFields();
        List<FieldInfo> values = new();

        foreach (FieldInfo field in fields)
        {
            values.Add(field);
        }

        return values;
    }
}