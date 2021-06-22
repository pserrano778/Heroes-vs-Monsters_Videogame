using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehaviour : UnitBehaviour
{
    protected override void DamageEnemy()
    {
        // Get all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);

        // Check them
        for (int i = 0; i < enemies.Length; i++)
        {
            // Get the enemy Unit
            UnitBehaviour enemy = enemies[i].GetComponent<UnitBehaviour>();
            print("enemy: " + enemy.transform.position.x);
            print("mage: " + transform.position.x);
            // Check if the enemy is in the same lane and is not dead
            if (enemy.getLane() == this.getLane() && enemy.getCurrentHealth() > 0)
            {
                // Check if the enemy is in front of the mage and is inside his range
                if ((transform.localScale.x >= 0 && (enemy.transform.position.x - transform.position.x >= -attackRange) && enemy.transform.position.x <= transform.position.x)
                    || (transform.localScale.x < 0 && (enemy.transform.position.x - transform.position.x <=  attackRange) && enemy.transform.position.x >= transform.position.x))
                {
                    // Make the damage
                    enemy.takeDamage(damage);

                    // If the enemy has died, set it to null
                    if (enemy.getCurrentHealth() <= 0)
                    {
                        enemies[i] = null;
                    }
                }
            }
        }

        // If the target has died, set it to null
        if (target.getCurrentHealth() <= 0)
        {
            target = null;
        }
    }
}
