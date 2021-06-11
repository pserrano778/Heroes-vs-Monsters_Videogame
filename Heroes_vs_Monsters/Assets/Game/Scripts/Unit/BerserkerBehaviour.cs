using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerBehaviour : MonsterBehaviour
{
    public float lifestealPercentage;
    protected override void DamageEnemy()
    {
        target.takeDamage(damage);
        if (target.getCurrentHealth() <= 0)
        {
            target = null;
        }
        if (lifestealPercentage > 0f)
        {
            currentHealth += (int)(damage * lifestealPercentage);
        }
    }
}
