using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PatternSpawner : BulletSpawner
{
    [SerializeField] Transform inverseTransform;
    [SerializeField] float _patternDuration;
    [SerializeField] float _lifeTime = 6;
    [SerializeField, Range(0,360)] int _rotation;
    [SerializeField] int _bulletsPerShoot;

    LookUpTable<float, float> _lookUpTableSin;
    LookUpTable<float, float> _lookUpTableCos;

    Action CurrentPattern = delegate { };

    private void Start()
    {
        //Spawn = FlowerPattern;

        _lookUpTableCos = new LookUpTable<float, float>(CalculateCos);
        _lookUpTableSin = new LookUpTable<float, float>(CalculateSin);

        //StartCoroutine(Shoot4D(_patternDuration, _cd, _rotation));
        //Debug.Log("Pattern Hell");
        //CurrentPattern = ShootCustomAxis;
        //bulletsPerShoot = 16;
        //_patternDuration = 4;
        //_cd = .1f;
        //_rotation = 15;
        //StartCoroutine(StartPattern(CurrentPattern, _patternDuration, _cd, _rotation));

        //EventManager.Subscribe("MementoLoad", StopPattern);
    }

    //private void OnDestroy()
    //{
    //    EventManager.Unsubscribe("MementoLoad", StopPattern);
        
    //}

    protected override void FixedUpdate()
    {
    }

    bool _alreadyShooting= false;
    public bool alreadyShooting { get { return _alreadyShooting;} }

    //public void ShootPattern(BulletPatternData myPattern)
    //{
    //    if (!_alreadyShooting)
    //    {
    //        SetPattern(myPattern);

    //        StartCoroutine(StartPattern(CurrentPattern, _patternDuration, _cd, _rotation));
    //    }
    //}

    //IEnumerator _currentRoutine;
    public void ShootPattern(BulletPatternSO myPattern)
    {
        if (!_alreadyShooting)
        {
            SetPattern(myPattern);

            //_currentRoutine = StartPattern(CurrentPattern, _patternDuration, _cd, _rotation);
            StartCoroutine(StartPattern(CurrentPattern, _patternDuration, _cd, _rotation));
        }
    }

    //public void StopPattern(params object[] noUse)
    //{
    //    if (_alreadyShooting)
    //    {
            
    //    StopCoroutine(_currentRoutine);
    //    }
    //    //a

    //}

    //public void SetPattern(BulletPatternData myPattern)
    //{
    //    _cd = myPattern.cd;
    //    _speed = myPattern.bulletSpeed;
    //    _patternDuration = myPattern.duration;
    //    _lifeTime = myPattern.bulletLifeTime;
    //    _rotation = myPattern.rotationPerShoot;
    //    _bulletsPerShoot = myPattern.bulletsPerShoot;

    //    switch (myPattern.type)
    //    {
    //        case PatternType.D8:
    //            CurrentPattern = Circle8D;
    //            break;
    //        case PatternType.D4:
    //            CurrentPattern = Circle4D;
    //            break;
    //        case PatternType.CustomAmount:
    //            CurrentPattern = ShootCustomAxis;
    //            break;
    //        case PatternType.Flower:
    //            CurrentPattern = FlowerPattern;
    //            break;
    //        default:
    //            CurrentPattern = delegate { };
    //            break;
    //    }
    //}

    public void SetPattern(BulletPatternSO myPattern)
    {
        _speed = myPattern.bulletData.bulletSpeed;
        _lifeTime = myPattern.bulletData.bulletLifeTime;
        _cd = myPattern.cd;
        _patternDuration = myPattern.duration;
        _rotation = myPattern.rotationPerShoot;
        _bulletsPerShoot = myPattern.bulletsPerShoot;

        switch (myPattern.type)
        {
            case PatternType.D8:
                CurrentPattern = Circle8D;
                break;
            case PatternType.D4:
                CurrentPattern = Circle4D;
                break;
            case PatternType.CustomAmount:
                CurrentPattern = ShootCustomAxis;
                break;
            case PatternType.Flower:
                CurrentPattern = FlowerPattern;
                break;
            default:
                CurrentPattern = delegate { };
                break;
        }
    }

    public IEnumerator StartPattern(Action ShootPattern, float duration, float shootRate, int rotation)
    {
        _alreadyShooting = true;
        float timer = 0;

        while (timer < duration)
        {
            ShootPattern();
            Rotate(rotation);

            timer += shootRate;
            yield return new WaitForSeconds(shootRate);
        }

        _alreadyShooting = false;
        transform.rotation = Quaternion.identity;
    }

    //public IEnumerator ShootCircle8D(float duration, float shootRate, float rotation)
    //{
    //    float timer = 0;
    //    while (timer < duration)
    //    {
    //        Circle8D();
    //        Rotate((int)(rotation));

    //        timer += shootRate;
    //        yield return new WaitForSeconds(shootRate);
    //    }

    //    transform.rotation = Quaternion.identity;
    //}

    //public IEnumerator Shoot4D(float duration, float shootRate, float rotation)
    //{
    //    float timer = 0;
    //    while (timer < duration)
    //    {
    //        //Circle4D();
    //        ShootCustomAxis();

    //        Rotate((int)(rotation));

    //        timer += shootRate;
    //        yield return new WaitForSeconds(shootRate);
    //    }

    //    transform.rotation = Quaternion.identity;
    //}

    void Rotate(int amount)
    {
        //transform.Rotate(new Vector3(0, 0, transform.rotation.z + amount));

        transform.rotation *= Quaternion.Euler(0,0,(int)(transform.rotation.z + amount));

        inverseTransform.position = transform.position;
        //inverseTransform.Rotate(new Vector3(0, 0, inverseTransform.rotation.z - amount));
        inverseTransform.rotation *= Quaternion.Euler(0,0,(int)(inverseTransform.rotation.z - amount));
    }
    float CalculateSin(float num)
    {
        //Debug.Log($"SenoCalculado {num} {Mathf.Sin(num)}");
        return MathF.Sin(num);
    }
    float CalculateCos(float num)
    {
        //Debug.Log($"CosenoCalculado {num} {Mathf.Cos(num)}");
        return MathF.Cos(num);
    }

    void ShootCustomAxis()
    {
        Vector3 finalDir = Vector3.zero;
        var tempDir = Vector3.zero;
        for (float i = 0; i < _bulletsPerShoot; i++)
        {
            //Debug.Log(i + 1);
            var num = (i/_bulletsPerShoot) * (2 * Mathf.PI);
            var x = transform.position.x + _lookUpTableSin.Calculate(num);
            var y = transform.position.y + _lookUpTableCos.Calculate(num);

            tempDir= new Vector3(x, y, 0);

            finalDir = (tempDir - transform.position).normalized;

            //var desireAngle = (360 / axisAmount) * i + (180 + transform.rotation.z) * (2 * Mathf.PI) - Mathf.PI * 360;
            var z = transform.rotation.z < 0 ? 180+transform.rotation.z : transform.rotation.z;
            var desireAngle = ((360 * i / _bulletsPerShoot) + z * Mathf.PI) % 360;
            //Debug.Log($"Angle {desireAngle}");

            var cos = _lookUpTableCos.Calculate(desireAngle * MathF.PI);
            var sin = _lookUpTableSin.Calculate(desireAngle * MathF.PI);

            var vectorA = new Vector3(cos, sin);
            var vectorB = new Vector3(-sin, cos);

            finalDir = (finalDir.x * vectorA + finalDir.y * vectorB).normalized;

            ShootLinearBullet(finalDir);
        }
    }

    public void ShootLinearBullet(Vector3 dir)
    {
        //Debug.Log("Linear");
        _bulletFactory.SetTeam(_team)
                .SetSpawnPos(transform)
                .SetLifeTime(_lifeTime)
                .SetMovement(BulletMovementType.Linear, _speed, dir.normalized).Pool.Get()
                .SetPool(_bulletFactory.Pool);
    }

    protected void Circle8D()
    {
        dir = transform.up;

        //_bulletFactory.SetMovement(BulletMovementType.Linear, _speed, dir).Pool.Get().SetPool(_bulletFactory.Pool);
        //_bulletFactory.SetMovement(BulletMovementType.Linear, _speed, -dir).Pool.Get().SetPool(_bulletFactory.Pool);
        ShootLinearBullet(dir);
        ShootLinearBullet(-dir);

        dir = transform.right;

        ShootLinearBullet(dir);
        ShootLinearBullet(-dir);

        dir = transform.right + transform.up;
        dir.Normalize();

        ShootLinearBullet(dir);
        ShootLinearBullet(-dir);

        dir = transform.right - transform.up;
        dir.Normalize();

        ShootLinearBullet(dir);
        ShootLinearBullet(-dir);
    }

    protected void Circle4D()
    {

        dir = transform.up;

        ShootLinearBullet(dir);
        ShootLinearBullet(-dir);

        dir = transform.right;

        ShootLinearBullet(dir);
        ShootLinearBullet(-dir);
    }

    void FlowerPattern()
    {
        
        Rotate(_rotation);
        
        dir = transform.up;

        ShootLinearBullet(dir);
        ShootLinearBullet(-dir);

        dir = transform.right;

        ShootLinearBullet(dir);
        ShootLinearBullet(-dir);

        dir = inverseTransform.up;

        ShootLinearBullet(dir);
        ShootLinearBullet(-dir);

        dir = inverseTransform.right;

        ShootLinearBullet(dir);
        ShootLinearBullet(-dir);
    }
}
