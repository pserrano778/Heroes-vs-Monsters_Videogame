using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCost : MonoBehaviour
{
    public TextMesh cost;

    // Start is called before the first frame update
    void Start()
    {
        // Set the cost text using the Unit cost
        cost.text = GetComponentInParent<UnitBehaviour>().cost.ToString();
    }
}
