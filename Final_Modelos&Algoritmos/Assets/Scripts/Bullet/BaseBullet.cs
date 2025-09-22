using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseBullet : MonoBehaviour
{
    BulletModel _myModel;
    BulletView _myView;
    ObjectPool<BaseBullet> _pool;

    [SerializeField] Team _myTeam;
    [SerializeField] BulletMovementType _myMovementType;

    [Header("View")]
    [SerializeField] SpriteRenderer _spriteRenderer;

    float _speed = 1;

    //afther linq
    public Team Team { get { return _myTeam; } }

    private void Awake()
    {
        _myModel = new BulletModel(this,transform,_myTeam).SetMovement(_myMovementType, _speed, transform.up);
        _myView = new BulletView(_spriteRenderer);

        //EventManager.Subscribe("OnShieldDestroy", ShieldDestroyed);
        EventManager.Subscribe("MementoLoad", TurnOff);
    }

    private void Update()
    {
        _myModel.FakeUpdate();

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        _myModel.FakeOnTrigerEnter(collision);
        //Debug.Log("a");
    }

    void ShieldDestroyed(params object[] noUse)
    {
        if (_myTeam == Team.Player) return;
        _myModel.OnShieldDestroy();
    }

    public void TurnOff(params object[] noUse)
    {
        _myModel.OnShieldDestroy();
    }

    public BaseBullet SetMovement(BulletMovementType type, float speed, Vector3 dir)
    {
        _myModel.SetMovement(type, speed, dir);
        _myMovementType = type;
        _speed = speed;

        return this;
    }

    public BaseBullet SetTeam(Team team)
    {
        _myTeam = team;
        _myModel.SetTeam(_myTeam);
        _myView.SetTeam(_myTeam);

        return this;
    }
    
    public BaseBullet SetLifeTime(float life)
    {
        _myModel.SetLifeTime(life);

        return this;
    }

    public BaseBullet SetPool(ObjectPool<BaseBullet> pool)
    {
        _pool = pool;
        _myModel.SetPool(pool);

        return this;
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnShieldDestroy", ShieldDestroyed);
        EventManager.Unsubscribe("MementoLoad", TurnOff);

    }
}
