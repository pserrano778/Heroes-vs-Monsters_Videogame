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
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            endGameManager.GameOver("Monsters");
        }
    }
}
