using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement
{
    Transform transform;
    float timer;
    float _offset;
    float _speed;
    float _orbitRadius;

    bool _chasePlayer;

    Action<Vector3> Movement = delegate { };
    Vector3 _targetMovement;

    EnemyMovementType _movementType;

    ITargeteable _targetPos;

    LookUpTable<float, float> _lookUpTableSin;
    LookUpTable<float, float> _lookUpTableCos;

    public EnemyMovement(Transform newTrans)
    {
        transform = newTrans;

        timer = 0;
        _offset = 0;
        _speed = 1;
        _orbitRadius = 2;

        _lookUpTableCos = new LookUpTable<float, float>(CalculateCos);
        _lookUpTableSin = new LookUpTable<float, float>(CalculateSin);
    }

    public void FakeFixedUpdate()
    {
        if (_chasePlayer) _targetMovement = _targetPos.GetPosition();
        timer += Time.fixedDeltaTime;
        if (timer > Math.PI * 2) timer = 0;


        Movement(_targetMovement);
    }

    float CalculateCos(float num)
    {
        return Mathf.Cos(num);
    }

    float CalculateSin(float num)
    {
        return Mathf.Sin(num);
    }

    void OrbitPosition(Vector3 target)
    {
        //Debug.Log("Orbita");

        float num = timer + (Mathf.PI * _offset) * _speed;

        var x = target.x + _lookUpTableSin.Calculate(num) * _orbitRadius;
        var y = target.y + _lookUpTableCos.Calculate(num) * _orbitRadius;

        transform.position = new Vector3(x, y, 0);
    }

    void LinearMovement(Vector3 direction)
    {
        //Debug.Log("Linear");


        transform.position += direction.normalized * _speed * Time.deltaTime;
    }

    void MoveToPosition(Vector3 pos)
    {
        //Debug.Log("Posicion");


        var dir  = (pos - transform.position).normalized;

        LinearMovement(dir);

        if(_movementType == EnemyMovementType.Orbit)
        {
            if (Vector3.Distance(transform.position, pos) < _orbitRadius)
            {
                //Debug.Log("Llegue a destino");
                SetMovement(_movementType, _speed, _chasePlayer,false);
            }
        }
        else 
        {
            if (Vector3.Distance(transform.position, pos) < 0.1f)
            {
                //Debug.Log("Llegue a destino");
                SetMovement(_movementType, _speed, _chasePlayer,false);
            }
        }
    }

    public EnemyMovement SetMovement(EnemyMovementType type, float speed, bool chase, bool goToPosition = false)
    {
        _movementType = type;
        _speed = speed;
        timer = 0;
        _chasePlayer = chase;

        if(goToPosition)
        {
            Movement = MoveToPosition;
            return this;
        }

        switch (_movementType)
        {
            case EnemyMovementType.Orbit:
                Movement = OrbitPosition;
                break;
            case EnemyMovementType.Static:
                Movement = delegate { };
                break;
            case EnemyMovementType.Linear:
                Movement = LinearMovement;
                break;
        }

        return this;
    }

    public EnemyMovement SetOrbitData(float rad, float offset)
    {
        _orbitRadius = rad;
        _offset = offset;

        return this;
    }

    public EnemyMovement SetTarget(ITargeteable target)
    {
        _targetPos = target;

        return this;
    }
    public EnemyMovement SetTargetPos(Vector3 movementTarget)
    {
        _targetPos = null;
        _targetMovement = movementTarget;

        return this;
    }
}
