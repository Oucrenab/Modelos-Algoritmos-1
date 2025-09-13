using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable, ILifeObservable, IMemento
{
    //mover entre puntos dentro de border
    //disparar patrones del spawner
    //disparar al player patron escopeta
    //disparar balas que al explotar hacen patrones de circulo
    //llamar a enemigos estaticos

    //[SerializeField] BulletPatternData[] _patterns;
    [SerializeField] BulletPatternSO[] _patterns;
    [SerializeField] EnemySpawnData[] _enemyData;
    [SerializeField] EnemySpawner _enemySpawner;
    //List<EnemySpawnData> _currentEnemyData = new List<EnemySpawnData>();
    List<EnemySpawnDataSO> _currentEnemyData = new ();
    [SerializeField] PatternSpawner _patternSpawner;
    [SerializeField] Team _team;
    [SerializeField] Transform _target;

    [SerializeField] float _maxLife;
    float _life;

    [SerializeField] float _speed;
    [SerializeField] Vector3[] _positions;
    [SerializeField] Vector3 _orbitPosition;
    int _positionIndex;

    public Action OnPosition = delegate { };
    Action<Vector3> Movement = delegate { };
    public BossMovementType _currentMovement;
    BossMovementType _lastMovement = BossMovementType.Linear;
    float _patternWait;
    float _lastShooting;
    public float patternWait { get { return _patternWait; } }
    public float lastShooting { get { return _lastShooting; } }

    float _lastShootgunShoot;
    float _shootgunCD = 1f;
    float timer;


    List<ILifeObserver> _lifeObservers = new List<ILifeObserver>();
    //[SerializeField] EnemySpawnData[] _fase2EnemySpawn;
    [SerializeField] EnemySpawnDataSO[] _fase2EnemySpawn;
    //[SerializeField] EnemySpawnData[] _fase3EnemySpawn;
    [SerializeField] EnemySpawnDataSO[] _fase3EnemySpawn;
    float _lastTimeEnemySpawn;
    float _enemySpawnWait = 1;
    bool _canSpawnEnemies = false;
    int _phase = 1;

    [SerializeField] BossMovement _myMovement;

    MementoState _mementoState;

    private void Awake()
    {
        _mementoState = new MementoState();
        _myMovement = new BossMovement(transform, this)
            .SetPositions(_positions, _orbitPosition)
            .SetSpeed(_speed);

        if (_currentMovement == BossMovementType.Linear)
            StartLinear();
        if (_currentMovement == BossMovementType.Orbit)
            StartOrbit();

        OnPosition += ChoosePatter;

        _life = _maxLife;

        Save();
        MementoSubscribe();
    }


    void ClearEnemySpawnList()
    {
        _currentEnemyData.Clear();
    }
    void AddEnemySpawnList(EnemySpawnDataSO data)
    {

        //ClearEnemySpawnList();

        _currentEnemyData.Add(data);
    }

    private void Update()
    {
        #region Movement
        //if (_currentMovement == BossMovementType.Linear && !InPosition(_positions[_positionIndex]))
        //    Movement(_positions[_positionIndex]);

        //if (_currentMovement == BossMovementType.None && Time.time > _lastShooting + _patternWait)
        //{
        //    _currentMovement = _lastMovement;

        //    switch (_currentMovement)
        //    {
        //        case BossMovementType.Orbit:
        //            Movement = OrbitPosition;
        //            break;
        //        case BossMovementType.Linear:
        //            Movement = LinearMovement;
        //            break;
        //        case BossMovementType.None:
        //            Movement = delegate { };
        //            break;
        //    }
        //} 
        #endregion
        if (_myMovement != null)
            _myMovement.FakeUpdate();

        SpawnEnemy();
    }

    private void FixedUpdate()
    {
        if(_currentMovement == BossMovementType.Orbit)
        {
            //Movement(_orbitPosition);
            if (_myMovement != null)
                _myMovement.FakeFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > _lastShootgunShoot + _shootgunCD)
            {
                Shootgun();
            }
        }
    }
    
    void SpawnEnemy()
    {
        if (!_canSpawnEnemies) return;
        if (_currentEnemyData.Count <= 0) return;

        if (Time.time < _lastTimeEnemySpawn + _enemySpawnWait)
            return;
        _lastTimeEnemySpawn = Time.time;

        for (float i = 0; i < _currentEnemyData.Count; i++)
        {
            var data = _currentEnemyData[(int)i];
            _enemySpawner.SetPos(data.targetPos, data.initialPos, data.goToPosition)
                .SetLife(data.life, data.useLifeTime, data.lifeTime)
                .SetShootingData(data.shootCD, data.bulletSpeed, data.trackingType)
                .SetMovement(data.movementType, data.speed, data.orbitRadius, i/_currentEnemyData.Count-1, data.chasePlayer)
                .SetTrackinTarget(_target.GetComponent<ITargeteable>())
                .SpawnEnemy();
            _enemySpawnWait = _currentEnemyData[(int)i].lifeTime + 2;
        }
    }


    void StartOrbit()
    {
        //_currentMovement = BossMovement.Orbit;
        _lastMovement = BossMovementType.Orbit;
        if (_myMovement != null)
            _myMovement.StartOrbit();
        //Movement = OrbitPosition;
    }

    public void SetCurrentMovement(BossMovementType type)
    {
        _currentMovement = type;
    }

    void StartLinear()
    {
        _currentMovement = BossMovementType.Linear;
        _lastMovement = BossMovementType.Linear;
        if (_myMovement != null)
            _myMovement.StartLinear();
        //Movement = LinearMovement;

    }


    //bool InPosition(Vector3 pos)
    //{
    //    if(Vector3.Distance(pos, transform.position) < 0.1f)
    //    {

    //        transform.position = pos;
    //        _currentMovement = BossMovementType.None;

    //        OnPosition();

    //        Movement = delegate { };

    //        _positionIndex++;
    //        if(_positionIndex >= _positions.Length ) _positionIndex = 0;

    //        return true;
    //    }

    //    return false;
    //}

    void ChoosePatter()
    {
        var num = UnityEngine.Random.Range(0, _patterns.Length);
        Shoot(num);
        _patternWait = _patterns[num].duration + 0.1f;
    }

    #region Movement
    //void LinearMovement(Vector3 pos)
    //{
    //    if (_currentMovement == BossMovementType.None) return;

    //    var dir = (pos - transform.position).normalized;
    //    transform.position += dir * _speed * Time.deltaTime;
    //    //Debug.Log("Lineando");
    //}

    //void OrbitPosition(Vector3 target)
    //{
    //    //Debug.Log("Orbita");
    //    OrbitTimer();
    //    float num = timer + (Mathf.PI) * _speed;

    //    //var x = target.x + _lookUpTableSin.Calculate(num) * _orbitRadius * 0.1f;
    //    //var y = target.y + _lookUpTableCos.Calculate(num) * _orbitRadius;
    //    var x = target.x + MathF.Sin(num) * 3 ;
    //    var y = target.y + MathF.Cos(num) * 3 * 0.1f;

    //    transform.position = new Vector3(x, y, 0);

    //    //Debug.Log("Orbitando");
    //}

    //void OrbitTimer()
    //{
    //    timer += Time.fixedDeltaTime;
    //    if(timer > _lastShootgunShoot + _shootgunCD)
    //    {
    //        Shootgun();
    //    }
    //} 
    #endregion

    void Shootgun()
    {
        //Debug.Log("SHOOTGUN");
        _lastShootgunShoot = timer;

        var dir = (_target.position - transform.position).normalized;
        transform.up = dir;
        
        _patternSpawner.transform.position = transform.position;
        _patternSpawner.ShootLinearBullet(transform.up);
        _patternSpawner.ShootLinearBullet(transform.right);
        _patternSpawner.ShootLinearBullet(transform.up + transform.right);
        _patternSpawner.ShootLinearBullet(transform.up * 2+ transform.right * 0.75f);
        _patternSpawner.ShootLinearBullet(-transform.right);
        _patternSpawner.ShootLinearBullet(transform.up - transform.right);
        _patternSpawner.ShootLinearBullet(transform.up * 2 - transform.right * 0.75f);

        transform.up = Vector3.up;
    } 

    void Shoot(int index)
    {
        _lastShooting = Time.time;
        _patternSpawner.transform.position = transform.position;
        _patternSpawner.ShootPattern(_patterns[index]);
    }

    public Team GetTeam()
    {
        return _team;
    }

    public void GetDamage(int amount)
    {
        _life -= amount;

        for (int i = 0; i < _lifeObservers.Count; i++)
        {
            _lifeObservers[i].Notify(_life, _maxLife);
        }

        if (_life < _maxLife * 2 / 3 && _phase <= 1)
        {
            //cambiar a orbitar y spawnear aliados
            //StartOrbit();
            ChangePhase(2);
        }
        if (_life < _maxLife / 3  && _phase <= 2)
        {
            //cambiar a linear y seguir spawneando
            ChangePhase(3);
        }

        if (_life <= 0)
        {
            _life = 0;
            Debug.Log("BossMurio");
            EventManager.Trigger("BossDeath");
            _myMovement = null;
        }
    }


    void ChangePhase(int Phase)
    {
        switch (Phase)
        {
            case 1:
                PhaseOne();
                break;
            case 2:
                PhaseTow();
                break;
            case 3:
                PhaseThree();
                break;
            default:
                Debug.Log($"Boss fase No Valid {Phase}");
                return;
                break;

        }
        EventManager.Trigger("MementoSave");
    }

    void PhaseOne()
    {
        Debug.Log("FASE 1");


        _canSpawnEnemies = false;
        _phase = 1;
        StartLinear();
        EventManager.Trigger("BossFirstPhase");
    }

    void PhaseTow()
    {
        if (_phase >= 2) return;
        Debug.Log("FASE 2");

        _canSpawnEnemies = true;
        //spawneo 8 enemigos que orbitan las esquinas superiores de la arena
        _phase = 2;

        StartOrbit();

        ClearEnemySpawnList();
        for (int i = 0; i < _fase2EnemySpawn.Length; i++)
        {
            //AddEnemySpawnList(_fase2EnemySpawn[i]);
            AddEnemySpawnList(_fase2EnemySpawn[i]);
        }

        EventManager.Trigger("BossSecondPhase");
    }

    void PhaseThree()
    {
        if(_phase >= 3) return;
        Debug.Log("FASE 3");
        _canSpawnEnemies = true;
        //spawneo 4 enemigos estaticos en la parte superior de la arena
        _phase = 3;

        StartLinear();

        ClearEnemySpawnList();
        for (int i = 0; i < _fase3EnemySpawn.Length; i++)
        {
            //AddEnemySpawnList(_fase3EnemySpawn[i]);
            AddEnemySpawnList(_fase3EnemySpawn[i]);
        }
        EventManager.Trigger("BossThirdPhase");
    }

    public void Subscribe(ILifeObserver x)
    {
        if (_lifeObservers.Contains(x)) return;

        _lifeObservers.Add(x);
    }

    public void Unsubscribe(ILifeObserver x)
    {
        if (x == null) return;
        if (_lifeObservers.Contains(x))
            _lifeObservers.Remove(x);

    }

    public void Save(params object[] parameters)
    {
        //life
        //phase
        _mementoState.Rec(_life, _phase);

    }

    public void Load(params object[] parameters)
    {
        if (!_mementoState.IsRemember()) return;

        var remember = _mementoState.Remember();

        _life = (float)remember.parameters[0];
        ChangePhase((int)remember.parameters[1]);

        foreach (var observer in _lifeObservers)
        {
            observer.Notify(_life, _maxLife);
        }

        _lastTimeEnemySpawn = Time.time - (_enemySpawnWait * 0.75f);
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

    private void OnDestroy()
    {
        MementoUnsubscribe();
    }
}

public enum BossMovementType
{
    Orbit,
    Linear,
    None
}
