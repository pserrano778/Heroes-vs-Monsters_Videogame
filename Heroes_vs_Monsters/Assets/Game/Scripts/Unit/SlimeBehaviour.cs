using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class SlimeBehaviour : MonsterBehaviour
{
    protected override void DamageEnemy()
    {
        // Get all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);

        // Check them
        for (int i = 0; i < enemies.Length; i++)
        {
            // Get the unit
            UnitBehaviour enemy = enemies[i].GetComponent<UnitBehaviour>();

            // If they are in the same line and if the are alive (nexus too)
            if ((enemy.getLane() == this.getLane() && enemy.getCurrentHealth() > 0) || 
                (enemy.tag == "Nexus" && enemy.getCurrentHealth() > 0))
            {
                // If they are in range
                if ( Mathf.Abs(enemy.transform.position.x - transform.position.x) <= attackRange)
                {

                    // Do the damage
                    enemy.takeDamage(damage);

                    // If they are dead, set it to null
                    if (enemy.getCurrentHealth() <= 0)
                    {
                        enemies[i] = null;
                    }
                }
            }
        }

        // Do the damage to the nexus if it was the target
        if (target == nexusStone)
        {
            target.takeDamage(damage);
        }

        // Set the target to null if it is dead
        if (target.getCurrentHealth() <= 0)
        {
            target = null;
        }
    }
}
