using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public Team GetTeam();

    public void GetDamage(int amount);
}

public enum Team
{
    Player,
    Enemy,
    None
}
