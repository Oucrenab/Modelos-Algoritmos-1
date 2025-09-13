using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUp : BasicGravityObject, IPowerUp
{
    public void DoPowerUp(Transform target)
    {
        target.GetComponent<IShieldable>().GetShield();
        TurnOff();
    }
}
