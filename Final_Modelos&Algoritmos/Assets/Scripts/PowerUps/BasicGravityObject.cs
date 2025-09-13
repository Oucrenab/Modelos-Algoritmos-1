using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGravityObject : MonoBehaviour
{
    //a
    [SerializeField] float _speed;
    PowerUpsSpawner _mySpawner;


    protected virtual void Update()
    {
        Movement(-Vector3.up);
    }

    protected void Movement(Vector3 dir)
    {
        transform.position += dir.normalized * _speed * Time.deltaTime;
        if (transform.position.y < BorderManager.Instance.Bottom - 1)
        {
            TurnOff();
        }
    }



    public void TurnOff()
    {
        if (_mySpawner != null)
            _mySpawner.TurnOff(this);

        gameObject.SetActive(false);
    }

    public BasicGravityObject SetSpawner(PowerUpsSpawner spawner)
    {
        _mySpawner = spawner;
        return this;
    }
}
