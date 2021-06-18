using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GoToMainMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        // Unpause the game
        Time.timeScale = 1;

        // Disconnect from the network
        PhotonNetwork.Disconnect();

        // Load Lobby scene
        PhotonNetwork.LoadLevel("Lobby");
    }
}
