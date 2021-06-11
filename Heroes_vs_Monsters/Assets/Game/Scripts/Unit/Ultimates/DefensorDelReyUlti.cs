using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DefensorDelReyUlti : Ultimate
{
    public float timeDuration;
    public int ultimateDefense;

    private static readonly object castingUltimate = new object();

    protected override IEnumerator castUltimate()
    {
        lock (castingUltimate)
        {
            if (CanCastUltimate())
            {
                // get unit
                UnitBehaviour unit = GetComponent<UnitBehaviour>();

                // update (increase) unit defense
                int baseDefense = unit.defense;
                unit.defense = ultimateDefense;
              
                // change colour to show that the unit is using its ultimate
                unit.GetComponent<SpriteRenderer>().color =  new UnityEngine.Color(1, 0.92f, 0.016f, 1);
                energy = 0;

                // wait until ultimate wears off
                yield return new WaitForSeconds(timeDuration);

                // change back colour and defense to base values
                unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
                unit.defense = baseDefense;
            }
        }
    }

    protected override bool CanCastUltimate()
    {
        // the ulti can only be casted when the unit has low health
        return GetComponent<UnitBehaviour>().getCurrentHealth() <= GetComponent<UnitBehaviour>().health * 0.5;
    }

}
