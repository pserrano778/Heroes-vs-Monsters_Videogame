using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : MonoBehaviour
{
    public Camera nonVRCamera;
    public GameObject prefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse clicked");
            RaycastHit hit;
            Ray ray = nonVRCamera.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out hit);
  
            GameObject newUnit = Instantiate(prefab, hit.point, Quaternion.identity);
            newUnit.tag = "Hero";
            newUnit.GetComponent<UnitBehaviour>().setLane(-1);
            print("My object is clicked by mouse");
        }
    }
}
