using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthBar : MonoBehaviour
{
    private BasicBehaviour unit;
    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Basic Behaviour component
        unit = GetComponent<BasicBehaviour>();

        // Change the health bar colour to the allied Units (or nexus if player is using heroes)
        if ((NetworkManager.GetTypeOfPlayer() == "Heroes" && (unit.tag == "Hero" || unit.tag == "Nexus")) || 
            (NetworkManager.GetTypeOfPlayer() == "Monsters" && unit.tag == "Monster"))
        {
            healthBar.sprite = Resources.Load<Sprite>("Img/healthBar3");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Change the bar fill origin to follow the orientation
        if (transform.localScale.x < 0)
        {
            healthBar.fillOrigin = 1;
        }
        else
        {
            healthBar.fillOrigin = 0;
        }

        // Get the current unit health
        float currentHealth = unit.getCurrentHealth();

        // Get the unit max health
        float maxHealth = unit.health;

        // Set the percetage of health
        healthBar.fillAmount = currentHealth / maxHealth;

        // Disable the bar if the unit has died
        if (currentHealth <= 0 && unit.tag != "Nexus" && GetComponent<PhotonView>().IsMine)
        {
            GetComponent<PhotonView>().RPC("DisableHealthBar", RpcTarget.All);
        }
    }

    [PunRPC]
    private void DisableHealthBar()
    {
        healthBar.transform.parent.gameObject.SetActive(false);
    }
}
