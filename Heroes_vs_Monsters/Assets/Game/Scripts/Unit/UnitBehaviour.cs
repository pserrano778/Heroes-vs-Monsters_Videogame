using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Photon.Pun;

public class UnitBehaviour : BasicBehaviour, IPunInstantiateMagicCallback
{
    protected int lane = -1;
    public int cost;
    public float speed = 1.5f;
    public float attackRange = 1f;
    public int damage = 50;
    public int phase;
    public float scale = 1;

    protected State state = State.Idle;

    protected BasicBehaviour target = null;

    public Animator anim;
    protected Rigidbody2D body2d;

    // States Enumerator
    public enum State
    {
        Idle,
        Follow,
        Die,
        Attack,
        Null,
    }

    public string typeOfEnemy = "";

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();

        // Set the health
        currentHealth = health;

        // Set the initial orientation
        if (tag == "Hero")
        {
            transform.localScale = new Vector3(-scale, scale, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(scale, scale, 1.0f);
        }

        // If it has a lane, go to next state
        if (lane != -1)
        {
            GoToNextState();
        }
    }

    protected virtual IEnumerator IdleState()
    {
        // While state is Idle
        while (state == State.Idle)
        {
            // Try to target a new enemy
            TargetNewEnemy();

            // If it is not null
            if (target != null)
            {
                // Set state to Follow
                state = State.Follow;
            }

            yield return 0;
        }

        // Go to next state
        GoToNextState();
    }

    protected virtual IEnumerator FollowState()
    {
        // While state is follow
        while (state == State.Follow)
        {
            if (target == null || target.getCurrentHealth() <= 0)
            {
                // Set the state to Idle
                state = State.Idle;

                // Set the animation to Idle
                anim.SetBool("Attack", false);
                anim.SetBool("Running", false);

                // Set the velocity to 0
                body2d.velocity = new Vector2(0, 0);

                // Set the target to null
                target = null;
            }
            else
            {
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

                // If the enemy is not in range
                if (Mathf.Abs(distanceToTarget) > attackRange)
                {
                    // Set the velocity
                    body2d.velocity = new Vector2(distanceToTarget * speed, body2d.velocity.y);

                    // Set the running variable to true
                    anim.SetBool("Running", true);
                }
                else
                {
                    // Set the velovity to 0
                    body2d.velocity = new Vector2(0, 0);

                    // Set the state to attack
                    state = State.Attack;

                    // Set the animation to attack
                    anim.SetBool("Attack", true);

                    waitTime = 0;
                }

                yield return new WaitForSeconds(waitTime);
            }
        }    

        // Go to next state
        GoToNextState();
    }

    protected IEnumerator AttackState()
    {
        float animDuration = 0f;

        // While state = attack
        while (state == State.Attack)
        {
            // If the target is null or has died
            if (target == null || target.getCurrentHealth() <= 0)
            {
                // Set the state to Idle
                state = State.Idle;

                // Set the animation to Idle
                anim.SetBool("Attack", false);
                anim.SetBool("Running", false);

                // Set the velocity to 0
                body2d.velocity = new Vector2(0, 0);

                // Set the target to null
                target = null;

                yield return 0;
            }
            else
            {
                // Get the distance to target
                float targetPosition = target.transform.position.x;
                float distanceToTarget = targetPosition - transform.position.x;

                // If it is not in range, return to follow state
                if (Mathf.Abs(distanceToTarget) > attackRange)
                {
                    state = State.Follow;
                    anim.SetBool("Attack", false);
                    animDuration = 0f;
                }
                else
                {
                    // Get attack anim duration
                    animDuration = getAnimDuration();
                }
            }

            // Wait for the duration
            yield return new WaitForSeconds(animDuration);

            // If state is attack
            if (state == State.Attack)
            {
                // Damage the enemy
                DamageEnemy();
            }
        }

        // Go to the next state
        GoToNextState();
    }

    protected IEnumerator DieState()
    {
        // Wait some time
        yield return new WaitForSeconds(2.1f);

        // If the player is the ownew, destroy the object
        if(GetComponent<PhotonView>().IsMine)
            PhotonNetwork.Destroy(this.gameObject);

        yield return 0;
    }

