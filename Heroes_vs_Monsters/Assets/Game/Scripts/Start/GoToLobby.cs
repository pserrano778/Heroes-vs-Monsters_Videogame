using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLobby : MonoBehaviour
{
    public void GoToLobbyMenu()
    {
        // Load the Lobby Scene
        SceneManager.LoadScene("Lobby");
    }
}
