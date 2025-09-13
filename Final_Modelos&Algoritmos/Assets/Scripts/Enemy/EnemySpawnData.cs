using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemySpawnData
{
    public float shootCD;
    public float bulletSpeed;
    public float bulletLifeTime;

    //factory from the spawner
    //position from the spawner

    public float life;

    public EnemyMovementType movementType;
    public float speed;
    public bool goToPosition;
    public bool chasePlayer;
    public Vector3 targetPos;
    public Vector3 initialPos;

    public float orbitRadius;
    public float offset;

    //team from the spawner

    public TrackingType trackingType;
    public Transform target;

    public bool useLifeTime;
    public float lifeTime;
}
