using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateBar : MonoBehaviour
{
    private Ultimate unit;
    public Image ultimateBar;

    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponentInParent<Ultimate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.localScale.x < 0)
        {
            ultimateBar.fillOrigin = 1;
        }
        else
        {
            ultimateBar.fillOrigin = 0;
        }
        float currentEnergy = unit.getEnergy();
        float maxEnergy = unit.maxEnergy;
        ultimateBar.fillAmount = currentEnergy / maxEnergy;

        if (GetComponentInParent<UnitBehaviour>().getCurrentHealth() <= 0)
        {
            gameObject.active = false;
        }
    }
}
