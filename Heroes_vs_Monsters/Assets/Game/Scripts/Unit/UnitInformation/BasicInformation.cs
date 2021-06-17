using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInformation : MonoBehaviour
{
    public string description;
    public bool hasUltimate = false;
    public bool hasPasive = false;
    public string pasiveDescription;
    public string ultimateDescription;

    private void Start()
    {
        description = description.Replace("@", System.Environment.NewLine);
        pasiveDescription = pasiveDescription.Replace("@", System.Environment.NewLine);
        ultimateDescription = ultimateDescription.Replace("@", System.Environment.NewLine);
    }
}
