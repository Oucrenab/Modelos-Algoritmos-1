using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class PlayerView
{
    SpriteRenderer _shieldSprite;

    public PlayerView()
    {
        EventManager.Subscribe("OnShieldDestroy", DestroyShield);
    }

    public void FakeUpdate()
    {

    }

    public void FakeOnDestroy()
    {
        EventManager.Unsubscribe("OnShieldDestroy", DestroyShield);

    }

    public void SetShild(SpriteRenderer sprite, bool shilded)
    {
        _shieldSprite = sprite;

        if(shilded)
            ShieldUp();
        else
            DestroyShield();
    }

    void ShieldUp()
    {
        if(_shieldSprite != null) 
        _shieldSprite.enabled = true;
    }

    void DestroyShield(params object[] noUse)
    {
        if(_shieldSprite != null) 
        _shieldSprite.enabled = false;
    }
}
