using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UltimateBar : MonoBehaviour
{
    private Ultimate unit;
    public Image ultimateBar;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Ultimate component
        unit = GetComponent<Ultimate>();
    }

    // Update is called once per frame
    void Update()
    {
        // Change the bar fill origin to follow the orientation
        if (transform.localScale.x < 0)
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
        if (GetComponent<UnitBehaviour>().getCurrentHealth() <= 0 && GetComponent<PhotonView>().IsMine)
        {
            GetComponent<PhotonView>().RPC("DisableUltimateBar", RpcTarget.All);
        }
    }

    [PunRPC]
    public void DisableUltimateBar()
    {
        ultimateBar.transform.parent.gameObject.SetActive(false);
    }
}
