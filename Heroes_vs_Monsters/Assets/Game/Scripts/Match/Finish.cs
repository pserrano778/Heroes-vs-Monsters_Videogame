using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviourPunCallbacks
{
    static private Button mainMenuButton;
    static private bool hasExited = false;
    // Start is called before the first frame update
    void Start()
    {
        hasExited = false;
        mainMenuButton = GetComponent<Button>();
        mainMenuButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public void GameOver(string winner)
    {
        if (!hasExited)
        {
            Time.timeScale = 0;
            if (winner == "Monsters")
            {
                print("GANAN LOS MONSTRUOS");
            }
            else
            {
                print("GANAN LOS HÉROES");
            }

            PhotonNetwork.LeaveRoom();
            mainMenuButton.gameObject.SetActive(true);
            hasExited = true;
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Lobby");
    }
}
