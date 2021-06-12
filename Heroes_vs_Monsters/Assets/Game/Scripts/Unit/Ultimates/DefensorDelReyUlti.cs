using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Photon.Pun;

public class DefensorDelReyUlti : Ultimate
{
    public float timeDuration;
    public int ultimateDefense;

    private static readonly object castingUltimate = new object();

    protected override IEnumerator castUltimate()
    {
        int baseDefense = GetComponent<UnitBehaviour>().defense;

        GetComponent<PhotonView>().RPC("ApplyUltimate", RpcTarget.All);

        // wait until ultimate wears off
        yield return new WaitForSeconds(timeDuration);

        GetComponent<PhotonView>().RPC("RemoveUltimate", RpcTarget.All, baseDefense);
    }

    [PunRPC]
    private void ApplyUltimate()
    {
        // get unit
        UnitBehaviour unit = GetComponent<UnitBehaviour>();

        // update (increase) unit defense

        unit.defense = ultimateDefense;

        // change colour to show that the unit is using its ultimate
        unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1, 0.92f, 0.016f, 1);
    }

    [PunRPC]
    private void RemoveUltimate(int baseDefense)
    {
        // get unit

        UnitBehaviour unit = GetComponent<UnitBehaviour>();
        // change back colour and defense to base values
        unit.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
        unit.defense = baseDefense;
    }
}
