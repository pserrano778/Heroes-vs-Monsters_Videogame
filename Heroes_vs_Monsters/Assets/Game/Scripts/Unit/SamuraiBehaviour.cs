using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiBehaviour : UnitBehaviour
{
    private int numAttacks = 0;
    public int passiveExtraDamage;
    public int numMaxAttacks;
    protected override void DamageEnemy()
    {
        // If has make the num max attacks
        if (numAttacks == numMaxAttacks)
        {
            // Do damage to the front enemies (and extra damage)
            DoDamageToFrontEnemies(damage + passiveExtraDamage);

            // Reset attacks
            numAttacks = 0;
        }
        else
        {
            // Do the normal damage
            target.takeDamage(damage);

            // Incremet the number of attacks
            numAttacks++;
        }

        // If the target has died
        if (target.getCurrentHealth() <= 0)
        {
            // Set it to null
            target = null;
        }

        // Update the animator variable
        anim.SetInteger("numAttacks", numAttacks);
    }

    private void DoDamageToFrontEnemies(int damage)
    {
        // Get all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);

        // Check them
        for (int i = 0; i < enemies.Length; i++)
        {
            // Get the enemy unit
            UnitBehaviour enemy = enemies[i].GetComponent<UnitBehaviour>();

            // If it is in the same lane and has not dead
            if (enemy.getLane() == this.getLane() && enemy.getCurrentHealth() > 0)
            {
                // If it is in front of the samurai and inside his range
                if ((transform.localScale.x >= 0 && (enemy.transform.position.x - transform.position.x >= -attackRange) && enemy.transform.position.x <= transform.position.x)
                    || (transform.localScale.x < 0 && (enemy.transform.position.x - transform.position.x <= attackRange) && enemy.transform.position.x >= transform.position.x))
                {
                    // Do the damage
                    enemy.takeDamage(damage);

                    // If the enemy has dead
                    if (enemy.getCurrentHealth() <= 0)
                    {
                        // Set it to null
                        enemies[i] = null;
                    }
                }
            }
        }
    }
}
