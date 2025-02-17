using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Stack<T> _pool = new();
    private readonly Func<T> _factoryMethod;
    private readonly Action<T> _onTakeFromPool;

    public ObjectPool(Func<T> factoryMethod, Action<T> onTakeFromPool, int initialSize)
    {
        _factoryMethod = factoryMethod;
        _onTakeFromPool = onTakeFromPool;

        for (int i = 0; i < initialSize; i++)
        {
            _factoryMethod();
            //_pool.Push(obj);
        } 
    }
    
    public T Get()
    {
        if (_pool.Count > 0)
        {
            T obj = _pool.Pop();

            if (obj == null)
            {
                return Get();
            }
            else
            {
                _onTakeFromPool?.Invoke(obj);
                return obj;
            }
        }
        else
        {
            return _factoryMethod();
        }
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Push(obj);
    }
}

