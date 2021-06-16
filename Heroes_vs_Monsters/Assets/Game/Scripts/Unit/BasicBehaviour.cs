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
        GetComponent<PhotonView>().RPC("takeDamageRPC", RpcTarget.All, damage);
    }

    [PunRPC]
    public abstract void takeDamageRPC(int damage);
}
