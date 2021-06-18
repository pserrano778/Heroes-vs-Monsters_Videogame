using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : UnitBehaviour
{
    public BasicBehaviour nexusStone;

    protected override IEnumerator IdleState()
    {
        // While state is Idle
        while (state == State.Idle)
        {
            // Target a new enemy
            TargetNewEnemy();

            // If the target is null
            if (target == null)
            {
                // Target the nexus
                target = nexusStone;
            }

            // If there is a target, change the state to Follow
            state = State.Follow;
            yield return 0;
        }

        // Go the the next state
        GoToNextState();
    }

    protected override IEnumerator FollowState()
    {
        // While state is Follow
        while (state == State.Follow)
        {
            // If the target is null or has died
            if (target == null || target.getCurrentHealth() <= 0)
            {
                // Change the state to idle
                state = State.Idle;

                // Change the animation to Idle
                anim.SetBool("Attack", false);
                anim.SetBool("Running", false);

                // Set the velocity to 0
                body2d.velocity = new Vector2(0, 0);

                // Set target to null
                target = null;
                yield return 0;
            }
            else
            {
                // Try to target a new enemy if the previous target was the nexus
                if (target == nexusStone) TargetNewEnemy();

                // If there is still no target, target the nexus again
                if (target == null)
                {
                    target = nexusStone;
                }

                // Get the distance to the target
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

                // If the unit doesnt have range
                if (Mathf.Abs(distanceToTarget) > attackRange)
                {
                    // Set the velocity
                    body2d.velocity = new Vector2(distanceToTarget * speed, body2d.velocity.y);

                    // Set the animation to Running
                    anim.SetBool("Running", true);
                }
                else
                {
                    // Set the velocity to 0
                    body2d.velocity = new Vector2(0, 0);

                    // Set the state and animation to attack
                    state = State.Attack;
                    anim.SetBool("Attack", true);

                    waitTime = 0;
                }

                yield return new WaitForSeconds(waitTime);
            }
        }
        // Go the the next state
        GoToNextState();
    }

    public override int TargetEnemy(GameObject[] enemies)
    {
        // No target at first
        int target = -1;

        // Set the distance to the minimun value
        double distance = double.MinValue;

        // Check all enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            // Get the distance to the enemy
            double distanceToEnemy = enemies[i].transform.position.x - transform.position.x;

            // If the enemy is closer (but not behind the unit)
            if (distanceToEnemy > distance && distanceToEnemy <= 0 && (enemies[i].GetComponent<UnitBehaviour>()).getCurrentHealth() > 0
                && enemies[i].GetComponent<UnitBehaviour>().getLane() == this.getLane())
            {
                // Change the target
                target = i;
                distance = enemies[i].transform.position.x;
            }
        }
        // Return the target
        return target;
    }
}
