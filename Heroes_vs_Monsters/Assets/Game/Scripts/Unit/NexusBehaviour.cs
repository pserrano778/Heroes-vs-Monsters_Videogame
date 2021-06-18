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
        // Reduce the health
        currentHealth -= damage;

        // If it has been destroyed (health <= 0)
        if (currentHealth <= 0)
        {
            // Monsters win the match
            endGameManager.GameOver("Monsters");
        }
    }
}
