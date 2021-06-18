using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour
{
    private string currentPrefabName;
    public TextMesh nameText;
    public TextMesh informationText;
    public GameObject unitInformation;

    private bool informationActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // Disable unit Information at start
        unitInformation.SetActive(false);
    }

    private void Update()
    {
        // Check mouse collisions
        Collider2D colliderHit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), LayerMask.GetMask("Spawn"));

        // If it has collided
        if (colliderHit)
        {
            // If it has collided with a prefab
            if (colliderHit.tag == "Prefab")
            {
                // Store the prefab and its information
                UnitBehaviour prefab = colliderHit.GetComponent<UnitBehaviour>();
                BasicInformation prefabInformation = colliderHit.GetComponent<BasicInformation>();

                // If the prefab is different from the previous one
                if (prefab.name != currentPrefabName)
                {
                    // Store prefab information in a string
                    string info = "Description: " + prefabInformation.description + System.Environment.NewLine
                        + "Damage: " + prefab.damage + System.Environment.NewLine
                        // + "Range: " + prefab.attackRange + System.Environment.NewLine
                        + "Defense: " + prefab.defense;

                    // Check if it has passive
                    if (prefabInformation.hasPassive)
                    {
                        // If it has, update information
                        info += System.Environment.NewLine + "Passive: " + prefabInformation.passiveDescription;
                    }

                    // Check if it has ultimate
                    if (prefab.GetComponent<BasicInformation>().hasUltimate)
                    {
                        // If it has, update information
                        info += System.Environment.NewLine + "Ultimate: " + prefabInformation.ultimateDescription;
                    }

                    // Update the text object
                    informationText.text = info;

                    // Store current prefab name
                    currentPrefabName = prefab.name;

                    // Update name text Object
                    nameText.text = prefab.name;
                    
                    // Activate the information display
                    unitInformation.SetActive(true);
                    informationActive = true;
                }
            }
        }
        else if(informationActive) // If the display is active
        {
            // Remove current prefab name
            currentPrefabName = "";

            // Disable the display
            informationActive = false;
            unitInformation.SetActive(false);
        }
    }
}
