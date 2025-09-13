using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ObjectPool<T>
{
    [SerializeField] public List<T> _stock = new List<T>();
    protected Func<T> _Factory;
    protected Action<T> _On;
    protected Action<T> _Off;
    public ObjectPool(Func<T> Factory, Action<T> ObjOn, Action<T> ObjOff, int currentStock = 5)
    {
        _Factory = Factory;
        _On = ObjOn;
        _Off = ObjOff;

        for (int i = 0; i < currentStock; i++)
        {
            var x = _Factory();
            _Off(x);
            _stock.Add(x);
        }
    }

    public T Get()
    {
        //Debug.Log("Get del Object pool llamado");

        T x;

        if (_stock.Count > 0)
        {
            //Debug.Log("ehhhhhhhhhhhhhhhhh");

            x = _stock[0];
            _stock.Remove(x);
        }
        else
        {
            //Debug.Log("ahhhhhhhhhhhhhhhhh");

            x = _Factory();
        }

        //Debug.Log("Antes del on");


        _On(x);

        //Debug.Log("Get del Object pool termino");

        return x;
    }

    public void Return(T obj)
    {
        _Off(obj);
        _stock.Add(obj);
    }
}
