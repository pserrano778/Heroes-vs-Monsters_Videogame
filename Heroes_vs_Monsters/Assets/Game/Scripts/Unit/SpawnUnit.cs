using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnUnit : MonoBehaviour
{
    public Camera camera;
    private UnitBehaviour prefab;
    private bool unitSelected;
    public GameObject[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
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
                    photonView.RPC("SpawnUnitAtPoint", RpcTarget.All, prefab.name, spawnPoint, selectedSpawner.typeOfUnit, selectedSpawner.lane);

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
    public void SpawnUnitAtPoint(string nombrePrefab, Vector2 spawnPoint, string tag, int lane)
    {   
        UnitBehaviour newUnit = Instantiate(FindUnit(nombrePrefab), spawnPoint, Quaternion.identity);
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
