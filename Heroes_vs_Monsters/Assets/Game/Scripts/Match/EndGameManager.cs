using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviourPunCallbacks
{
    public Button mainMenuButton;
    public MatchAudioManager audioManager;
    public GameObject endGameText;

    private bool hasFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set finished status and interface to false
        hasFinished = false;
        endGameText.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
    }

    public void GameOver(string winner)
    {
        // If it has not finished
        if (!hasFinished)
        {
            // Active end Game text
            endGameText.SetActive(true);

            // Stop the game
            Time.timeScale = 0;

            // If I am the winner
            if (NetworkManager.GetTypeOfPlayer() == winner)
            {
                // Change end game Text
                endGameText.GetComponent<TextMesh>().text = NetworkManager.GetTypeOfPlayer() + " win";

                // Play the winner theme
                audioManager.PlayWinTheme();
            }
            else{ // If I am the loser

                // Change end game Text
                endGameText.GetComponent<TextMesh>().text = NetworkManager.GetTypeOfPlayer() + " lose";

                // Play the loser theme
                audioManager.PlayLoseTheme();
            }

            // Activate the button to return to the main menu
            mainMenuButton.gameObject.SetActive(true);

            // Set finished state to true
            hasFinished = true;
        }
    }
}
