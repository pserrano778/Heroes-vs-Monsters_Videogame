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
        currentHealth = health;
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public virtual void takeDamage(int damage)
    {
        int damageTaken = 1;
        if (damage - defense > 0)
        {
            damageTaken = damage - defense;
        }

        GetComponent<PhotonView>().RPC("takeDamageRPC", RpcTarget.All, damageTaken);
    }

    [PunRPC]
    public abstract void takeDamageRPC(int damage);
}
