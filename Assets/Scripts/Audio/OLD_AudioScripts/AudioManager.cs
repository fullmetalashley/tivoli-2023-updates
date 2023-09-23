using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Would like to kill this in each scene instance and utilize the persistent SFX manager instead.
public class AudioManager : MonoBehaviour
{
    public AudioClip buttonClick;
    AudioSource thisAudio;

    public AudioClip dollSwap;
    public AudioClip paperRustle;

    // Start is called before the first frame update
    void Start()
    {
        thisAudio = GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonSound()
    {
        thisAudio.PlayOneShot(buttonClick);
    }

    public void PlayDollSwap()
    {
        thisAudio.PlayOneShot(dollSwap);
    }

    public void PlayPaper()
    {
        thisAudio.PlayOneShot(paperRustle);
    }
}