    public void GoToNextState()
    {
        // Stop the coroutines
        StopAllCoroutines();
        if (state != State.Null)
        {
            // Get the method name
            string methodName = state.ToString() + "State";

            // Get acces to the method and Start the coroutine
            System.Reflection.MethodInfo info = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            StartCoroutine((IEnumerator)info.Invoke(this, null));
        }
    }

    public override void takeDamage(int damage)
    {
        // If the player is not the owner
        if (!GetComponent<PhotonView>().IsMine)
        {
            // Calculate the damage
            int damageTaken = 1;
            if (damage - defense > 0)
            {
                damageTaken = damage - defense;
            }

            // Do the damage using rpc
            GetComponent<PhotonView>().RPC("takeDamageRPC", RpcTarget.All, damageTaken);
        }
    }

    [PunRPC]
    public override void takeDamageRPC(int damage)
    {
        // Do the damage
        currentHealth -= damage;

        // If it has died
        if (currentHealth <= 0)
        {
            // If is the owner
            if (GetComponent<PhotonView>().IsMine)
            {
                // Set the state to Die
                state = State.Die;
                anim.SetBool("Dead", true);

                // Call UnitDead using RPC
                GetComponent<PhotonView>().RPC("UnitDead", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void UnitDead()
    {
        // Untag the unit
        this.tag = "Untagged";

        // Set the velocity to 0
        body2d.velocity = new Vector2(0, 0);
    }

    protected void TargetNewEnemy()
    {
        // Get all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);

        // If there is enemies
        if (enemies.Length > 0)
        {
            // Target an enemies
            int newTarget = TargetEnemy(enemies);

            // If there is a target, assign it to the attribute
            if (newTarget > -1)
            {
                target = enemies[newTarget].GetComponent<UnitBehaviour>();
            }
        }
    }

    public virtual int TargetEnemy(GameObject[] enemies)
    {
        int target = -1;

        double distance = double.MaxValue;

        for (int i=0; i<enemies.Length; i++)
        {
            // If the enemy is in the same lane (Prioritize the nearest enemy to the nexus)
            if (enemies[i].transform.position.x < distance && (enemies[i].GetComponent<UnitBehaviour>()).getCurrentHealth() > 0  
                && enemies[i].GetComponent<UnitBehaviour>().getLane() == this.getLane())
            {
                // Update the target
                target = i;
                distance = enemies[i].transform.position.x;
            }
        }
        return target;
    }

    public float getAnimDuration()
    {
        // Return the animation duration
        return anim.GetCurrentAnimatorStateInfo(0).length * anim.GetCurrentAnimatorStateInfo(0).speed;
    }

    public int getLane()
    {
        // Return the lane
        return lane;
    }

    public void setLane(int lane)
    {
        // Set a new lane
        this.lane = lane;
    }

    public State getState()
    {
        // Return the state
        return state;
    }

    protected virtual void DamageEnemy()
    {
        // Do the damage
        target.takeDamage(damage);

        // If it has dies, set the target to null
        if (target.getCurrentHealth() <= 0)
        {
            target = null;
        }
    }

    public void newInformation(int lane, string tag)
    {
        // Set the new information using RPC
        GetComponent<PhotonView>().RPC("setInformation", RpcTarget.All, lane, tag);
    }

    [PunRPC]
    public void setInformation(int lane, string tag)
    {
        // Set the lane and the tag
        this.lane = lane;
        this.tag = tag;

        // If it is a monster
        if (GetComponent<MonsterBehaviour>() != null)
        {
            // Set the nexus stone
            GetComponent<MonsterBehaviour>().nexusStone = GameObject.FindGameObjectsWithTag("Nexus")[0].GetComponent<BasicBehaviour>();
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // When the unit is instantiated, if i am not the owner
        if (!GetComponent<PhotonView>().IsMine)
        {
            // Set it state to null
            state = State.Null;

            // Stop All Coroutines
            StopAllCoroutines();
        }
    }
}
