using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyModel
{
    BaseEnemy _myBase;
    Transform transform;
    BulletFactory _bulletFactory;

    [SerializeField] Team _team;
    [SerializeField] float _speed;

    ITargeteable _target;
    ITargeteable _trackingTarget;

    float _maxLife;
    float _life;
    float _cd;
    float _bulletSpeed;
    float _lastShoot;

    EnemyMovement _myMovement;
    EnemyMovementType _myMovementType;

    float _lifeTime;
    bool _useLifeTime;

    Action<ITargeteable> Tracking = delegate { };

     public float Life { get { return _life; } }
    public EnemyModel(BaseEnemy enemy)
    {
        _myBase = enemy;
        transform = _myBase.transform;

        _myMovement = new EnemyMovement(transform);
    }

    float _timer;
    public void FakeUpdate()
    {
        

        Tracking(_trackingTarget);

        if (Time.time > _lastShoot + _cd)
            Shoot();

        //if (_useLifeTime)
        //{
        //    _timer += Time.deltaTime;
        //    if (_timer > _lifeTime)
        //        _myBase.CallCoroutine(TurnOff());
        //        //TurnOff();
        //}
    }

    public void FakeFixedUpdate()
    {
        _myMovement.FakeFixedUpdate();
    }

    public EnemyModel SetAlgo()
    {
        return this;
    }
    
    public EnemyModel SetTracking(ITargeteable target,TrackingType tracking)
    {
        _trackingTarget = target;

        switch (tracking)
        {
            case TrackingType.PredictTarget:
                Tracking = PredictTarget;
                break;
            case TrackingType.Straigh:
                Tracking = TrackTarget;
                break;
            case TrackingType.None:
                Tracking = delegate { };
                break;
        }

        return this;
    }
    public EnemyModel SetTeam(Team team)
    {
        _team = team;
        return this;
    }
    public EnemyModel SetBulletData(float cd, float speed)
    {
        _cd = cd;
        _bulletSpeed = speed;

        return this;
    }
    public EnemyModel SetBulletFactory(BulletFactory bulletFactory)
    {
        _bulletFactory = bulletFactory;
        return this;
    }
    public EnemyModel SetMovement(EnemyMovementType type, float speed, bool chasePlayer,bool goToPosition = false)
    {
        _myMovementType = type;
        _speed = speed;

        _myMovement
            .SetMovement(type, speed, chasePlayer, goToPosition);

        return this;
    }
    public EnemyModel SetTargetPos(Vector3 movementTarget)
    {
        _myMovement.SetTargetPos(movementTarget);

        return this;
    }
    public EnemyModel SetTarget(ITargeteable targeteable)
    {
        _target = targeteable;
        _myMovement.SetTarget(targeteable);

        return this;
    }
    public EnemyModel SetOrbitData(float rad, float offset)
    {
        _myMovement
            .SetOrbitData(rad,offset);

        return this;
    }

    public EnemyModel SetLife(float maxLife)
    {
        _maxLife = maxLife;
        _life = maxLife;

        return this;
    }

    public EnemyModel SetLifeTime(bool useLifeTime, float lifeTime)
    {
        _useLifeTime = useLifeTime;
        _lifeTime = lifeTime;
        _timer = 0;


        return this;
    }

    public void GetDamage(int amount)
    {
        _life -= amount;
        //if (_life < 0)
        //{
        //    Death();
        //}
    }

    public void Death()
    {
        _myBase.CallCoroutine(TurnOff());
    }
    //Martin
    IEnumerator TurnOff()
    {
        _myBase.OnDeath();

        yield return new WaitForSeconds(0.5f);
        _myBase.Pool.Return(_myBase);
    }

    bool _canShoot = false;

    public EnemyModel SetCanShoot(bool canShoot)
    {
        //Debug.Log(canShoot);

        _canShoot = canShoot;
        return this;
    }

    protected void Shoot()
    {
        //Debug.Log(transform.position.x + " " + transform.position.y);
        //if (BorderManager.Instance.OutOfBounce(transform.position.x, transform.position.y)) 
        //    return;
        if (!_canShoot) return;
        //Debug.Log("PEW");
        _lastShoot = Time.time;
        LinearBullet(transform.up);
    }
    protected void TrackTarget(ITargeteable target)
    {
        if (target == null)
            return;

        transform.up = (target.GetPosition() - transform.position).normalized;
    }
    protected void PredictTarget(ITargeteable target)
    {
        if(target == null)
            return;

        var dist = Vector3.Magnitude(target.GetPosition() + target.GetVelocity());

        var dir = (target.GetPosition() + target.GetVelocity()*dist);
        transform.up = (dir - transform.position).normalized;
    }

    protected void LinearBullet(Vector3 dir)
    {
        _bulletFactory.SetTeam(_team)
            .SetSpawnPos(transform)
            .SetLifeTime(6)
            .SetMovement(BulletMovementType.Linear, _bulletSpeed, dir).Pool.Get().SetPool(_bulletFactory.Pool);
    }
}
