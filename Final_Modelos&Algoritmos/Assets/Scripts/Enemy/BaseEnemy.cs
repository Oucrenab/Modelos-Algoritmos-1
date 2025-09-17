using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    EnemyModel _myModel;
    EnemyView _myView;

    [SerializeField] float _speed;
    [SerializeField] float _offset;
    [SerializeField] float _orbitRadius;
    [SerializeField] Team _myTeam;
    [SerializeField] BulletFactory _bulletFactory;
    [SerializeField] ITargeteable _target;

    [SerializeField]EnemyMovementType _movementType;
    [SerializeField] Vector3 _movementTarget;
    [SerializeField] bool _goToPos;
    [SerializeField]TrackingType _trackingType;
    ObjectPool<BaseEnemy> _pool;
    public ObjectPool<BaseEnemy> Pool { get { return _pool; } }

    [SerializeField] bool _useLifeTime;
    [SerializeField] float _lifeTime;

    public Action OnDeath = delegate { };
    [Header("VIEW")]
    [SerializeField] ParticleSystem _deathParticles;

    public virtual void GetDamage(int amount)
    {
        //Debug.Log("Duele");
        _myModel.GetDamage(amount);
    }

    public Team GetTeam()
    {
        return _myTeam;
    }

    protected virtual void Awake()
    {
        //_target = Dev_Target.GetComponent<ITargeteable>();

        _myModel = new EnemyModel(this);
            //.SetBulletFactory(_bulletFactory)
            //.SetTeam(_myTeam)
            //.SetBulletData(1, 2)
            //.SetOrbitData(_orbitRadius, _offset)
            //.SetMovement(_movementType, _speed, _goToPos)
            //.SetTarget(_target)
            //.SetTargetPos(_movementTarget)
            //.SetTracking(_target, _trackingType)
            //.SetLife(4);
        _myView = new EnemyView()
            .SetDeath(this, _deathParticles);

        //OnDeath += _myView.Death;
        EventManager.Subscribe("MementoLoad", TurnOff);

    }

    void TurnOff(params object[] noUse)
    {
        if(_pool != null)
        _pool.Return(this);
    }

    protected void Update()
    {
        _myModel.FakeUpdate();
    }

    protected void FixedUpdate()
    {
        _myModel.FakeFixedUpdate();
    }

    public BaseEnemy SetBulletFactory(BulletFactory bulletFactory)
    {
        _bulletFactory = bulletFactory;
        _myModel.SetBulletFactory(bulletFactory);

        return this;
    }
    
    public BaseEnemy SetTracking(TrackingType type, ITargeteable target)
    {
        _trackingType = type;
        _target = target;

        _myModel.SetTracking(target, type);

        return this;
    }

    public BaseEnemy SetBulletData(float cd, float speed)
    {
        _myModel.SetBulletData(cd, speed);

        return this;
    }

    public BaseEnemy SetTeam(Team team)
    {
        _myTeam = team;
        _myModel.SetTeam(team);

        return this;
    }

    public BaseEnemy SetMovement(EnemyMovementType type, float speed, bool chase,bool goToPosition = false)
    {
        _movementType = type;
        _speed = speed;

        _myModel.SetMovement(type, speed, chase ,goToPosition);

        return this;
    }

    public BaseEnemy SetTarget(ITargeteable target)
    {
        _target = target;
        _myModel.SetTarget(target);

        return this;
    }

    public BaseEnemy SetTargetPos(Vector3 target)
    {
        _movementTarget = target;
        _myModel.SetTargetPos(target);

        return this;
    }

    public BaseEnemy SetOrbitData(float rad, float offset)
    {
        _myModel.SetOrbitData(rad, offset);

        return this;
    }

    public BaseEnemy SetLife(float maxLife)
    {
        _myModel.SetLife(maxLife);

        return this;
    }

    public BaseEnemy SetPool(ObjectPool<BaseEnemy> pool)
    {
        _pool = pool;
        return this;
    }

    public BaseEnemy SetCanShoot(bool canShoot)
    {
        _myModel.SetCanShoot(canShoot);

        return this;
    }
    

    public BaseEnemy SetLifeTime(bool useLifeTime, float time)
    {
        _myModel.SetLifeTime(useLifeTime, time);

        return this;
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("MementoLoad", TurnOff);

    }

    public void CallCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}


public enum EnemyMovementType
{
    Orbit,
    Static,
    Linear,
    
}

public enum TrackingType
{
    PredictTarget,
    Straigh,
    None,
}
