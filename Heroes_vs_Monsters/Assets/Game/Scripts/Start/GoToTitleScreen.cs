using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToTitleScreen : MonoBehaviour
{
    public void GoToTitleScreenMenu()
    {
        // Load the Title Screen Scene
        SceneManager.LoadScene("TitleScreen");
    }
}
