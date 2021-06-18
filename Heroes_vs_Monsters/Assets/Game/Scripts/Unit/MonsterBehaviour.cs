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
            TargetNewEnemy();

            if (target == null)
            {
                target = nexusStone;
            }

            state = State.Follow;
            yield return 0;
        }
        GoToNextState();
    }

    protected override IEnumerator FollowState()
    {
        while (state == State.Follow)
        {
            if (target == null || target.getCurrentHealth() <= 0)
            {
                state = State.Idle;
                anim.SetBool("Attack", false);
                anim.SetBool("Running", false);
                body2d.velocity = new Vector2(0, 0);
            }
            else
            {
                TargetNewEnemy();

                if (target == null)
                {
                    target = nexusStone;
                }

                // -- Handle input and movement --
                float targetPosition = target.transform.position.x;
                float distanceToTarget = targetPosition - transform.position.x;

                float waitTime = 0.1f;

                // Swap direction of sprite depending on walk direction
                if (distanceToTarget < 0)
                {
                    transform.localScale = new Vector3(scale, scale, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(-scale, scale, 1.0f);
                }
                if (Mathf.Abs(distanceToTarget) > attackRange)
                {

                    body2d.velocity = new Vector2(distanceToTarget * speed, body2d.velocity.y);

                    anim.SetBool("Running", true);
                }
                else
                {
                    body2d.velocity = new Vector2(0, 0);

                    state = State.Attack;
                    anim.SetBool("Attack", true);

                    waitTime = 0;
                }

                yield return new WaitForSeconds(waitTime);
            }
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
            if (distanceToEnemy > distance && distanceToEnemy <= 0 && (enemies[i].GetComponent<UnitBehaviour>()).getCurrentHealth() > 0
                && enemies[i].GetComponent<UnitBehaviour>().getLane() == this.getLane())
            {
                target = i;
                distance = enemies[i].transform.position.x;
            }
        }
        return target;
    }
}
