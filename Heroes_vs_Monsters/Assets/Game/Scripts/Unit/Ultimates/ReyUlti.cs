﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Photon.Pun;

public class ReyUlti : Ultimate
{
    public float timeDuration;
    public int ultimateDamage;
    private bool casting = false;

    private static readonly object castingUltimate = new object();

    protected override IEnumerator castUltimate()
    {
        casting = true;

        UnitBehaviour unit = GetComponent<UnitBehaviour>();
        int baseDamage = unit.damage;

        unit.StopAllCoroutines();
        Animator anim = GetComponent<Animator>();

        bool attack = anim.GetBool("Attack");
        bool running = anim.GetBool("Running");

        anim.SetBool("Attack", false);
        anim.SetBool("Running", false);

        GetComponent<PhotonView>().RPC("UltimateAnimation", RpcTarget.All);
        GetComponent<PhotonView>().RPC("ApplyUltimate", RpcTarget.All);
        GetComponent<PhotonView>().RPC("ResetEnergy", RpcTarget.All);

        anim.SetBool("Attack", attack);
        anim.SetBool("Running", running);

        unit.GoToNextState();

        // wait until ultimate wears off
        yield return new WaitForSeconds(timeDuration);

        GetComponent<PhotonView>().RPC("RemoveUltimate", RpcTarget.All, baseDamage);

        casting = false;

    }


    [PunRPC]
    private IEnumerator UltimateAnimation()
    {
        // get unit
        UnitBehaviour unit = GetComponent<UnitBehaviour>();

        unit.anim.SetBool("Ultimate", true);

        yield return new WaitForSeconds(unit.getAnimDuration());

        unit.anim.SetBool("Ultimate", false);
    }

    [PunRPC]
    private void ApplyUltimate()
    {
        // get unit
        UnitBehaviour unit = GetComponent<UnitBehaviour>();
        // update (increase) unit attack
        unit.damage = ultimateDamage;

        // change colour to show that the unit is using its ultimate
        unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1, 0.92f, 0.016f, 1);
    }

    [PunRPC]
    private void RemoveUltimate(int baseDamage)
    {
        // get unit
        UnitBehaviour unit = GetComponent<UnitBehaviour>();

        // change back colour and defense to base values
        unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
        unit.damage = baseDamage;
    }
}
