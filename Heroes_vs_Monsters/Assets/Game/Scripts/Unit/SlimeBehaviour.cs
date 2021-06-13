using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class SlimeBehaviour : MonsterBehaviour
{
    protected override void DamageEnemy()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);


        for (int i = 0; i < enemies.Length; i++)
        {
            UnitBehaviour enemy = enemies[i].GetComponent<UnitBehaviour>();
            print(enemy);

            if ((enemy.getLane() == this.getLane() && enemy.getCurrentHealth() > 0) || 
                (enemy.tag == "Nexus" && enemy.getCurrentHealth() > 0))
            {
                if ( (enemy.transform.position.x - transform.position.x >= -attackRange)
                    ||  (enemy.transform.position.x - transform.position.x <= attackRange))
                {
                    enemy.takeDamage(damage);

                    if (enemy.getCurrentHealth() <= 0)
                    {
                        enemies[i] = null;
                    }
                }
            }
        }

        if (target == nexusStone)
        {
            target.takeDamage(damage);
        }

        if (target.getCurrentHealth() <= 0)
        {
            target = null;
        }
    }
}
