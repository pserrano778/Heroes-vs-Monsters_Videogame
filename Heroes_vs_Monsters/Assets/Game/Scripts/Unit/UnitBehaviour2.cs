using UnityEngine;
using System.Collections;

public class UnitBehaviour2 : MonoBehaviour
{
    //    private int currentHealth;
    //    public int health = 100;
    //    public int defense = 10;

    //    public enum State
    //    {
    //        Idle,
    //        Follow,
    //        Die,
    //        Attack,
    //    }

    //    public State state = State.Idle;

    //    //public Transform target;

    //    public float followRange = 10.0f;

    //    public float idleRange = 10.0f;

    //    public float m_speed = 4.0f;

    //    

    //    private bool m_combatIdle = false;
    //    private bool m_isDead = false;

    //    // Use this for initialization
    //    void Start()
    //    {
    //        
    //    }

    //    // Update is called once per frame
    //    void Update()
    //    {


    //        //Set AirSpeed in animator
    //        anim.SetFloat("AirSpeed", body2d.velocity.y);

    //        // -- Handle Animations --
    //        //Death
    //        if (Input.GetKeyDown("e"))
    //        {
    //            if (!m_isDead)
    //                anim.SetTrigger("Death");
    //            else
    //                anim.SetTrigger("Recover");

    //            m_isDead = !m_isDead;
    //        }

    //        //Hurt
    //        else if (Input.GetKeyDown("q"))
    //            anim.SetTrigger("Hurt");

    //        //Attack
    //        else if (Input.GetMouseButtonDown(0))
    //        {
    //            anim.SetTrigger("Attack");
    //        }

    //        //Change between idle and combat idle
    //        else if (Input.GetKeyDown("f"))
    //            m_combatIdle = !m_combatIdle;



    //        //Run
    //        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
    //            anim.SetInteger("AnimState", 2);

    //        //Combat Idle
    //        else if (m_combatIdle)
    //            anim.SetInteger("AnimState", 1);

    //        //Idle
    //        else
    //            anim.SetInteger("AnimState", 0);
    //    }

    //    

    //    

    //    IEnumerator DieState()
    //    {
    //        Debug.Log("Die: Enter");
    //        //agent.isStopped = true;
    //        anim.SetBool("dead", true);

    //        Destroy(this.gameObject, 5);

    //        Debug.Log("Idle: Exit");

    //        yield return 0;
    //    }

    //    IEnumerator AttackState()
    //    {
    //        Debug.Log("Attack: Enter");

    //        anim.SetFloat("speed", 0);
    //        anim.SetBool("attacking", true);

    //        while (state == State.Attack)
    //        {
    //            //RotateTowards(target);

    //            //if (GetDistance() > (agent.stoppingDistance + 1))
    //            //{
    //            //    state = State.Follow;
    //            //}
    //            yield return 0;
    //        }

    //        GoToNextState();
    //    }

    //    void GoToNextState()
    //    {
    //        StopAllCoroutines();

    //        string methodName = state.ToString() + "State";
    //        System.Reflection.MethodInfo info = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    //        StartCoroutine((IEnumerator)info.Invoke(this, null));
    //    }
    //}

    //public class ZombieBehaviour : MonoBehaviour
    //{


    //    private Animator anim;



    //    private UnityEngine.AI.NavMeshAgent agent;


    //    public void OnCollisionEnter(Collision collision)
    //    {
    //        //if (currentHealth > 0)
    //        //{
    //        //    TakeDamage(UnityEngine.Random.Range(5, 20));
    //        //}
    //    }

    //    private void TakeDamage(int damageToDeal)
    //    {
    //        //currentHealth -= damageToDeal;

    //        //if (currentHealth <= 0)
    //        //{
    //        //    state = State.Die;

    //        //}
    //        //else
    //        //{
    //        //    followRange = Mathf.Max(GetDistance(), followRange);
    //        //    state = State.Follow;
    //        //    anim.SetTrigger("hit");
    //        //}

    //        //GoToNextState();
    //    }

    //    private void RotateTowards(Transform target)
    //    {
    //        Vector3 direction = (target.position - transform.position).normalized;
    //        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    //        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
    //    }



    //    public float GetDistance()
    //    {
    //        return 0;//(transform.position - target.transform.position).magnitude;
    //    }



    //    public int damageAmount = 20;

    //    public void PhysicalAttack()
    //    {
    //        if (GetDistance() <= agent.stoppingDistance + 0.5f)
    //        {
    //            //target.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
    //        }
    //    }
}