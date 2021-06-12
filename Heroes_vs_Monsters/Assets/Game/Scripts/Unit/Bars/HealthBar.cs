using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private BasicBehaviour unit;
    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponentInParent<BasicBehaviour>();
        if ((NetworkManager.GetTypeOfPlayer() == "Heroes" && (unit.tag == "Hero" || unit.tag == "Nexus")) || 
            (NetworkManager.GetTypeOfPlayer() == "Monsters" && unit.tag == "Monster"))
        {
            healthBar.sprite = Resources.Load<Sprite>("Img/healthBar3");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.localScale.x < 0)
        {
            healthBar.fillOrigin = 1;
        }
        else
        {
            healthBar.fillOrigin = 0;
        }
        float currentHealth = unit.getCurrentHealth();
        float maxHealth = unit.health;
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0 && unit.tag != "Nexus")
        {
            gameObject.active = false;
        }
    }
}
