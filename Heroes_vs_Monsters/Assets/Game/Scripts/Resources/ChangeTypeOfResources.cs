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
        // If the player is using Heroes
        if (NetworkManager.GetTypeOfPlayer() == "Heroes" )
        {
            // Set the Gold coin
            resourceIcon.sprite = Resources.Load<Sprite>("Img/Gold");
        }
        else // If he is playing with Monsters
        {
            // Set the void Energy icon
            resourceIcon.sprite = Resources.Load<Sprite>("Img/VoidEnergy");
        }
    }
}
