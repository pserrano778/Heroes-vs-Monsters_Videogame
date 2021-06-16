using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public GameObject settingsMenu;
    public Canvas mainMenu;

    static private bool settingMenuActive = false;


    public void Start()
    {
        // settingMenuActive = false;
    }

    public void ChangeSettingsMenuVisibility()
    {
        if (settingMenuActive)
        {
            HideSettingsMenu();
        }
        else
        {
            ShowSettingsMenu();
        }

        settingMenuActive = !settingMenuActive;
    }

    public void ShowSettingsMenu()
    {
        mainMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    }

    public void HideSettingsMenu()
    {
        settingsMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
}
