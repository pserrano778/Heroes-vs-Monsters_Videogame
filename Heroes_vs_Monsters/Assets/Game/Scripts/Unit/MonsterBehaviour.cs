using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : UnitBehaviour
{
    public GameObject nexusStone;

    public override int TargetEnemy(GameObject[] enemies)
    {
        int target = -1;

        double distance = double.MaxValue;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].transform.position.x < distance && (enemies[i].GetComponent<UnitBehaviour>()).getCurrentHealth() > 0
                && enemies[i].GetComponent<UnitBehaviour>().getLane() == this.getLane())
            {
                target = i;
                distance = enemies[i].transform.position.x;
            }
        }
        print(target);
        return target;
    }
}
