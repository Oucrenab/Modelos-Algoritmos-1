using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView
{
    SpriteRenderer _spriteRenderer;

    public BulletView(SpriteRenderer renderer)
    {
        _spriteRenderer = renderer;
    }

    public BulletView SetTeam(Team team)
    {
        SetColor(team);

        return this;
    }

    void SetColor(Team team)
    {
        switch (team)
        {
            case Team.Player:
                _spriteRenderer.color = Color.cyan;
                break;
            case Team.Enemy:
                _spriteRenderer.color = Color.red;
                break;
            case Team.None:
                _spriteRenderer.color = Color.white;
                break;
        }
    }
}
