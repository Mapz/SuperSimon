﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMgr<T> : IEnumerable where T : MonoBehaviour, IPause
{
    public delegate T GetObj();
    public static ObjectMgr<T> _Instance;
    private List<T> m_pool = new List<T>();
    public static ObjectMgr<T> Instance
    {
        get
        {
            if (null != _Instance) return _Instance;
            _Instance = new ObjectMgr<T>();
            return _Instance;
        }
    }

    public IEnumerator GetEnumerator()
    {
        return m_pool.GetEnumerator();
    }

    public T Create(GetObj creatAction)
    {
        T newObj = creatAction();
        _Instance.m_pool.Add(newObj);
        return newObj;
    }

    public void Destroy(T obj)
    {
        _Instance.m_pool.Remove(obj);
        UnityEngine.Object.Destroy(obj.gameObject);
    }

    public void PauseAll(bool _pause)
    {
        foreach (T obj in _Instance.m_pool)
        {
            if (null == obj) continue;
            obj.Pause(_pause);
        }
    }

    public void Clear()
    {
        foreach (T obj in _Instance.m_pool)
        {
            if (null != obj)
                UnityEngine.Object.Destroy(obj.gameObject);
        }
        _Instance.m_pool.Clear();
    }
}