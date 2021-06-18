using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManagement : MonoBehaviour
{
    private const float TICK_TIME = 2.0f;
    private const float TIME_TO_NEXT_PHASE = 60f;
    private const int MAX_PHASE = 4;
    private float elapsedTime;
    private float phaseTime;
    private int resourcesPerTick;
    private int resources;
    public TextMesh resourceCounterText;

    public GameObject[] heroesPrefabs;

    public GameObject[] monstersPrefabs;

    public GameObject[] unitsPrefabs;

    public UnityEngine.Color[] originalColours;

    private int currentPhase;

    public TextMesh phaseText;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
        resources = 0;
        currentPhase = 1;
        phaseText.text = "Phase: " + currentPhase;
        phaseTime = 0;
        string typeOfPlayer = NetworkManager.GetTypeOfPlayer();

        if (typeOfPlayer == "Heroes")
        {
            unitsPrefabs = heroesPrefabs;
            resourcesPerTick = 1;
        }
        else
        {
            unitsPrefabs = monstersPrefabs;
            resourcesPerTick = 2;
        }

        originalColours = new UnityEngine.Color[unitsPrefabs.Length];
        
        for (int i = 0; i < unitsPrefabs.Length; i++)
        {
            print(unitsPrefabs[i]);
            originalColours[i] = unitsPrefabs[i].GetComponent<SpriteRenderer>().color;
            if (unitsPrefabs[i].GetComponent<UnitBehaviour>().phase > currentPhase)
            {
                unitsPrefabs[i].GetComponent<SpriteRenderer>().color = new UnityEngine.Color(0f, 0f, 0f, 0.3f);
            }
            else
            {
                UpdateUnitColour(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        phaseTime += Time.deltaTime;

        if (elapsedTime / TICK_TIME >= 1)
        {
            elapsedTime -= TICK_TIME;
            resources += resourcesPerTick;
            
            if (currentPhase < MAX_PHASE && phaseTime / TIME_TO_NEXT_PHASE >= 1)
            {
                phaseTime -= TIME_TO_NEXT_PHASE;
                NextPhase();
            }
            else
            {
                UpdateUnitsColours();
            }
            UpdateCounterText();
        }
    }

    public int GetResources()
    {
        return resources;
    }

    public void DecreaseResources(int cost)
    {
        resources = resources - cost;
    }

    public void SetResourcesPerTick(int newResourcesPerTick)
    {
        resourcesPerTick = newResourcesPerTick;
    }

    public void UpdateCounterText()
    {
        resourceCounterText.text = resources.ToString();
    }

    private void UpdateUnitColour(int unit)
    {
        if (unitsPrefabs[unit].GetComponent<UnitBehaviour>().cost > resources)
        {
            unitsPrefabs[unit].GetComponent<SpriteRenderer>().color = new UnityEngine.Color(0.38f, 0.38f, 0.38f, 1f);
        }
        else
        {
            unitsPrefabs[unit].GetComponent<SpriteRenderer>().color = originalColours[unit];
        }
    }

    public void UpdateUnitsColours(){
        for (int i = 0; i<unitsPrefabs.Length; i++)
        {
            if (unitsPrefabs[i].GetComponent<UnitBehaviour>().phase <= currentPhase)
            {
                UpdateUnitColour(i);
            }
        }
    }

    public void NextPhase()
    {
        currentPhase++;
        phaseText.text = "Phase: " + currentPhase;
        resourcesPerTick = (resourcesPerTick / (currentPhase - 1)) * currentPhase;
        UpdateUnitsColours();
    }

    public int GetPhase()
    {
        return currentPhase;
    }
}
