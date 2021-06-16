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

    private bool hasExited = false;

    // Start is called before the first frame update
    void Start()
    {
        hasExited = false;
        endGameText.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
    }

    public void GameOver(string winner)
    {
        if (!hasExited)
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

            PhotonNetwork.LeaveRoom();
            mainMenuButton.gameObject.SetActive(true);
            hasExited = true;
        }
    }
}
