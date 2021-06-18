using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class SpawnUnit : MonoBehaviour
{
    private UnitBehaviour prefab;
    private bool unitSelected;
    public ResourceManagement resourceManager;
    private GameObject[] spawnPoints;
    public GameObject[] heroesSpawnPoints;
    public GameObject[] monstersSpawnPoints;
    public GameObject[] heroesPrefabs;
    public GameObject[] monstersPrefabs;
    public NexusBehaviour nexusStone;

    private int unitCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        string typeOfPlayer = NetworkManager.GetTypeOfPlayer();

        // If the player is playing with Heroes
        if (typeOfPlayer == "Heroes")
        {
            // Disable monsters spawns
            spawnPoints = monstersSpawnPoints;
            ChangeVisibility(false);

            // Set the spawns points to Heroes
            spawnPoints = heroesSpawnPoints;

            // Disable Monsters
            for (int i = 0; i < monstersPrefabs.Length; i++)
            {
                monstersPrefabs[i].SetActive(false);
            }
        }
        else
        {
            // Disable Heroes Spawns
            spawnPoints = heroesSpawnPoints;
            ChangeVisibility(false);

            // Set the spawns points to Monsters
            spawnPoints = monstersSpawnPoints;

            // Disable Heroes
            for (int i = 0; i < heroesPrefabs.Length; i++)
            {
                heroesPrefabs[i].SetActive(false);
            }
        }

        // Disable Spawns
        ChangeVisibility(false);

        // No unit selected
        unitSelected = false;
    }

    void Update()
    {
        // If the playes has made left click
        if (Input.GetMouseButtonDown(0))
        {
            // Check if it has collide with something in layer Spawn
            Collider2D colliderHit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), LayerMask.GetMask("Spawn"));

            // If it has collide
            if (colliderHit) 
            {
                // If it has collide with a Prefab Tag and there is no any unit selected
                if (colliderHit.tag == "Prefab" && !unitSelected)
                {
                    // Set the prefab
                    prefab = colliderHit.GetComponent<UnitBehaviour>();

                    // If it has enough resources and is in the right phase
                    if (HasEnoughResources(prefab.cost) && IsInRightPhase(prefab.phase))
                    {
                        // Enable the spawns
                        unitSelected = true;
                        ChangeVisibility(true);
                    }
                    else
                    {
                        // set prefab to null
                        prefab = null;
                    }
                }

                // If it has collide with a Spawn and there is an unit selected
                else if (colliderHit.tag == "Spawn" && unitSelected)
                {      
                    // Get the spawner
                    Spawner selectedSpawner = colliderHit.GetComponent<Spawner>();

                    // Get the spawn pooint
                    Vector3 spawnPoint = selectedSpawner.getSpawnPoint();

                    // Increment 'z' component for layer order
                    spawnPoint[2] += unitCounter;
                    unitCounter++;

                    // Get the photon view
                    PhotonView photonView = PhotonView.Get(this);

                    // Instantiate the Unit
                    GameObject newUnit = PhotonNetwork.Instantiate("Units/" + NetworkManager.GetTypeOfPlayer() + "/" + prefab.name, spawnPoint, Quaternion.identity, 0);

                    // Set the information (lane and type of unit
                    newUnit.GetComponent<UnitBehaviour>().newInformation(selectedSpawner.lane, selectedSpawner.typeOfUnit);

                    // Remove the cost from the resources
                    resourceManager.DecreaseResources(prefab.cost);

                    // Update resources in UI
                    resourceManager.UpdateCounterText();

                    // Change colours (if needed)
                    resourceManager.UpdateUnitsColours();

                    // Disables spawners
                    ChangeVisibility(false);

                    // No unit selected now
                    unitSelected = false;
                }
                else {
                    // Disables spawners
                    ChangeVisibility(false);

                    // No unit selected now
                    unitSelected = false;
                }
            }
        }   
    }


    void ChangeVisibility(bool visible)
    {
        // Enable or Disable all spawns points
        for (int i=0; i<spawnPoints.Length; i++)
        {
            spawnPoints[i].SetActive(visible);
        }
    }

    private bool HasEnoughResources(int cost)
    {
        // Return if the player has enough resources
        return resourceManager.GetResources() >= cost;   
    }

    private bool IsInRightPhase(int phase)
    {
        // Return if the player is in a concrete phase
        return resourceManager.GetPhase() >= phase;
    }
}
