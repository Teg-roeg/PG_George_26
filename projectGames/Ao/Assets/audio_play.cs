using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class audio_play : MonoBehaviour
{
    public AudioSource aud;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        playAduio();
    }

    void playAduio() { 
        aud.Play();
    }
}

