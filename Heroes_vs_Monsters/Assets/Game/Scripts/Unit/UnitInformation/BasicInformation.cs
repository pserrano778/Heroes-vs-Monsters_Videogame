using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInformation : MonoBehaviour
{
    public string description;
    public bool hasUltimate = false;
    public bool hasPassive = false;
    public string passiveDescription;
    public string ultimateDescription;

    private void Start()
    {
        // Set the general, passive and ultimate descriptions replacing @ with end of line
        description = description.Replace("@", System.Environment.NewLine);
        passiveDescription = passiveDescription.Replace("@", System.Environment.NewLine);
        ultimateDescription = ultimateDescription.Replace("@", System.Environment.NewLine);
    }
}
