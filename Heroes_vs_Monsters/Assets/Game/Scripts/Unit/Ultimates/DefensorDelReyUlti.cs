using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Photon.Pun;

public class DefensorDelReyUlti : Ultimate
{
    public float timeDuration;
    public int ultimateDefense;

    protected override IEnumerator castUltimate()
    {
        // Store base defense
        int baseDefense = GetComponent<UnitBehaviour>().defense;

        // Apply ultimate using RPC
        GetComponent<PhotonView>().RPC("ApplyUltimate", RpcTarget.All);

        // Wait until ultimate wears off
        yield return new WaitForSeconds(timeDuration);

        // Remove ultimate using RPC
        GetComponent<PhotonView>().RPC("RemoveUltimate", RpcTarget.All, baseDefense);
    }

    [PunRPC]
    private void ApplyUltimate()
    {
        // Get unit
        UnitBehaviour unit = GetComponent<UnitBehaviour>();

        // Update (increase) unit defense

        unit.defense = ultimateDefense;

        // Change colour to show that the unit is using its ultimate
        unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1, 0.92f, 0.016f, 1);
    }

    [PunRPC]
    private void RemoveUltimate(int baseDefense)
    {
        // Get unit

        UnitBehaviour unit = GetComponent<UnitBehaviour>();
        // Change back colour and defense to base values
        unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
        unit.defense = baseDefense;
    }
}
