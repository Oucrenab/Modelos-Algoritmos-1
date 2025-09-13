using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModel
{
    Transform transform;
    Team _myTeam;
    ObjectPool<BaseBullet> _pool;
    [SerializeField] BaseBullet _myBullet;

    BulletMovement _myMovement;
    BulletMovementType _myMovementType;

    float _speed;
    float _lifeTime;
    float _timer;

    public BulletModel(BaseBullet myBullet ,Transform newTrans, Team newTeam) 
    {
        _myBullet = myBullet;
        transform = newTrans;
        _myTeam = newTeam;

        _myMovement = new BulletMovement(transform);
        _lifeTime = 5;
        _timer = 0;
    }

    public void FakeUpdate()
    {
        if(_timer > _lifeTime)
        {
            TurnOffBullet();
        }
        _timer += Time.deltaTime;

       _myMovement.FakeUpdate();
    }

    public void FakeOnTrigerEnter(Collider2D collision)
    {
        CheckDamageable(collision.transform);
    }

    void CheckDamageable(Transform other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable) && damageable.GetTeam() != _myTeam)
        {
            TurnOffBullet();
            damageable.GetDamage(1);
        }
    }

    public void OnShieldDestroy()
    {
        TurnOffBullet();
    }

    void TurnOffBullet()
    {

        _timer = 0;

        if(_pool !=null)
        _pool.Return(_myBullet);
    }

    #region Builder
    public BulletModel SetMovement(BulletMovementType movementType, float speed, Vector2 dir)
    {
        _speed = speed;
        _myMovementType = movementType;

        _myMovement.SetMovement(movementType, speed, dir);

        return this;
    } 

    public BulletModel SetTeam(Team team)
    {
        _myTeam = team; 
        return this;
    }
    
    public BulletModel SetLifeTime(float life)
    {
        _lifeTime = life;
        _timer = 0;

        return this;
    }

    public BulletModel SetPool(ObjectPool<BaseBullet> pool)
    {
        _pool = pool;

        return this;
    }
    #endregion



}

public enum BulletMovementType
{
    Linear,
    Sen,
    Cos,
    Static
}
