using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnData", menuName = "ScriptableObjects/EnemySpawn")]
public class EnemySpawnDataSO : ScriptableObject
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

    public bool useLifeTime;
    public float lifeTime;
    //a
}

