using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class SpawnUnit : MonoBehaviour
{
    public Camera camera;
    private UnitBehaviour prefab;
    private bool unitSelected;
    private GameObject[] spawnPoints;
    public GameObject[] heroesSpawnPoints;
    public GameObject[] monstersSpawnPoints;
    public GameObject[] heroesPrefabs;
    public GameObject[] monstersPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        string typeOfPlayer = NetworkManager.GetTypeOfPlayer();

        // set spawn points and prefabs to player
        // delete object depending on type of player
        if (typeOfPlayer == "Heroes")
        {
            spawnPoints = monstersSpawnPoints;
            ChangeVisibility(false);

            spawnPoints = heroesSpawnPoints;
            for (int i = 0; i < monstersPrefabs.Length; i++)
            {
                monstersPrefabs[i].SetActive(false);
                //Destroy(monstersPrefabs[i]);
            }
        }
        else
        {
            spawnPoints = heroesSpawnPoints;
            ChangeVisibility(false);

            spawnPoints = monstersSpawnPoints;

            for (int i = 0; i < heroesPrefabs.Length; i++)
            {
                heroesPrefabs[i].SetActive(false);
                //Destroy(heroesPrefabs[i]);
            }
        }

        // free memory if player doesn't need the array
        // Array.Clear(monstersPrefabs, 0, monstersPrefabs.Length - 1);
        // Array.Clear(heroesPrefabs, 0, heroesPrefabs.Length - 1);
        // Array.Clear(monstersSpawnPoints, 0, monstersSpawnPoints.Length - 1);
        // Array.Clear(heroesSpawnPoints, 0, heroesSpawnPoints.Length - 1);

        ChangeVisibility(false);
        unitSelected = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                if (hit.collider.tag == "Prefab" && !unitSelected)
                {
                    prefab = hit.collider.GetComponent<UnitBehaviour>();
                    unitSelected = true;
                    ChangeVisibility(true);
                }

                else if (hit.collider.tag == "Spawn" && unitSelected)
                {      
                    Spawner selectedSpawner = hit.collider.GetComponent<Spawner>();

                    Vector2 spawnPoint = selectedSpawner.spawnPoint;
                    PhotonView photonView = PhotonView.Get(this);
                    photonView.RPC("SpawnUnitAtPointRPC", RpcTarget.All, prefab.name, spawnPoint, selectedSpawner.typeOfUnit, selectedSpawner.lane);
                    //SpawnUnitAtPoint(prefab.name, spawnPoint, selectedSpawner.typeOfUnit, selectedSpawner.lane);

                    ChangeVisibility(false);
                    unitSelected = false;
                }

                else {
                    ChangeVisibility(false);
                    unitSelected = false;
                }
            }
        }   
    }


    void ChangeVisibility(bool visible)
    {
        for (int i=0; i<spawnPoints.Length; i++)
        {
            spawnPoints[i].SetActive(visible);
        }
    }

    [PunRPC]
    public void SpawnUnitAtPointRPC(string nombrePrefab, Vector2 spawnPoint, string tag, int lane)
    {   
        UnitBehaviour newUnit = Instantiate(Resources.Load(nombrePrefab) as GameObject, spawnPoint, Quaternion.identity).GetComponent<UnitBehaviour>();
        newUnit.tag = tag;
        newUnit.GetComponent<UnitBehaviour>().setLane(lane);
    }


    public void SpawnUnitAtPoint(string nombrePrefab, Vector2 spawnPoint, string tag, int lane)
    {
        UnitBehaviour newUnit = PhotonNetwork.Instantiate(nombrePrefab, spawnPoint, Quaternion.identity).GetComponent<UnitBehaviour>();
        newUnit.tag = tag;  
        newUnit.GetComponent<UnitBehaviour>().setLane(lane);
    }

    private UnitBehaviour FindUnit(string name)
    {
        UnitBehaviour unit = null;

        GameObject[] units = GameObject.FindGameObjectsWithTag("Prefab");

        bool encontrado = false;

        for (int i=0; i<units.Length && !encontrado; i++)
        {
            if (units[i].name == name)
            {
                encontrado = true;
                unit = units[i].GetComponent<UnitBehaviour>();
            }
        }
        return unit;
    }
}
