using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BerserkerUlti : Ultimate
{
    public float timeDuration;

    private static readonly object castingUltimate = new object();

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
                // get berkserker unit
                BerserkerBehaviour unit = GetComponent<BerserkerBehaviour>();

                // update (increase) unit damage and lifesteal
                int baseDamage = unit.damage;
                unit.damage = ultimateDamage;
                unit.lifestealPercentage = ultimateLifestealPercentage;

                // change colour to show that the unit is using its ultimate
                unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 0f, 0f, 1f);

                // wait until ultimate wears off
                yield return new WaitForSeconds(timeDuration);

                // change colour, damage and lifesteal to base values
                unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
                unit.damage = baseDamage;
                unit.lifestealPercentage = 0;
                energy = 0;
            }
        }
    }

    protected override bool CanCastUltimate()
    {
        // the berserker will only be able to cast its ultimate when his health is low
        return GetComponent<UnitBehaviour>().getCurrentHealth() <= GetComponent<UnitBehaviour>().health * 0.3;
    }

}
