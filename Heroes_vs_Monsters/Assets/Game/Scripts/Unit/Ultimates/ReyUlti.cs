using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Photon.Pun;

public class ReyUlti : Ultimate
{
    public float timeDuration;
    public int ultimateDamage;
    public int ultimateDefense;

    private static readonly object castingUltimate = new object();

    protected override IEnumerator castUltimate()
    {
        // Get the rigid body
        Rigidbody2D body2d = GetComponent<Rigidbody2D>();

        // Get the velocity
        Vector2 velocity = body2d.velocity;

        // Get the Unit Behaviour
        UnitBehaviour unit = GetComponent<UnitBehaviour>();

        // Store base damage and defense
        int baseDamage = unit.damage;
        int baseDefense = unit.defense;

        // Stop all Coroutines in the unit
        unit.StopAllCoroutines();

        // Get the animator
        Animator anim = GetComponent<Animator>();

        // Store the actual animator state
        bool attack = anim.GetBool("Attack");
        bool running = anim.GetBool("Running");

        // Change the anim state to idle
        anim.SetBool("Attack", false);
        anim.SetBool("Running", false);

        // Stop the unit (Velocity = 0)
        body2d.velocity = new Vector2(0, 0);

        // Set the animator state to Ultimate
        unit.anim.SetBool("Ultimate", true);

        // Reset energy using RPC
        GetComponent<PhotonView>().RPC("ResetEnergy", RpcTarget.All);

        // Wait until the animation has finished
        yield return new WaitForSeconds(unit.getAnimDuration());

        // Return to anim state idle
        unit.anim.SetBool("Ultimate", false);

        // Apply the ultimate using RPC
        GetComponent<PhotonView>().RPC("ApplyUltimate", RpcTarget.All);
        
        // Return to the previous unit staus
        anim.SetBool("Attack", attack);
        anim.SetBool("Running", running);
        body2d.velocity = velocity;
        unit.GoToNextState();

        // Wait until ultimate wears off
        yield return new WaitForSeconds(timeDuration);

        // Remove the ultimate using RPC
        GetComponent<PhotonView>().RPC("RemoveUltimate", RpcTarget.All, baseDamage, baseDefense);
    }

    [PunRPC]
    private void ApplyUltimate()
    {
        // Get unit
        UnitBehaviour unit = GetComponent<UnitBehaviour>();
        // Update (increase) unit attack and defense
        unit.damage = ultimateDamage;
        unit.defense = ultimateDefense;

        // Change colour to show that the unit is using its ultimate
        unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1, 0.92f, 0.016f, 1);
    }

    [PunRPC]
    private void RemoveUltimate(int baseDamage, int baseDefense)
    {
        // Get unit
        UnitBehaviour unit = GetComponent<UnitBehaviour>();

        // Change back colour and defense to base values
        unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
        unit.damage = baseDamage;
        unit.defense = baseDefense;
    }
}
