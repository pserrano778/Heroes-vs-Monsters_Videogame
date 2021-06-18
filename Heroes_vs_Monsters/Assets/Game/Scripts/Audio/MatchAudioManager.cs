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
        // Play the match theme
        matchTheme.Play();
    }
    
    public void PlayWinTheme()
    {
        // Stop the match theme
        matchTheme.Stop();

        // Play the win theme
        winTheme.Play();
    }

    public void PlayLoseTheme()
    {
        // Stop the match theme
        matchTheme.Stop();

        // Play the lose theme
        loseTheme.Play();
    }

    public void StopAllMusic()
    {
        // Stop all audio sources
        matchTheme.Stop();
        winTheme.Stop();
        loseTheme.Stop();
    }
}
