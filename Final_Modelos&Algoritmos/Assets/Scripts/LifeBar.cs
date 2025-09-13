using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour, ILifeObserver
{
    [SerializeField] Image _bar;
    [SerializeField] GameObject _observable;

    private void Awake()
    {
        if (_observable.GetComponent<ILifeObservable>() != null)
        {
            _observable.GetComponent<ILifeObservable>().Subscribe(this);
            _bar.fillAmount = 1;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Notify(float life, float maxLife)
    {
        _bar.fillAmount = life/maxLife;
    }

    private void OnDestroy()
    {
        if (_observable.GetComponent<ILifeObservable>() != null)
            _observable.GetComponent<ILifeObservable>().Unsubscribe(this);
    }
}
