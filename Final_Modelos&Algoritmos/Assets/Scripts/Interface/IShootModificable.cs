using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootModificable
{
    public void ChangeShootType(BulletMovementType type, float speed, float cd, float duration);
}
