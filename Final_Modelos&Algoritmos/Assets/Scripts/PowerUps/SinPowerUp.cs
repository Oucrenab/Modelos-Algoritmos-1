using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinPowerUp : BasicGravityObject, IPowerUp
{
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _cd;
    [SerializeField] float _duration;

    public void DoPowerUp(Transform target)
    {
        target.GetComponent<IShootModificable>().ChangeShootType(BulletMovementType.Sen, _bulletSpeed, _cd, _duration);
        TurnOff();
    }
}
