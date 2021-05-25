using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{
    private float timeLeft; 
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 5;
    }

    // Update is called once per frame
    void Update()
    {
        print("tiempoRestante:" +timeLeft);
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Finish.GameOver("Heroes");
        }
    }
}
