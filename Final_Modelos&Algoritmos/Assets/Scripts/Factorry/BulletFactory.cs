using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : Factory<BaseBullet>
{
    [SerializeField] ObjectPool<BaseBullet> _pool;
    [SerializeField] int _initialPoolCount;
    public ObjectPool<BaseBullet> Pool { get { return _pool; } }

    [SerializeField] BaseBullet _bulletPrefab;

    BulletMovementType _type;
    float _speed;
    float _lifeTime;
    Vector3 _dir;
    Team _team;
    Transform _turnOnPos;

    private void Awake()
    {
        _pool = new ObjectPool<BaseBullet>(Create, TurnOn, TurnOff, _initialPoolCount);
        _turnOnPos = transform;
    }

    public override BaseBullet Create()
    {
        BaseBullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);

        return bullet;
    }

    public override void TurnOff(BaseBullet other)
    {
        other.gameObject.SetActive(false);
    }

    public override void TurnOn(BaseBullet other)
    {
        TurnOn(other, _type, _speed, _dir, _team, _lifeTime);
    }

    void TurnOn(BaseBullet other, BulletMovementType type, float speed, Vector3 dir, Team team, float life)
    {
        other.SetMovement(type, speed, dir);
        other.SetTeam(team);
        other.SetLifeTime(life);
        other.transform.position = _turnOnPos.position;

        other.gameObject.SetActive(true);
    }

    public BulletFactory SetTeam(Team team)
    {
        _team = team;
        return this;
    }
    
    public BulletFactory SetLifeTime(float life)
    {
        _lifeTime = life;
        return this;
    }

    public BulletFactory SetMovement(BulletMovementType type, float speed, Vector3 dir)
    {
        _type = type;
        _speed = speed;
        _dir = dir;

        return this;
    }

    public BulletFactory SetSpawnPos(Transform pos)
    {
        _turnOnPos = pos;

        return this;
    }
}
