using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoState
{
    //List<ParamsMemento> _parameters = new List<ParamsMemento>();
    ParamsMemento _parameters;
    public void Rec(params object[] parameters)
    {
        //if (_parameters.Count > 500)
        //    _parameters.RemoveAt(0);

        //var remember = new ParamsMemento(parameters);
        //_parameters.Add(remember);

        _parameters = new ParamsMemento(parameters);
    }

    public bool IsRemember()
    {
        //return _parameters.Count > 0;
        return _parameters != null;
    }

    public ParamsMemento Remember()
    {
        //var x = _parameters[_parameters.Count - 1];
        //_parameters.RemoveAt(_parameters.Count - 1);

        var x = _parameters;

        return x;
    }
}
