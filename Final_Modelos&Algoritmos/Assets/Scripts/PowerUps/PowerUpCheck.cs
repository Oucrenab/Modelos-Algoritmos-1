using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCheck
{
    Transform transform;

    public PowerUpCheck(Transform newTransform)
    {
        transform = newTransform;
    }

    public void FakeUpdate()
    {
        CheckForPowerUps();
    }

    void CheckForPowerUps()
    {
        var allObjects = Physics2D.CircleCastAll(transform.position, 0.5f, Vector3.forward);
        for(int i = 0; i < allObjects.Length; i++) 
        {
            if (allObjects[i].transform.TryGetComponent<IPowerUp>(out var powerUp))
                powerUp.DoPowerUp(transform);
        }
    }
}
