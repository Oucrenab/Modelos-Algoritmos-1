using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BossMovement 
{
    [SerializeField]Transform transform;
    [SerializeField]Boss _myBoss;

    [SerializeField]BossMovementType _currentMovement = BossMovementType.None;
    [SerializeField]BossMovementType _lastMovement = BossMovementType.Linear;
    [SerializeField]Action<Vector3> Movement = delegate { };
    [SerializeField]float _speed;
    [SerializeField]float timer;
    [SerializeField]Vector3[] _positions;
    [SerializeField]Vector3 _orbitPosition;
    [SerializeField]int _positionIndex;

    //float _patternWait;
    //float _lastShooting;
    //[SerializeField]float _lastShootgunShoot;
    //[SerializeField]float _shootgunCD = 1f;

    LookUpTable<float, float> _lookUpTableSin;
    LookUpTable<float, float> _lookUpTableCos;


    public BossMovement(Transform newTrans, Boss newBoss)
    {
        transform = newTrans;
        _myBoss = newBoss;

        _lookUpTableCos = new LookUpTable<float, float>(CalculateCos);
        _lookUpTableSin = new LookUpTable<float, float> (CalculateSin);
    }

    public BossMovement SetPositions(Vector3[] positions, Vector3 orbitPositions)
    {
        _positions = positions;
        _orbitPosition = orbitPositions;
        return this;
    }
    public BossMovement SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    }
    

    public void FakeUpdate()
    {
        //if (_currentMovement == BossMovementType.Linear && !InPosition(_positions[_positionIndex]))
        //    Movement(_positions[_positionIndex]);
        if (_currentMovement == BossMovementType.Linear && !InPosition(_currentPosition))
            Movement(_currentPosition);

        if (_currentMovement == BossMovementType.None && Time.time > _myBoss.lastShooting + _myBoss.patternWait)
        {
            _currentMovement = _lastMovement;
            _myBoss.SetCurrentMovement(_currentMovement);

            switch (_currentMovement)
            {
                case BossMovementType.Orbit:
                    Movement = OrbitPosition;
                    break;
                case BossMovementType.Linear:
                    Movement = LinearMovement;
                    break;
                case BossMovementType.None:
                    Movement = delegate { };
                    break;
            }
        }
    }

    public void FakeFixedUpdate()
    {
        
            Movement(_orbitPosition);
        timer += Time.fixedDeltaTime;



    }


    public void StartOrbit()
    {
        //_currentMovement = BossMovement.Orbit;
        _lastMovement = BossMovementType.Orbit;
        //Movement = OrbitPosition;
    }
    public void StartLinear()
    {
        _currentMovement = BossMovementType.Linear;
        _lastMovement = BossMovementType.Linear;
        Movement = LinearMovement;

    }

    void OrbitPosition(Vector3 target)
    {
        //Debug.Log("Orbita");
        //OrbitTimer();
        float num = timer + (Mathf.PI) * _speed;

        var x = target.x + _lookUpTableCos.Calculate(num) * 3;
        var y = target.y + _lookUpTableSin.Calculate(num) * 3 * 0.1f;
        //var x = target.x + MathF.Sin(num) * 3;
        //var y = target.y + MathF.Cos(num) * 3 * 0.1f;

        transform.position = new Vector3(x, y, 0);

        //Debug.Log("Orbitando");
    }

    void LinearMovement(Vector3 pos)
    {
        if (_currentMovement == BossMovementType.None) return;

        var dir = (pos - transform.position).normalized;
        transform.position += dir * _speed * Time.deltaTime;
        //Debug.Log("Lineando");
    }

    bool InPosition(Vector3 pos)
    {
        if (Vector3.Distance(pos, transform.position) < 0.1f)
        {

            transform.position = pos;
            _currentMovement = BossMovementType.None;

            _myBoss.OnPosition();

            Movement = delegate { };

            //_positionIndex++;
            //if (_positionIndex >= _positions.Length) _positionIndex = 0;

            _currentPosition = ChooseRandomPos(_positions);

            return true;
        }

        return false;
    }

    Vector3 _currentPosition;

    Vector3 ChooseRandomPos(Vector3[] positions)
    {
        //return positions.Where(x => x!=_currentPosition)
        //    .Skip(UnityEngine.Random.Range(0, positions.Length-1))
        //    .First();

        System.Random rand = new System.Random();

        var pattern = positions.Where(x => x != _currentPosition)
            .OrderBy(x => rand.Next());

        if (pattern.Any())
            return pattern.First();

        return default(Vector3);
    }

    float CalculateCos(float num)
    {
        return MathF.Cos(num);
    }
    float CalculateSin(float num)
    {
        return MathF.Sin(num);
    }
}
