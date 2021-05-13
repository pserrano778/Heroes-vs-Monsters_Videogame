using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    int lane = selectedSpawner.lane;
                    UnitBehaviour newUnit = Instantiate(prefab, spawnPoint, Quaternion.identity);
                    newUnit.tag = selectedSpawner.typeOfUnit;
                    newUnit.GetComponent<UnitBehaviour>().setLane(lane);
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
}
