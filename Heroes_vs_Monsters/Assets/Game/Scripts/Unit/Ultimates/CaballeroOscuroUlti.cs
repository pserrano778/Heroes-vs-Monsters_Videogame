using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaballeroOscuroUlti : Ultimate
{
    protected override IEnumerator castUltimate()
    {
        UnitBehaviour unit = GetComponent<UnitBehaviour>();
        unit.StopAllCoroutines();
        Animator anim = GetComponent<Animator>();

        bool attack = anim.GetBool("Attack");
        bool running = anim.GetBool("Attack");

        anim.SetBool("Attack", true);
        anim.SetBool("Running", false);

        yield return new WaitForSeconds(unit.getAnimDuration());

        anim.SetBool("Running", running);
        anim.SetBool("Attack", attack);

        unit.GoToNextState();
    }
}
