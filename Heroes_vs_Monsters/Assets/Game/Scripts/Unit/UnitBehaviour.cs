using UnityEngine;
using System.Collections;

public class UnitBehaviour : MonoBehaviour
{
    private int currentHealth;
    public int health = 100;
    public int defense = 10;

    public enum State
    {
        Idle,
        Follow,
        Die,
        Attack,
    }

    public State state = State.Idle;

    //public Transform target;

    public float followRange = 10.0f;

    public float idleRange = 10.0f;

    public float m_speed = 4.0f;

    public Animator anim;
    private Rigidbody2D body2d;

    private bool m_combatIdle = false;
    private bool m_isDead = false;

    // Use this for initialization
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
        

        //Set AirSpeed in animator
        anim.SetFloat("AirSpeed", body2d.velocity.y);

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("e"))
        {
            if (!m_isDead)
                anim.SetTrigger("Death");
            else
                anim.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }

        //Hurt
        else if (Input.GetKeyDown("q"))
            anim.SetTrigger("Hurt");

        //Attack
        else if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;



        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            anim.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            anim.SetInteger("AnimState", 1);

        //Idle
        else
            anim.SetInteger("AnimState", 0);
    }

    IEnumerator FollowState()
    {
        Debug.Log("Follow: Enter");
        while (state == State.Follow)
        {
            // -- Handle input and movement --
            float distance = Input.GetAxis("Horizontal");

            // Swap direction of sprite depending on walk direction
            if (distance > followRange)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            else if (distance < -followRange)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            // Move
            body2d.velocity = new Vector2(distance * m_speed, body2d.velocity.y);

            anim.SetFloat("speed", agent.velocity.magnitude);
            anim.SetBool("attacking", false);

            if (GetDistance() > idleRange)
            {
                state = State.Idle;
            }
            else if ((GetDistance() <= agent.stoppingDistance + 0.5f) && (agent.pathStatus == NavMeshPathStatus.PathComplete))
            {
                state = State.Attack;
            }

            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("Follow: Exit");
        GoToNextState();
    }

    IEnumerator IdleState()
    {
        Debug.Log("Idle: Enter");

        agent.isStopped = true;
        anim.SetFloat("speed", 0);
        anim.SetBool("attacking", false);

        while (state == State.Idle)
        {
            if (GetDistance() < followRange)
            {
                state = State.Follow;
            }

            yield return 0;
        }

        Debug.Log("Idle: Exit");
        GoToNextState();
    }

    IEnumerator DieState()
    {
        Debug.Log("Die: Enter");
        agent.isStopped = true;
        anim.SetBool("dead", true);

        Destroy(this.gameObject, 5);

        Debug.Log("Idle: Exit");

        yield return 0;
    }

    IEnumerator AttackState()
    {
        Debug.Log("Attack: Enter");

        anim.SetFloat("speed", 0);
        anim.SetBool("attacking", true);

        while (state == State.Attack)
        {
            RotateTowards(target);

            if (GetDistance() > (agent.stoppingDistance + 1))
            {
                state = State.Follow;
            }
            yield return 0;
        }

        GoToNextState();
    }

    void GoToNextState()
    {
        StopAllCoroutines();

        string methodName = state.ToString() + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }
}

public class ZombieBehaviour : MonoBehaviour
{
    

    private Animator anim;

    

    private NavMeshAgent agent;


    public void OnCollisionEnter(Collision collision)
    {
        if (currentHealth > 0)
        {
            TakeDamage(UnityEngine.Random.Range(5, 20));
        }
    }

    private void TakeDamage(int damageToDeal)
    {
        currentHealth -= damageToDeal;

        if (currentHealth <= 0)
        {
            state = State.Die;

        }
        else
        {
            followRange = Mathf.Max(GetDistance(), followRange);
            state = State.Follow;
            anim.SetTrigger("hit");
        }

        GoToNextState();
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
    }

    

    public float GetDistance()
    {
        return (transform.position - target.transform.position).magnitude;
    }

    

    public int damageAmount = 20;

    public void PhysicalAttack()
    {
        if (GetDistance() <= agent.stoppingDistance + 0.5f)
        {
            target.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
        }
    }
}