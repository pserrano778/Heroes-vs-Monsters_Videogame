using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTypeOfResources : MonoBehaviour
{
    public Image resourceIcon;

    // Start is called before the first frame update
    void Start()
    {
        if (NetworkManager.GetTypeOfPlayer() == "Heroes" )
        {
            resourceIcon.sprite = Resources.Load<Sprite>("Img/Gold");
        }
        else
        {
            resourceIcon.sprite = Resources.Load<Sprite>("Img/VoidEnergy");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
