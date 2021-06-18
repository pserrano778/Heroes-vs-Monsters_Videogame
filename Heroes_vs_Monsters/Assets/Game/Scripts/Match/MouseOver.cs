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
        unitInformation.SetActive(false);
    }

    private void Update()
    {
        Collider2D colliderHit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), LayerMask.GetMask("Spawn"));
        if (colliderHit)
        {
            if (colliderHit.tag == "Prefab")
            {
                UnitBehaviour prefab = colliderHit.GetComponent<UnitBehaviour>();
                BasicInformation prefabInformation = colliderHit.GetComponent<BasicInformation>();

                if (prefab.name != currentPrefabName)
                {
                    string info = "Description: " + prefabInformation.description + System.Environment.NewLine
                        + "Damage: " + prefab.damage + System.Environment.NewLine
                        // + "Range: " + prefab.attackRange + System.Environment.NewLine
                        + "Defense: " + prefab.defense;

                    if (prefabInformation.hasPasive)
                    {
                        info += System.Environment.NewLine + "Passive: " + prefabInformation.pasiveDescription;
                    }

                    if (prefab.GetComponent<BasicInformation>().hasUltimate)
                    {
                        info += System.Environment.NewLine + "Ultimate: " + prefabInformation.ultimateDescription;
                    }

                    informationText.text = info;
                    currentPrefabName = prefab.name;
                    nameText.text = prefab.name;
                    
                    unitInformation.SetActive(true);
                    informationActive = true;
                }
            }

            else
            {
                print("No se ha pulsado sobre nada");
            }
        }
        else if(informationActive)
        {
            currentPrefabName = "";
            informationActive = false;
            unitInformation.SetActive(false);
        }
    }
}
