using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour, IDamageable, ITargeteable, ILifeObservable, IShootModificable, IShieldable, IMemento
{
    [SerializeField] PlayerModel _myModel;
    [SerializeField] PlayerView _myView;
    [SerializeField] PlayerControl _myControl;

    [SerializeField] float _speed;
    [SerializeField] int _maxLife;

    [SerializeField] Team _myTeam;

    List<ILifeObserver> _lifeObservers = new List<ILifeObserver>();
    public List<ILifeObserver> lifeObservers { get {  return _lifeObservers; } }
    [SerializeField] BulletSpawner _mySpawner;

    [Header("View Refs")]
    [SerializeField] SpriteRenderer _shieldSprite;

    public void GetDamage(int amount)
    {
        //Debug.Log("Player Damaged");

        _myModel.GetDamage(amount, _lifeObservers);

        
    }


    public void ChangeShootType(BulletMovementType type, float speed, float cd, float duration)
    {
        _myModel.ShootingTypeChanged(duration);
        _mySpawner.SetCoolDown(cd)
            .SetSpeed(speed)
            .SetBulletType(type);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Team GetTeam()
    {
        return _myTeam;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Vector3 GetVelocity()
    {
        return _myModel.GetVelocity();
    }

    public void Subscribe(ILifeObserver x)
    {
        if (_lifeObservers.Contains(x)) return;

        _lifeObservers.Add(x);
    }

    public void Unsubscribe(ILifeObserver x)
    {
        if (_lifeObservers.Contains(x))
            _lifeObservers.Remove(x);

    }

    private void Awake()
    {
        _myModel = new PlayerModel(this,transform).SetSpeed(_speed).SetLife(_maxLife);
        _myView = new PlayerView();
        _myControl = new PlayerControl(this);

        MementoSubscribe();
    }

    private void Update()
    {
        _myModel.FakeUpdate();
        _myControl.FakeUpdate();
        _myView.FakeUpdate();


    }

    private void OnDestroy()
    {
        MementoUnsubscribe();
        _myView.FakeOnDestroy();
    }

    public void GetShield()
    {
        _myModel.ShieldUp();
        _myView.SetShild(_shieldSprite , true);
    }

    public void Save(params object[] parameters)
    {
        //vida
        //pos
        _myModel.Save(transform.position);  
    }

    public void Load(params object[] parameters)
    {
        _myModel.Load(transform);
    }

    public void MementoSubscribe()
    {
        EventManager.Subscribe("MementoSave", Save);
        EventManager.Subscribe("MementoLoad", Load);
        
    }

    public void MementoUnsubscribe()
    {
        EventManager.Unsubscribe("MementoSave", Save);
        EventManager.Unsubscribe("MementoLoad", Load);
    }

    [SerializeField] BaseScreen _pauseScreen;

    public void ActivatePause()
    {
        if (_pauseScreen == null) return;
        if (ScreenManager.Instance.ScreenActive) return;

        ScreenManager.Instance.ActivateScreen(_pauseScreen);
        Time.timeScale = 0f;
    }
}
