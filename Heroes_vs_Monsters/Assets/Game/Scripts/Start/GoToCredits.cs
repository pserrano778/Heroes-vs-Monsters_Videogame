using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToCredits : MonoBehaviour
{
    public void GoToCreditsMenu()
    {
        // Load the Credits Scene
        SceneManager.LoadScene("Credits");
    }
}
