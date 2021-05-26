using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManagement : MonoBehaviour
{
    private const float TICK_TIME = 5.0f;
    private float elapsedTime;
    private int resourcesPerTick;
    private int resources;
    public TextMeshProUGUI resourceCounterText;


    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
        resources = 0;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime % TICK_TIME >= 0)
        {
            elapsedTime -= TICK_TIME;
            resources += resourcesPerTick;
            UpdateCounterText();
        }
    }

    public int GetResources()
    {
        return resources;
    }

    public void DecreaseResources(int cost)
    {
        resources = resources - cost;
    }

    public void SetResourcesPerTick(int newResourcesPerTick)
    {
        resourcesPerTick = newResourcesPerTick;
    }

    public void UpdateCounterText ()
    {
        resourceCounterText.text = resources.ToString();
    }
}
