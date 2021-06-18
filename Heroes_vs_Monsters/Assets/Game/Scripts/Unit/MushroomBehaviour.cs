using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBehaviour : MonsterBehaviour
{
    private bool previousEnemyHasDead = false;
    public int biteDamage;
    protected override void DamageEnemy()
    {
        // Set the next attack default damage to the unit damage
        int nextAttackDamage = damage;

        // If the prefious enemy has dead
        if (previousEnemyHasDead)
        {
            // Increase next attack damage
            nextAttackDamage += biteDamage;
        }

        // Do the damage
        target.takeDamage(nextAttackDamage);

        // Check if the target has died
        if (target.getCurrentHealth() <= 0)
        {
            // If it has died
            target = null;

            // Previous enemy has dead
            previousEnemyHasDead = true;
        }
        else
        {
            // It has not dead
            previousEnemyHasDead = false;
        }

        // Change the animator variable
        anim.SetBool("previousEnemyHasDead", previousEnemyHasDead);
    }
    
}
