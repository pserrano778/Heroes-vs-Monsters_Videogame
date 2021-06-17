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
        hasFinished = false;
        endGameText.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
    }

    public void GameOver(string winner)
    {
        if (!hasFinished)
        {
            endGameText.SetActive(true);
            Time.timeScale = 0;
            if (NetworkManager.GetTypeOfPlayer() == winner)
            {
                endGameText.GetComponent<TextMesh>().text = NetworkManager.GetTypeOfPlayer() + " win";
                audioManager.PlayWinTheme();
            }
            else{
                endGameText.GetComponent<TextMesh>().text = NetworkManager.GetTypeOfPlayer() + " lose";
                audioManager.PlayLoseTheme();
            }

            mainMenuButton.gameObject.SetActive(true);
            hasFinished = true;
        }
    }
}
