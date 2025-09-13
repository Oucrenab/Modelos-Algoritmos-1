using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float _shootCD;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _maxLife;
    [SerializeField] float _lifeTime;
    [SerializeField] float _speed;
    [SerializeField] float _rad;
    [SerializeField] float _offset;
    [SerializeField] bool _goToPos;
    [SerializeField] bool _useLifeTime;
    [SerializeField] Vector3 _pos;
    [SerializeField] Vector3 _initialPos;
    [SerializeField] Team _team;
    [SerializeField] EnemyMovementType _enemyMovementType;
    [SerializeField] Transform _targetObject;
    [SerializeField] ITargeteable _target;
    [SerializeField] ITargeteable _trackingTarget;
    [SerializeField] TrackingType _trackingType;
    [SerializeField] BulletFactory _bulletFactory;
    [SerializeField] EnemyFactory _enemyFactory;
    [SerializeField] bool _chasePlayer;

    [SerializeField] EnemySpawnData Dev_SpawnTest;
    //private void Awake()
    //{
    //    _trackingTarget = _targetObject.GetComponent<ITargeteable>();
    //}

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Space))
    //        SpawnEnemy(Dev_SpawnTest);
    //}
    public EnemySpawner SetTrackinTarget(ITargeteable target)
    {
        _trackingTarget = target;
        return this;
    }
    public EnemySpawner SetShootingData(float shootCd, float bulletSpeed, TrackingType tracking)
    {
        _shootCD = shootCd;
        _bulletSpeed = bulletSpeed;
        _trackingType = tracking;
        return this;
    }
    public EnemySpawner SetLife(float maxLife, bool useLifeTime, float lifeTime)
    {
        _maxLife = maxLife;
        _lifeTime = lifeTime;
        _useLifeTime = useLifeTime;
        return this;
    }
    public EnemySpawner SetMovement(EnemyMovementType movementType, float speed, float radius, float offset, bool chasePlayer)
    {
        _enemyMovementType = movementType;
        _speed = speed;
        _rad = radius;
        _offset = offset;
        _chasePlayer = chasePlayer;
        return this;
    }
    public EnemySpawner SetPos(Vector3 targetPos, Vector3 initialPos, bool goToPos)
    {
        _pos = targetPos;
        _initialPos = initialPos;
        _goToPos = goToPos;
        return this;
    }

    public void SpawnEnemy(/*EnemySpawnData data*/)
    {
        //_trackingTarget = data.target.GetComponent<ITargeteable>();
        //_shootCD = data.shootCD;
        //_bulletSpeed = data.bulletSpeed;
        //_trackingType = data.trackingType;
        //_maxLife = data.life;
        //_useLifeTime = data.useLifeTime;
        //_lifeTime = data.lifeTime;
        //_enemyMovementType = data.movementType;
        //_speed = data.speed;
        //_rad = data.orbitRadius;
        //_offset = data.offset;
        //_chasePlayer = data.chasePlayer;
        //_goToPos = data.goToPosition;
        //_pos = data.targetPos;
        //transform.position = data.initialPos;
        

        _enemyFactory.SetBulletData(_shootCD, _bulletSpeed)
            .SetBulletFactor(_bulletFactory)
            .SetTargetPos(_pos)
            .SetTarget(_trackingTarget)
            .SetInitialPos(_initialPos)
            .SetLife(_maxLife)
            .SetLifeTime(_useLifeTime, _lifeTime)
            .SetMovement(_enemyMovementType, _speed, _chasePlayer, _goToPos)
            .SetOrbitData(_rad, _offset)
            .SetTeam(_team)
            .SetTracking(_trackingType, _trackingTarget)
            .Pool.Get().SetPool(_enemyFactory.Pool);

        //transform.position = Vector3.up * BorderManager.Instance.Top;
    }

}
