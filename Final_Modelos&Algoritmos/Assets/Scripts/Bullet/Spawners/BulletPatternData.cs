using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BulletPatternData
{
    public float bulletSpeed;
    public float bulletLifeTime;

    public float cd;
    public float duration;
    public int rotationPerShoot;
    public int bulletsPerShoot;
    public PatternType type;
}

public enum PatternType
{
    D8,
    D4,
    CustomAmount,
    Flower
}
