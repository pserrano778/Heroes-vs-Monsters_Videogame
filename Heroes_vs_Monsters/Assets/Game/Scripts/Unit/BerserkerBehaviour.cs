using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BerserkerBehaviour : MonsterBehaviour
{
    public float lifestealPercentage;
    protected override void DamageEnemy()
    {
        target.takeDamage(damage);
        if (target.getCurrentHealth() <= 0)
        {
            target = null;
        }
        if (lifestealPercentage > 0f)
        {
            GetComponent<PhotonView>().RPC("HealUnit", RpcTarget.All, damage, lifestealPercentage);
        }
    }

    [PunRPC]
    private void HealUnit(int damage, float lifestealPercentage)
    {
        currentHealth += (int)(damage * lifestealPercentage);
    }
}
