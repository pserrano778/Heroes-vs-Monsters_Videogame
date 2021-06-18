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
        // Set the time left to 4 minutes
        timeLeft = 240;

        // Start the Coroutine that will update the time remaining text
        StartCoroutine(UpdateTimeText());
    }

    // Update is called once per frame
    void Update()
    {
        // Update the time left
        timeLeft -= Time.deltaTime;

        // If the match has finished due to the time
        if (timeLeft < 0)
        {
            // Heroes win
            endGameManager.GameOver("Heroes");
        }
    }

    private IEnumerator UpdateTimeText()
    {
        // Wile the time is greater or equal than 0
        while (!(timeLeft < 0))
        {
            // Get the minutes
            minutes = ((int)timeLeft) / 60;

            // Get the seconds
            seconds = ((int)timeLeft) % 60;
            string textSeconds = "";

            // if the seconds only have 1 digit
            if (seconds < 10)
            {
                // Add a 0 before
                textSeconds = "0" + seconds;
            }
            else
            {
                textSeconds = seconds.ToString();
            }

            timeText.text = minutes + ":" + textSeconds;

            // Wait some time for the next update
            yield return new WaitForSeconds(0.33f);
        }

        // When it has finished, set the counter in 0
        timeText.text = "0:00";
    }
}
