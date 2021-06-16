using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchAudioManager : MonoBehaviour
{
    public AudioSource matchTheme;
    public AudioSource winTheme;
    public AudioSource loseTheme;

    // Start is called before the first frame update
    void Start()
    {
        matchTheme.Play();
    }
    
    public void PlayWinTheme()
    {
        matchTheme.Stop();
        winTheme.Play();
    }

    public void PlayLoseTheme()
    {
        matchTheme.Stop();
        loseTheme.Play();
    }

    public void StopAllMusic()
    {
        matchTheme.Stop();
        winTheme.Stop();
        loseTheme.Stop();
    }
}
