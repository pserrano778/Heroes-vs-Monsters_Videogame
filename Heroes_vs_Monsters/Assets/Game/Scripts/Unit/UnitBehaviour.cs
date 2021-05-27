using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class UnitBehaviour : BasicBehaviour
{
    protected int lane = -1;
    public int cost;
    public float speed = 1.5f;
    public float attackRange = 1f;
    public int damage = 50;
    public int phase;

    protected State state = State.Idle;

    protected BasicBehaviour target = null;

    public Animator anim;
    protected Rigidbody2D body2d;

    public enum State
    {
        Idle,
        Follow,
        Die,
        Attack,
    }

    public string typeOfEnemy = "";

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();

        currentHealth = health;

        if (tag == "Hero")
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        if (lane != -1)
        {
            GoToNextState();
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    protected virtual IEnumerator IdleState()
    {
        while (state == State.Idle)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);
            if (enemies.Length > 0)
            {
                int newTarget = TargetEnemy(enemies);
                if (newTarget > -1)
                {
                    target = enemies[newTarget].GetComponent<UnitBehaviour>();
                }
            }

            if (target != null)
            {
                state = State.Follow;
            }

            yield return 0;
        }
        GoToNextState();
    }

    protected IEnumerator FollowState()
    {
        while (state == State.Follow)
        {
            // -- Handle input and movement --
            float targetPosition = target.transform.position.x;
            float distanceToTarget = targetPosition - transform.position.x;

            float waitTime = 0.1f;

            // Swap direction of sprite depending on walk direction
            if (distanceToTarget < 0)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            if (distanceToTarget > attackRange)
            {
                
                body2d.velocity = new Vector2(distanceToTarget * speed, body2d.velocity.y);

                anim.SetBool("Running", true);
            } 
            else if (distanceToTarget < -attackRange)
            {
                
                // Move
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
        GoToNextState();
    }

    protected IEnumerator AttackState()
    {
        
        float animDuration = 200;
        while (state == State.Attack)
        {
            
            if (target == null || target.getCurrentHealth() <= 0)
            {
                state = State.Idle;
                anim.SetBool("Attack", false);
                anim.SetBool("Running", false);
                animDuration = 0.1f;
            }
            else
            {
                animDuration = getAnimDuration();
            }
            yield return new WaitForSeconds(animDuration);

            if (state == State.Attack)
            {
                target.takeDamage(damage);
                if (target.getCurrentHealth() <= 0)
                {
                    target = null;
                }
            }
        }
        GoToNextState();
    }

    protected IEnumerator DieState()
    {
        Destroy(this.gameObject, getAnimDuration() + 2);

        yield return 0;
    }

    protected void GoToNextState()
    {
        StopAllCoroutines();

        string methodName = state.ToString() + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }


    public override void takeDamage(int damage)
    {
        int damageTaken = 1;
        if (damage - defense > 0)
        {
            damageTaken = damage - defense;
        }
        currentHealth -= damageTaken;

        if (currentHealth <= 0)
        {
            state = State.Die;
            this.tag = "Untagged";
            anim.SetBool("Dead", true);
        }
    }

    public virtual int TargetEnemy(GameObject[] enemies)
    {
        int target = -1;

        double distance = double.MaxValue;
        for (int i=0; i<enemies.Length; i++)
        {
            if (enemies[i].transform.position.x < distance && (enemies[i].GetComponent<UnitBehaviour>()).getCurrentHealth() > 0  
                && enemies[i].GetComponent<UnitBehaviour>().getLane() == this.getLane())
            {
                target = i;
                distance = enemies[i].transform.position.x;
            }
        }
        return target;
    }

    protected float getAnimDuration()
    {
        return anim.GetCurrentAnimatorStateInfo(0).length * anim.GetCurrentAnimatorStateInfo(0).speed;
    }

    public int getLane()
    {
        return lane;
    }

    public void setLane(int lane)
    {
        this.lane = lane;
    }
}
