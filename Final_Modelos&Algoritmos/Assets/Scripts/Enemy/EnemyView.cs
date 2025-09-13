using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView
{
    [SerializeField] ParticleSystem _deathParticles;


    public EnemyView()
    {

    }

    public EnemyView SetDeath(BaseEnemy myBase, ParticleSystem particle)
    {
        _deathParticles = particle;
        myBase.OnDeath += Death;
        return this;
    }

    public void Death()
    {
        _deathParticles.Play();
    }
}
