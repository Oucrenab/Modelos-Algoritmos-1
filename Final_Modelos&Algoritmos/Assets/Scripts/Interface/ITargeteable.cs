using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargeteable
{
    public Vector3 GetVelocity();

    public Vector3 GetPosition();

    public Transform GetTransform();
}
