using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public GameObject settingsMenu;
    private bool settingMenuActive = false;

    public void Start()
    {
        // Hide the settings menu at start
        HideSettingsMenu();
    }

    public void ChangeSettingsMenuVisibility()
    {
        // If the menu is active
        if (settingMenuActive)
        {
            // Hide it
            HideSettingsMenu();
        }
        else // It is not active
        {
            // Show it
            ShowSettingsMenu();
        }

        // Shange status
        settingMenuActive = !settingMenuActive;
    }

    public void ShowSettingsMenu()
    {
        // Enable setting menu object
        settingsMenu.gameObject.SetActive(true);
    }

    public void HideSettingsMenu()
    {
        // Disable setting menu object
        settingsMenu.gameObject.SetActive(false);
    }
}
