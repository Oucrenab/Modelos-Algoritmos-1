using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LookUpTable<T1, T2>
{
    Dictionary<T1, T2> _table = new();
    Func<T1, T2> _func;

    public LookUpTable(Func<T1, T2> func)
    {
        _func = func;
    }
    public T2 Calculate(T1 value)
    {
        if (_table.ContainsKey(value))
            return _table[value];

        var result = _func(value);

        _table[value] = result;
        return result;
    }
}
