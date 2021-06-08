using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehaviour : UnitBehaviour
{
    protected override void DamageEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);


        for (int i = 0; i < enemies.Length; i++)
        {
            UnitBehaviour enemy = enemies[i].GetComponent<UnitBehaviour>();
            if (enemy.getLane() == this.getLane() && enemy.getCurrentHealth() > 0)
            {
                if ((transform.localScale.x < 0 && (enemy.transform.position.x - transform.position.x >= -attackRange))
                    || (transform.localScale.x >= 0 && (enemy.transform.position.x - transform.position.x <= attackRange)))
                {
                    enemy.takeDamage(damage);

                    if (enemy.getCurrentHealth() <= 0)
                    {
                        enemies[i] = null;
                    }
                }
            }
        }

        if (target.getCurrentHealth() <= 0)
        {
            target = null;
        }
    }
}
