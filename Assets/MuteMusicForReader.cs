using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Turn off the eReader once the music has been played in here.
public class MuteMusicForReader : MonoBehaviour
{
    //Script refs
    PersistentCrossfade music;

    //Float for music length
    public float trackLength;
    public float timer;

    //Bools to track where players started
    public bool music1;
    public bool music2;

    public bool musicEnded;

    // Start is called before the first frame update
    void Start()
    {
        music = FindObjectOfType<PersistentCrossfade>();
        timer = trackLength;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && !musicEnded)
        {
            musicEnded = true;
            if (music.player.isPlaying)
            {
                music1 = true;
                music2 = false;
                music.player.mute = true;
            }

            if (music.player2.isPlaying)
            {
                music1 = false;
                music2 = true;
                music.player2.mute = true;
            }
        }
    }

    //Turn the music back on when we leave the scene.
    public void MusicBackOn()
    {
        music.player.mute = music1;
        music.player2.mute = music2;
    }
}
