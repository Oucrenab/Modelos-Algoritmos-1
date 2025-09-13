using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : Factory<BaseEnemy>
{
    [SerializeField] ObjectPool<BaseEnemy> _pool;
    [SerializeField] int _initialPoolCount;
    public ObjectPool<BaseEnemy> Pool { get { return _pool; } }

    [SerializeField] BaseEnemy _enemyPrefab;

    private void Awake()
    {
        _pool = new ObjectPool<BaseEnemy>(Create, TurnOn, TurnOff, _initialPoolCount);
        //_turnOnPos = transform;
    }

    public override BaseEnemy Create()
    {
        var enemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);

        return enemy;
    }

    public override void TurnOff(BaseEnemy other)
    {
        other.gameObject.SetActive(false);
    }

    public override void TurnOn(BaseEnemy other)
    {

        other.transform.position = _initialPos;
        other.SetBulletData(_shootCD, _bulletSpeed)
            .SetTargetPos(_pos)
            .SetBulletFactory(_bulletFactory)
            .SetLife(_maxLife)
            .SetLifeTime(_useLifeTime, _lifeTime)
            .SetMovement(_enemyMovementType, _speed, _chaseTarget,_goToPos)
            .SetOrbitData(_rad, _offset)
            .SetTarget(_target)
            .SetTeam(_team)
            .SetTracking(_trackingType, _trackingTarget)
            .gameObject.SetActive(true);

    }

    private EnemyFactory SetAlgo()
    {


        return this;
    }

    float _shootCD;
    float _bulletSpeed;
    float _maxLife;
    float _lifeTime;
    float _speed;
    float _rad;
    float _offset;
    bool _chaseTarget;
    bool _useLifeTime;
    bool _goToPos;
    Vector3 _pos;
    Vector3 _initialPos;
    Team _team;
    EnemyMovementType _enemyMovementType;
    ITargeteable _target;
    ITargeteable _trackingTarget;
    TrackingType _trackingType;
    BulletFactory _bulletFactory;

    public EnemyFactory SetBulletData(float cd, float speed)
    {
        _shootCD = cd;
        _bulletSpeed = speed;

        return this;
    }
    public EnemyFactory SetBulletFactor(BulletFactory bulletFactory)
    {
        _bulletFactory = bulletFactory;
        return this;
    }
    public EnemyFactory SetLife(float life)
    {
        _maxLife = life;

        return this;
    }
    
    public EnemyFactory SetLifeTime(bool useLifeTime, float lifeTime)
    {
        _useLifeTime = useLifeTime;
        _lifeTime = lifeTime;

        return this;
    }


    public EnemyFactory SetMovement(EnemyMovementType type, float speed, bool chase,bool goToPos = false)
    {
        _enemyMovementType = type;
        _speed = speed;
        _goToPos = goToPos;
        _chaseTarget = chase;

        return this;
    }
    public EnemyFactory SetOrbitData(float radius, float offset)
    {
        _rad = radius;
        _offset = offset;

        return this;
    }
    public EnemyFactory SetTarget(ITargeteable target)
    {
        _target = target;

        return this;
    }
    public EnemyFactory SetTargetPos(Vector3 pos)
    {
        _target = null;
        _pos = pos;

        return this;
    }
    public EnemyFactory SetTeam(Team team)
    {
        _team = team;

        return this;
    }
    public EnemyFactory SetTracking(TrackingType type ,ITargeteable target)
    {
        _trackingType = type;
        _trackingTarget = target;

        return this;
    }
    public EnemyFactory SetInitialPos(Vector3 pos)
    {
        _initialPos = pos;

        return this;
    }


}
