using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{ 
    [SerializeField] Transform _mask;

    private void Update()
    {
        SetMask();
    }

    public void SetMask()
    {
        float xScale = (BorderManager.Instance.Right - BorderManager.Instance.Left);
        float yScale = (BorderManager.Instance.Top - BorderManager.Instance.Bottom);

        if (_mask.localScale.x != xScale || _mask.localScale.y != yScale)
        { 
            _mask.localScale = new Vector3(xScale, yScale);

            float xPos = (BorderManager.Instance.Right + BorderManager.Instance.Left) *0.5f;
            float yPos = (BorderManager.Instance.Top + BorderManager.Instance.Bottom)*0.5f;
            _mask.position = new Vector3(xPos, yPos);
        }

    }
}
