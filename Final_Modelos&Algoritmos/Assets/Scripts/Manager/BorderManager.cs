using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderManager : MonoBehaviour
{
    [SerializeField] float _left;
    [SerializeField] float _right;
    [SerializeField] float _top;
    [SerializeField] float _bottom;

    public static BorderManager Instance;

    public float Left {  get { return _left; } }
    public float Right { get { return _right;} }
    public float Top { get { return _top;} }
    public float Bottom { get { return _bottom;} }


    private void Awake()
    {
        Instance = this;
    }

    public bool OutOfBounce(float x, float y)
    {
        if (x < _left || x > _right)
        {
            //Debug.Log($"Out of bounce: Left {_left} Right {_right} Position {x}");
            return true;
        }
        if (y > _top || y < _bottom)
        {
            //Debug.Log($"Out of bounce: Top {_top} Bottom {_bottom} Position {y}");
            return true;
        }

        return false;
    }
}
