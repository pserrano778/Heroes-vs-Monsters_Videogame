using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public abstract class Ultimate : MonoBehaviour
{
    public int maxEnergy;
    protected int energy = 0;
    private int energyPerTick = 10;
    private const int TICK_RATE = 1;
    private float timeToTick = 0;

    // Update is called once per frame
    void Update()
    {
        // If the player if the owner of the network Object
        if (GetComponent<PhotonView>().IsMine) {

            // Try to cast the Ultimate
            TryCastUltimate();

            // If the Unit has not died
            if (GetComponent<UnitBehaviour>().getState() != UnitBehaviour.State.Die)
            {
                // Update time to tick
                timeToTick += Time.deltaTime;

                // If it need to tick
                if (timeToTick >= TICK_RATE)
                {
                    // Update the time to the next tick
                    timeToTick -= TICK_RATE;

                    // Update the Energy using RPC
                    GetComponent<PhotonView>().RPC("UpdateEnergy", RpcTarget.All);
                }
            }
        }
        
    }

    protected virtual void TryCastUltimate()
    {
        // If has reached max energy
        if (energy >= maxEnergy)
        {
            // If can cast the Ultimate
            if (CanCastUltimate())
            {
                // Reset energy using RPC
                GetComponent<PhotonView>().RPC("ResetEnergy", RpcTarget.All);

                // Cast the ultimate using a Coroutine
                StartCoroutine(castUltimate());
            }
        }
    }

    protected virtual bool CanCastUltimate()
    {
        // Return always true (default behaviour)
        return true;
    }

    // Abstract Coroutine that Units with ultimate will implement
    protected abstract IEnumerator castUltimate();

    public int getEnergy()
    {
        // Return current energy
        return energy;
    }

    // RPC Method
    [PunRPC]
    protected virtual void UpdateEnergy()
    {
        // If has not reached the max energy
        if (energy < maxEnergy)
        {
            // Increment energy
            energy += energyPerTick;

            // If it is greater than max energy now
            if (energy > maxEnergy)
            {
                // Set max energy
                energy = maxEnergy;
            }
        }
    }

    // RPC Method
    [PunRPC]
    protected virtual void ResetEnergy()
    {
        // Set energy to 0
        energy = 0;
    }
}
