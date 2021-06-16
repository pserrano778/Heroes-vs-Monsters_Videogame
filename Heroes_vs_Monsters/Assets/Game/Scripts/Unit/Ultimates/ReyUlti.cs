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
        Rigidbody2D body2d = GetComponent<Rigidbody2D>();
        Vector2 velocity = body2d.velocity;

        UnitBehaviour unit = GetComponent<UnitBehaviour>();
        int baseDamage = unit.damage;
        int baseDefense = unit.defense;

        unit.StopAllCoroutines();
        Animator anim = GetComponent<Animator>();

        bool attack = anim.GetBool("Attack");
        bool running = anim.GetBool("Running");

        anim.SetBool("Attack", false);
        anim.SetBool("Running", false);
        body2d.velocity = new Vector2(0, 0);

        unit.anim.SetBool("Ultimate", true);
        GetComponent<PhotonView>().RPC("ResetEnergy", RpcTarget.All);

        yield return new WaitForSeconds(unit.getAnimDuration());

        unit.anim.SetBool("Ultimate", false);

        GetComponent<PhotonView>().RPC("ApplyUltimate", RpcTarget.All);
        

        anim.SetBool("Attack", attack);
        anim.SetBool("Running", running);
        body2d.velocity = velocity;
        unit.GoToNextState();

        // wait until ultimate wears off
        yield return new WaitForSeconds(timeDuration);

        GetComponent<PhotonView>().RPC("RemoveUltimate", RpcTarget.All, baseDamage, baseDefense);
    }

    [PunRPC]
    private void ApplyUltimate()
    {
        // get unit
        UnitBehaviour unit = GetComponent<UnitBehaviour>();
        // update (increase) unit attack and defense
        unit.damage = ultimateDamage;
        unit.defense = ultimateDefense;

        // change colour to show that the unit is using its ultimate
        unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1, 0.92f, 0.016f, 1);
    }

    [PunRPC]
    private void RemoveUltimate(int baseDamage, int baseDefense)
    {
        // get unit
        UnitBehaviour unit = GetComponent<UnitBehaviour>();

        // change back colour and defense to base values
        unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
        unit.damage = baseDamage;
        unit.defense = baseDefense;
    }
}
