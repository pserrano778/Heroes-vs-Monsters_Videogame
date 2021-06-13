using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBehaviour : MonsterBehaviour
{
    private bool previousEnemyHasDead = false;
    public int biteDamage;
    protected override void DamageEnemy()
    {
        int nextAttackDamage = damage;
        if (previousEnemyHasDead)
        {
            nextAttackDamage += biteDamage;
        }

        target.takeDamage(nextAttackDamage);

        if (target.getCurrentHealth() <= 0)
        {
            target = null;
            previousEnemyHasDead = true;
        }
        else
        {
            previousEnemyHasDead = false;
        }

        anim.SetBool("previousEnemyHasDead", previousEnemyHasDead);
    }
    
}
