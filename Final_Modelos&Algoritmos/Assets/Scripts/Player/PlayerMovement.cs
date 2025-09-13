using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerMovement
{
    Transform transform;
    [SerializeField] Vector3 _velocity;
    LayerMask _obstacle;

    [SerializeField]float _speed;

    Action Movement = delegate { };

    public PlayerMovement(Transform newTransform)
    {
        transform = newTransform;

        Movement = NormalMovement;
        _speed = 1f;
        _velocity = Vector3.zero;

        EventManager.Subscribe("SetMovementInput", SetVelocity);
    }

    public void FakeUpdate()
    {
        
        Movement();
    }

    #region CheckForObstacle NoFunca
    //void CheckForObstacle()
    //{
    //    Debug.DrawRay(transform.position, Vector2.left * 0.5f, Color.blue);
    //    Debug.DrawRay(transform.position, Vector2.right * 0.5f, Color.green);
    //    Debug.DrawRay(transform.position, Vector2.up * 0.5f, Color.red);
    //    Debug.DrawRay(transform.position, Vector2.down * 0.5f, Color.magenta);

    //    if (_velocity == Vector3.zero) return;

    //    if (Physics.Raycast(transform.position, Vector2.left * 0.5f, out var hit, _obstacle) 
    //        && hit.transform.gameObject.GetComponent<IBlockMovent>() != null)
    //    {
    //        Debug.Log(hit.transform.gameObject.name);
    //        if(_velocity.x < 0f) _velocity.x = 0f;          
    //    }
    //    if (Physics.Raycast(transform.position, Vector2.right * 0.5f, out hit, _obstacle)
    //        && hit.transform.gameObject.GetComponent<IBlockMovent>() != null)
    //    {
    //        Debug.Log(hit.transform.gameObject.name);

    //        if (_velocity.x > 0f) _velocity.x = 0f;
    //    }
    //    if (Physics.Raycast(transform.position, Vector2.up * 0.5f, out hit, _obstacle)
    //        && hit.transform.gameObject.GetComponent<IBlockMovent>() != null)
    //    {
    //        Debug.Log(hit.transform.gameObject.name);

    //        if (_velocity.y > 0f) _velocity.y = 0f;
    //    }
    //    if (Physics.Raycast(transform.position, Vector2.down * 0.5f, out hit, _obstacle)
    //        && hit.transform.gameObject.GetComponent<IBlockMovent>() != null)
    //    {
    //        Debug.Log(hit.transform.gameObject.name);

    //        if (_velocity.y < 0f) _velocity.y = 0f;
    //    }


    //} 
    #endregion

    void WorldBorder()
    {
        if (_velocity == Vector3.zero) return;

        if (transform.position.x < BorderManager.Instance.Left && _velocity.x < 0) _velocity.x = 0;
        if (transform.position.x > BorderManager.Instance.Right && _velocity.x > 0) _velocity.x = 0;
        if (transform.position.y < BorderManager.Instance.Bottom && _velocity.y < 0) _velocity.y = 0;
        if (transform.position.y > BorderManager.Instance.Top && _velocity.y > 0) _velocity.y = 0;
    }

    void NormalMovement()
    {
        //Debug.Log(_velocity);
        WorldBorder();
        transform.position += _velocity.normalized * _speed * Time.deltaTime;
    }

    void SetVelocity(params object[] parameters)
    {
        var dir = (Vector3)parameters[0];

        _velocity = dir;
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }

    #region Builder
    public PlayerMovement SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    } 


    #endregion
}
