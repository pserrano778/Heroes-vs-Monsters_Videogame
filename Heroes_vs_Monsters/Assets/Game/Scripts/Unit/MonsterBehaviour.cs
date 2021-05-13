using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : UnitBehaviour
{
    public BasicBehaviour nexusStone;

    protected override IEnumerator IdleState()
    {
        while (state == State.Idle)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);
            if (enemies.Length > 0)
            {
                int newTarget = TargetEnemy(enemies);
                if (newTarget > -1)
                {
                    target = enemies[newTarget].GetComponent<BasicBehaviour>();
                }
            }

            

            if (target == null)
            {
                target = nexusStone;
            }

            print("target " + target);

            state = State.Follow;
            yield return 0;
        }
        GoToNextState();
    }

    public override int TargetEnemy(GameObject[] enemies)
    {
        int target = -1;

        double distance = double.MinValue;
        for (int i = 0; i < enemies.Length; i++)
        {
            double distanceToEnemy = enemies[i].transform.position.x - transform.position.x;
            if (distanceToEnemy > distance  && distance <= 0 && (enemies[i].GetComponent<UnitBehaviour>()).getCurrentHealth() > 0
                && enemies[i].GetComponent<UnitBehaviour>().getLane() == this.getLane())
            {
                target = i;
                distance = enemies[i].transform.position.x;
            }
        }
        print("targetting enemy " + target);
        return target;
    }
}
