using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        TryCastUltimate();

        if (GetComponent<UnitBehaviour>().getState() != UnitBehaviour.State.Die)
        {
            timeToTick += Time.deltaTime;
            if (timeToTick >= TICK_RATE)
            {
                timeToTick -= TICK_RATE;
                UpdateEnergy();
            }
        }
    }

    protected virtual void TryCastUltimate()
    {
        if (energy >= maxEnergy)
        {
            if (CanCastUltimate())
            {
                energy = 0;
                StartCoroutine(castUltimate());
            }
        }
    }

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

    protected virtual bool CanCastUltimate()
    {
        return true;
    }

    protected abstract IEnumerator castUltimate();
}
