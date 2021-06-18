using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{
    private float timeLeft;
    public EndGameManager endGameManager;
    public TextMesh timeText;

    private int minutes;
    private int seconds;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 240;

        StartCoroutine(UpdateTimeText());
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            endGameManager.GameOver("Heroes");
        }
    }

    private IEnumerator UpdateTimeText()
    {
        while (!(timeLeft < 0))
        {
            minutes = ((int)timeLeft) / 60;
            seconds = ((int)timeLeft) % 60;
            string textSeconds = "";
            if (seconds < 10)
            {
                textSeconds = "0" + seconds;
            }
            else
            {
                textSeconds = seconds.ToString();
            }
            timeText.text = minutes + ":" + textSeconds;
            yield return new WaitForSeconds(0.33f);
        }
        timeText.text = "0:00";
    }
}
