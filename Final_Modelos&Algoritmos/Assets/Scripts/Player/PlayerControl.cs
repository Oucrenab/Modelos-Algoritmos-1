using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class PlayerControl
{
    BasePlayer _myBase;
    public PlayerControl(BasePlayer myBase)
    {
        _myBase = myBase;
    }

    public void FakeUpdate()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
            _myBase.ActivatePause();

        GetMovementInput();
    }

    void GetMovementInput()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        var dir = new Vector3(x, y).normalized;

        EventManager.Trigger("SetMovementInput", dir);
    }
}
