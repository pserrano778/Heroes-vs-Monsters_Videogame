using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BerserkerBehaviour : MonsterBehaviour
{
    public float lifestealPercentage;

    protected override void DamageEnemy()
    {
        // Make the damage
        target.takeDamage(damage);

        // If the target has die, set it to null
        if (target.getCurrentHealth() <= 0)
        {
            target = null;
        }

        // If has lifesteal active
        if (lifestealPercentage > 0f)
        {
            // Heal the unit using RPC
            GetComponent<PhotonView>().RPC("HealUnit", RpcTarget.All, damage, lifestealPercentage);
        }
    }

    [PunRPC]
    private void HealUnit(int damage, float lifestealPercentage)
    {
        // Update the health with the enemy health stolen
        currentHealth += (int)(damage * lifestealPercentage);
    }
}
