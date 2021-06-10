using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BerserkerUlti : Ultimate
{
    public float timeDuration;

    private static readonly object castingUltimate = new object();

    private bool casting = false;

    public BerserkerUlti()
    {
        energy = maxEnergy;
    }

    protected override IEnumerator castUltimate()
    {
        lock (castingUltimate)
        {
            if (CanCastUltimate())
            {
                print("ULTI BERSERKER");
                casting = true;

                UnitBehaviour unit = GetComponent<UnitBehaviour>();
                //unit.StopAllCoroutines();
                //Animator anim = GetComponent<Animator>();

                // bool attack = anim.GetBool("Attack");
                // bool running = anim.GetBool("Attack");

                // anim.SetBool("Attack", true);
                // anim.SetBool("Running", false);

                unit.defense = 100000;

                unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 0f, 0f, 1f);

                yield return new WaitForSeconds(timeDuration);

                casting = false;

                unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
                unit.defense = 0;
                energy = 0;


            }
        }
    }

    protected override bool CanCastUltimate()
    {
        UnitBehaviour unit = GetComponent<UnitBehaviour>();
        return unit.health <= unit.health * 0.5;
    }

}
