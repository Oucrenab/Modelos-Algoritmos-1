using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class PlayerModel
{
    BasePlayer _myBase;
    [SerializeField] PlayerMovement _myMovement;
    PowerUpCheck _powerUpCheck;
    MementoState _mementoState;


    int _maxLife;
    int _life;

    public PlayerModel(BasePlayer newPlayer ,Transform newTransform)
    {
        _myBase = newPlayer;
        _myMovement = new PlayerMovement(newTransform);
        _powerUpCheck = new PowerUpCheck(newTransform);
        _mementoState = new MementoState();

    }

    public void FakeUpdate()
    {
        _myMovement.FakeUpdate();
        _powerUpCheck.FakeUpdate();

        BulletPowerUpTimer();
    }

    public Vector3 GetVelocity()
    {
        return _myMovement.GetVelocity();
    }

    public void DamageShield()
    {
        _shielded = false;
        Debug.Log("Escudo Roto");
        EventManager.Trigger("OnShieldDestroy");

        GameLists.Instance.BlankEffect(_myBase.transform.position, 3);
    }

    public void GetDamage(int amount, List<ILifeObserver> observers)
    {
        if (_shielded && amount > 0)
        {
            DamageShield();
            return;
        }

        _life -= amount;


        if(_life > _maxLife) _life = _maxLife;
        if(_life <= 0)
        {
            _life = 0;
            Debug.Log("PlayerMurio");
            //EventManager.Trigger("MementoLoad");
            EventManager.Trigger("PlayerDeath");
        }

        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].Notify(_life, _maxLife);
        }
    }

    bool _shootingChanged;
    float _shootPowerUpDuration;
    float _lastChanged;

    public void ShootingTypeChanged(float duration)
    {
        _shootingChanged = true;
        _shootPowerUpDuration = duration;
        _lastChanged = Time.time;
    }
    BulletMovementType type = BulletMovementType.Linear;
    float speed = 10, cd = 0.07f, duration = 0;

    void BulletPowerUpTimer()
    {
        if (!_shootingChanged) return;

        if(Time.time > _lastChanged + _shootPowerUpDuration)
        {
            _myBase.ChangeShootType(type, speed, cd, duration);
            _shootingChanged=false;
        }
    }

    bool _shielded;
    public void ShieldUp()
    {
        Debug.Log("Escudado");
        _shielded = true;
    }


    public void Save(Vector3 pos)
    {
        _mementoState.Rec(_life, pos);
    }

    public void Load(Transform transform)
    {
        if(!_mementoState.IsRemember()) return;

        var remember = _mementoState.Remember();

        _life = (int)remember.parameters[0];
        transform.position = (Vector3)remember.parameters[1];

        foreach(var observer in _myBase.lifeObservers)
        {
            observer.Notify(_life, _maxLife);
        }
    }

    #region Builder
    public PlayerModel SetSpeed(float speed)
    {
        _myMovement.SetSpeed(speed);
        return this;
    } 
    
    public PlayerModel SetLife(int maxLife)
    {
        _maxLife = maxLife;
        _life = _maxLife;

        Save(new Vector3(0, -2, 0));

        return this;
    }
    #endregion
}
