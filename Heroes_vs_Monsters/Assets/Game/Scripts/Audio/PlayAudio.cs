using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        // Play audio at start
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
