using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GoToMainMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Lobby");
    }
}
