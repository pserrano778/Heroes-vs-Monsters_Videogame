using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour
{

    private UnitBehaviour prefab;
    private string currentPrefabName;
    public TextMesh nameText;
    public TextMesh damageValueText;
    public TextMesh defenseValueText;
    public TextMesh descText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        Collider2D colliderHit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), LayerMask.GetMask("Spawn"));
        if (colliderHit)
        {
            if (colliderHit.tag == "Prefab")
            {
                prefab = colliderHit.GetComponent<UnitBehaviour>();

                if(prefab.name != currentPrefabName)
                {
                    currentPrefabName = prefab.name;
                    nameText.text = prefab.name;
                    damageValueText.text = prefab.damage.ToString();
                    defenseValueText.text = prefab.defense.ToString();
                }
            }

            else
            {
                print("No se ha pulsado sobre nada");
            }
        }
        
    }
}
