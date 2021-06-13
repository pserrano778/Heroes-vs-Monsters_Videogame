using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int lane;
    private Vector3 spawnPoint;
    public string typeOfUnit;


    // Start is called before the first frame update
    void Start()
    {
        spawnPoint[0] = gameObject.transform.position.x;
        spawnPoint[1] = gameObject.transform.position.y;
        spawnPoint[2] = 0;
    }

    public Vector2 getSpawnPoint()
    {
        return spawnPoint;
    }
}
