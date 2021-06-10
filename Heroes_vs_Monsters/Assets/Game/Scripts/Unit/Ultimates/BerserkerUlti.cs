using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BerserkerUlti : Ultimate
{
    public float timeDuration;

    private static readonly object castingUltimate = new object();

    private bool casting = false;

    public int ultimateDamage;
    public float ultimateLifestealPercentage;

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
                casting = true;

                UnitBehaviour unit = GetComponent<UnitBehaviour>();

                int baseDamage = unit.damage;
                unit.damage = ultimateDamage;
                unit.lifestealPercentage = ultimateLifestealPercentage;

                unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 0f, 0f, 1f);

                yield return new WaitForSeconds(timeDuration);

                casting = false;

                unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
                unit.damage = baseDamage;
                unit.lifestealPercentage = 0;
                energy = 0;


            }
        }
    }

    protected override bool CanCastUltimate()
    {
        return GetComponent<UnitBehaviour>().getCurrentHealth() <= GetComponent<UnitBehaviour>().health * 0.5;
    }

}
