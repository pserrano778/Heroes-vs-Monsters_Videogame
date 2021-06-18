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
        // Initialize data
        elapsedTime = 0;
        resources = 0;
        currentPhase = 1;
        phaseText.text = "Phase: " + currentPhase;
        phaseTime = 0;
        string typeOfPlayer = NetworkManager.GetTypeOfPlayer();

        // If the player is playing with Heroes
        if (typeOfPlayer == "Heroes")
        {
            // Set the units that he will be using
            unitsPrefabs = heroesPrefabs;

            // Set the resourcer per tick that he will recive
            resourcesPerTick = 1;
        }
        else // If he is playing with Monsters
        {
            // Set the units that he will be using
            unitsPrefabs = monstersPrefabs;

            // Set the resourcer per tick that he will recive
            resourcesPerTick = 2;
        }

        originalColours = new UnityEngine.Color[unitsPrefabs.Length];
        
        // Change units start colour
        for (int i = 0; i < unitsPrefabs.Length; i++)
        {
            // Store the original Colours of the units
            originalColours[i] = unitsPrefabs[i].GetComponent<SpriteRenderer>().color;

            // Change  the colour of the unit if it phase is greater that the current phase
            if (unitsPrefabs[i].GetComponent<UnitBehaviour>().phase > currentPhase)
            {
                unitsPrefabs[i].GetComponent<SpriteRenderer>().color = new UnityEngine.Color(0f, 0f, 0f, 0.3f);
            }
            else
            {
                // Update the colour (Depending of the resources of the plauyer)
                UpdateUnitColour(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update total time since last tick
        elapsedTime += Time.deltaTime;

        // Update phase time
        phaseTime += Time.deltaTime;

        // If we need to tick again
        if (elapsedTime / TICK_TIME >= 1)
        {
            // Update time since last tick
            elapsedTime -= TICK_TIME;

            // Increase resources
            resources += resourcesPerTick;
            
            // If we have not reached last phase, and we need to increase it
            if (currentPhase < MAX_PHASE && phaseTime / TIME_TO_NEXT_PHASE >= 1)
            {
                // Update time to nest phase
                phaseTime -= TIME_TO_NEXT_PHASE;

                // Go to next phase
                NextPhase();
            }
            else
            {
                // Try to update Units colours
                UpdateUnitsColours();
            }

            // Update the text of the resources
            UpdateCounterText();
        }
    }

    public int GetResources()
    {
        // return the player resources
        return resources;
    }

    public void DecreaseResources(int cost)
    {
        // If the player has enough resources
        if (resources - cost >= 0)
        {
            // Remove some player resources
            resources = resources - cost;
        }
    }

    public void UpdateCounterText()
    {
        // Change the resources text with the updater value of resources
        resourceCounterText.text = resources.ToString();
    }

    private void UpdateUnitColour(int unit)
    {
        // If the player can not buy this unit
        if (unitsPrefabs[unit].GetComponent<UnitBehaviour>().cost > resources)
        {
            // Change the colour
            unitsPrefabs[unit].GetComponent<SpriteRenderer>().color = new UnityEngine.Color(0.38f, 0.38f, 0.38f, 1f);
        }
        else
        {
            // Set the original colour
            unitsPrefabs[unit].GetComponent<SpriteRenderer>().color = originalColours[unit];
        }
    }

    public void UpdateUnitsColours(){
        // Check all units
        for (int i = 0; i<unitsPrefabs.Length; i++)
        {
            // If the Unit is now available due to it phase
            if (unitsPrefabs[i].GetComponent<UnitBehaviour>().phase <= currentPhase)
            {
                // Update its colour
                UpdateUnitColour(i);
            }
        }
    }

    public void NextPhase()
    {
        // Increase the phase value
        currentPhase++;

        // Change the phase text
        phaseText.text = "Phase: " + currentPhase;

        // Increase the resources per thick -> baseResourcesPerTick * currentPhase
        resourcesPerTick = (resourcesPerTick / (currentPhase - 1)) * currentPhase;

        // Try to update de Units colours
        UpdateUnitsColours();
    }

    public int GetPhase()
    {
        // Return the current phase
        return currentPhase;
    }
}
