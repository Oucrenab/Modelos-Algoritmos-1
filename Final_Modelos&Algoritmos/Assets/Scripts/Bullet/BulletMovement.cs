using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement
{
    Action Movement = delegate { };

    Vector3 _velocity;
    Transform transform;

    float _bulletLife = 5;
    float _speed;

    Vector2 _spawnPosition;
    float timer = 0f;

    [SerializeField] float amplitude = 4; //A
    [SerializeField] float frequency = 10; // B
    float offset = 0; // C
    float axis = 0; // D

    // A . sen[B ( X + C)] + D

    public BulletMovement(Transform newTrans)
    {
        transform = newTrans;

        _spawnPosition = transform.position;
    }

    #region Builder
    public BulletMovement SetMovement(BulletMovementType movementType, float speed, Vector2 dir)
    {
        _speed = speed;
        _velocity = dir;

        switch (movementType)
        {
            case BulletMovementType.Linear:
                Movement = LinearMovement;
                break;
            case BulletMovementType.Sen:
                timer = 0;
                Movement = SenMovement;
                break;
            case BulletMovementType.Cos:
                timer = 0;
                Movement = CosMovement;
                break;
            case BulletMovementType.Static:
                Movement = delegate { };
                break;
        }

        return this;
    } 
    #endregion

    public void FakeUpdate()
    {
        if (timer > _bulletLife) timer = 0f;
        timer += Time.deltaTime;

        Movement();
    }

    void LinearMovement()
    {
        //Debug.Log("como la mueve esa mucha chota");

        transform.position += transform.up * _speed * Time.deltaTime;
        transform.up = _velocity.normalized;
    }

    void SenMovement()
    {
        // A . sen[B ( X + C)] + D

        offset = 0f;

        var x = amplitude * MathF.Sin((timer * frequency) + offset) + axis;
        x *= Time.deltaTime;
        var y = _speed * Time.deltaTime;

        transform.position += new Vector3(x, y, 0);
        transform.up = new Vector3(x, y, 0).normalized;
    }

    void CosMovement()
    {
        offset = 0.5f * (float)Math.PI;

        // A . sen[B ( X + C)] + D

        var x = amplitude * MathF.Cos((timer * frequency) + offset) + axis;
        x *= Time.deltaTime;
        var y = _speed * Time.deltaTime;

        transform.position += new Vector3(x, y, 0);
        transform.up = new Vector3(x, y, 0).normalized;
    }
}
