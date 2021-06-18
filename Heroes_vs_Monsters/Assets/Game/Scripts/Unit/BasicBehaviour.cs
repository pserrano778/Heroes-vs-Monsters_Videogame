using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class BasicBehaviour : MonoBehaviour
{
    public int health = 1000;
    protected int currentHealth = 1000;
    public int defense = 0;

    private void Start()
    {
        // Initialize the current health
        currentHealth = health;
    }

    public int getCurrentHealth()
    {
        // Return the current health
        return currentHealth;
    }

    public virtual void takeDamage(int damage)
    {
        // Set the default damage to 1
        int damageTaken = 1;

        // If the damage after the defense mitigation is greater than 0
        if (damage - defense > 0)
        {
            // Update the damage taken
            damageTaken = damage - defense;
        }

        // Make the damage using RPC
        GetComponent<PhotonView>().RPC("takeDamageRPC", RpcTarget.All, damageTaken);
    }

    // RPC abstract function
    [PunRPC]
    public abstract void takeDamageRPC(int damage);
}
