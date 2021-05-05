using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{
    private int currentHealth;
    public int health = 100;
    public int defense = 10;
    public float speed = 1.5f;
    public float attackRange = 2f;

    private State state = State.Idle;

    private GameObject target = null;

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
            target = enemies[0];

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
            print("distancia: " + attackRange);
            // Swap direction of sprite depending on walk direction
            if (distanceToTarget > attackRange)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                body2d.velocity = new Vector2(distanceToTarget * speed, body2d.velocity.y);
            } 
            else if (distanceToTarget < -attackRange)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                // Move
                body2d.velocity = new Vector2(distanceToTarget * speed, body2d.velocity.y);
  
                anim.SetInteger("AnimState", 2);
                anim.SetBool("attacking", false);
            }
            else
            {
                state = State.Attack;
            }

            yield return new WaitForSeconds(0.2f);
        }
        GoToNextState();
    }

    IEnumerator AttackState()
    {
        while (state == State.Attack)
        {
            anim.SetBool("Attack", true);
            anim.speed = 2;
            yield return new WaitForSeconds(2f);
        }
    }

    void GoToNextState()
    {
        StopAllCoroutines();

        string methodName = state.ToString() + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    private int targetEnemy(GameObject[] enemies)
    {
        int target = -1;

        double distance = double.MaxValue;
        for (int i=0; i<enemies.Length; i++)
        {
            print(enemies[i].ToString());
            if (enemies[i].transform.position.x < distance)
            {
                target = i;
                distance = enemies[i].transform.position.x;
                print("entro");
            }
        }
        return target;
    }
}
