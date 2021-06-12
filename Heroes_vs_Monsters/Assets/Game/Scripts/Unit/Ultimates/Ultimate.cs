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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>().IsMine) {
            TryCastUltimate();

            if (GetComponent<UnitBehaviour>().getState() != UnitBehaviour.State.Die)
            {
                timeToTick += Time.deltaTime;
                if (timeToTick >= TICK_RATE)
                {
                    timeToTick -= TICK_RATE;
                    GetComponent<PhotonView>().RPC("UpdateEnergy", RpcTarget.All);
                }
            }
        }
        
    }

    protected virtual void TryCastUltimate()
    {
        if (energy >= maxEnergy)
        {
            if (CanCastUltimate())
            {
                GetComponent<PhotonView>().RPC("ResetEnergy", RpcTarget.All);
                StartCoroutine(castUltimate());
            }
        }
    }

    protected virtual bool CanCastUltimate()
    {
        return true;
    }

    protected abstract IEnumerator castUltimate();

    public int getEnergy()
    {
        return energy;
    }

    [PunRPC]
    protected virtual void UpdateEnergy()
    {
        if (energy < maxEnergy)
        {
            energy += energyPerTick;
            if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }
        }
    }

    [PunRPC]
    protected virtual void ResetEnergy()
    {
        energy = 0;
    }
}
