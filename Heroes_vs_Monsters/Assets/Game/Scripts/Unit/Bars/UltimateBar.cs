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
        // Get the Ultimate component
        unit = GetComponentInParent<Ultimate>();
    }

    // Update is called once per frame
    void Update()
    {
        // Change the bar fill origin to follow the parent orientation
        if (transform.parent.localScale.x < 0)
        {
            ultimateBar.fillOrigin = 1;
        }
        else
        {
            ultimateBar.fillOrigin = 0;
        }

        // Get the current unit energy
        float currentEnergy = unit.getEnergy();

        // Get the unit max energy
        float maxEnergy = unit.maxEnergy;

        // Set the percetage of energy
        ultimateBar.fillAmount = currentEnergy / maxEnergy;

        // Disable the bar if the unit has died
        if (GetComponentInParent<UnitBehaviour>().getCurrentHealth() <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
