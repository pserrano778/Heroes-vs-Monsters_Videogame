using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ultimate : MonoBehaviour
{
    public int maxEnergy;

    private int energy = 0;

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
        timeToTick += Time.deltaTime;
        if (timeToTick >= TICK_RATE)
        {
            timeToTick -= TICK_RATE;
            energy += energyPerTick;

            if (energy >= maxEnergy)
            {
                energy = 0;
                StartCoroutine(castUltimate());
            }
        }
    }

    protected abstract IEnumerator castUltimate();
}
