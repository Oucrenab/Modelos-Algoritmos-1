using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMemento
{
    public void Save(params object[] parameters);
    public void Load(params object[] parameters);
    public void MementoSubscribe();
    public void MementoUnsubscribe();

        //EventManager.Subscribe("CallMementoLoad", Load);
        //EventManager.Subscribe("CallMementoSave", Save);
}
