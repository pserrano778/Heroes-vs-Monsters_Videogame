using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class UnitBehaviour : MonoBehaviour
{
    public int health = 100;
    private int currentHealth = 1;
    public int defense = 10;
    public float speed = 1.5f;
    public float attackRange = 2f;
    public int damage = 50;

    private State state = State.Idle;

    private UnitBehaviour target = null;

    public Animator anim;
    private Rigidbody2D body2d;

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

        GoToNextState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator IdleState()
    {
        while (state == State.Idle)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);
            if (enemies.Length > 0)
            {
                target = enemies[targetEnemy(enemies)].GetComponent<UnitBehaviour>();
            }

            if (target != null)
            {
                
                state = State.Follow;
            }

            yield return 0;
        }
        GoToNextState();
    }

    IEnumerator FollowState()
    {
        while (state == State.Follow)
        {
            // -- Handle input and movement --
            float targetPosition = target.transform.position.x;
            float distanceToTarget = targetPosition - transform.position.x;
            // Swap direction of sprite depending on walk direction
            if (distanceToTarget > attackRange)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                body2d.velocity = new Vector2(distanceToTarget * speed, body2d.velocity.y);

                anim.SetBool("Running", true);
            } 
            else if (distanceToTarget < -attackRange)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                // Move
                body2d.velocity = new Vector2(distanceToTarget * speed, body2d.velocity.y);

                anim.SetBool("Running", true);

            }
            else
            {
                state = State.Attack;
                anim.SetBool("Running", false);
            }

            yield return new WaitForSeconds(0.2f);
        }
        GoToNextState();
    }

    IEnumerator AttackState()
    {
        float animDuration = 200;
        while (state == State.Attack)
        {
            
            if (target == null || target.getCurrentHealth() <= 0)
            {
                state = State.Idle;
                anim.SetBool("Attack", false);
                animDuration = 0.2f;
            }
            else
            {
                anim.SetBool("Attack", true);
                animDuration = getAnimDuration();
            }
            yield return new WaitForSeconds(animDuration);
            print(tag + " " + animDuration);
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

    IEnumerator DieState()
    {
        anim.SetBool("Dead", true);

        Destroy(this.gameObject, getAnimDuration() + 2);

        yield return 0;
    }

    void GoToNextState()
    {
        StopAllCoroutines();

        string methodName = state.ToString() + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
        
            state = State.Die;
            this.tag = "Untagged";

        }
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    private int targetEnemy(GameObject[] enemies)
    {
        int target = -1;

        double distance = double.MaxValue;
        for (int i=0; i<enemies.Length; i++)
        {
            if (enemies[i].transform.position.x < distance && (enemies[i].GetComponent<UnitBehaviour>()).getCurrentHealth() > 0)
            {
                target = i;
                distance = enemies[i].transform.position.x;
            }
        }
        return target;
    }

    private float getAnimDuration()
    {
        return anim.GetCurrentAnimatorStateInfo(0).length * anim.GetCurrentAnimatorStateInfo(0).speed;
    }
}
