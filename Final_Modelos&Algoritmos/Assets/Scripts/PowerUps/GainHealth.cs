using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainHealth : BasicGravityObject, IPowerUp
{
    public void DoPowerUp(Transform target)
    {
        target.GetComponent<IDamageable>().GetDamage(-5);
        TurnOff();
    }
}
