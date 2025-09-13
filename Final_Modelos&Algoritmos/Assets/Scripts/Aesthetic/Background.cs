using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] SpriteRenderer _background;
    [SerializeField] Transform _bottom;
    [SerializeField] Transform _top;
    [SerializeField] float _speed;
    Vector3 dir = Vector3.up;

    private void FixedUpdate()
    {
        Movement();
    }

    void CheckDist()
    {
        // si llega bottom a border bottom cambiar moviemiento para abajo
        // si llega top a border top cambiar moviemiento para arriba

        if(dir.y >= 0)
        {
            if(_bottom.position.y > BorderManager.Instance.Bottom)
                dir = -transform.up;
        }
        else
        {
            if (_top.position.y < BorderManager.Instance.Top)
                dir = transform.up;
        }
    }

    void Movement()
    {
        transform.position += dir * _speed * Time.fixedDeltaTime;

        CheckDist();
    }
}
