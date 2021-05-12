using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : MonoBehaviour
{
    public Camera camera;
    public GameObject prefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse clicked");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                if (hit.collider.tag == "Spawn")
                {
                    Vector2 spawnPoint = hit.collider.GetComponent<Spawner>().spawnPoint;
                    int lane = hit.collider.GetComponent<Spawner>().lane;
                    GameObject newUnit = Instantiate(prefab, spawnPoint, Quaternion.identity);
                    newUnit.tag = "Hero";
                    newUnit.GetComponent<UnitBehaviour>().setLane(lane);
                }
            }
        }   
    }
}
