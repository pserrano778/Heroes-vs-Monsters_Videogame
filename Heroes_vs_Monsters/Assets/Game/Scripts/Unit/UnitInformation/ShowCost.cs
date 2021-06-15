using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCost : MonoBehaviour
{
    public TextMesh cost;
    // Start is called before the first frame update
    void Start()
    {
        cost.text = GetComponentInParent<UnitBehaviour>().cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
