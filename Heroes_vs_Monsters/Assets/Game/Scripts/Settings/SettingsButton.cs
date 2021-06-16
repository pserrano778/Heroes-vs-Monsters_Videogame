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
        HideSettingsMenu();
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
        settingsMenu.gameObject.SetActive(true);
    }

    public void HideSettingsMenu()
    {
        settingsMenu.gameObject.SetActive(false);
    }
}
