using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiBehaviour : UnitBehaviour
{
    private int numAttacks = 0;
    public int passiveExtraDamage;

    protected override void DamageEnemy()
    {
        
        if (numAttacks == 3)
        {
            DoDamageToFrontEnemies(damage + passiveExtraDamage);
        }
        else
        {
            target.takeDamage(damage);
        }

        if (target.getCurrentHealth() <= 0)
        {
            target = null;
        }

        if (numAttacks == 3)
        {
            numAttacks = 0;
        }
        else
        {
            numAttacks++;
        }

        anim.SetInteger("numAttacks", numAttacks);
    }

    private void DoDamageToFrontEnemies(int damage)
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
    }
}
