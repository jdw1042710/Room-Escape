using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    private List<KeyData> _keys;

    private void Awake()
    {
        _keys = new List<KeyData>();
    }

    public void AddKey(KeyData key)
    {
        _keys.Add(key);
    }

    public bool HasKey(KeyData key)
    {
        return _keys.Contains(key);;
    }

    public void UseKey(KeyData key)
    {
        _keys.Remove(key);
    }
}
