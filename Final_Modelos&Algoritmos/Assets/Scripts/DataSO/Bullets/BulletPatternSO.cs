using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletPattern", menuName = "ScriptableObjects/BulletPattern")]
public class BulletPatternSO : ScriptableObject
{
    public BulletDataSO bulletData;

    public float cd;
    public float duration;
    public int rotationPerShoot;
    public int bulletsPerShoot;
    public PatternType type;
    //a

}
