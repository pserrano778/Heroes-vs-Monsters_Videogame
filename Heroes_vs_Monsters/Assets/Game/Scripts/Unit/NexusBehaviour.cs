using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NexusBehaviour : BasicBehaviour
{
    public EndGameManager endGameManager;

    [PunRPC]
    public override void takeDamageRPC(int damage)
    {
        int damageTaken = 1;
        if (damage - defense > 0)
        {
            damageTaken = damage - defense;
        }
        currentHealth -= damageTaken;

        if (currentHealth <= 0)
        {
            endGameManager.GameOver("Monsters");
        }
    }
}
