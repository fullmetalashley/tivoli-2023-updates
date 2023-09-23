using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Disable the audio on the story map if the player has been idle for X number of seconds.
public class IdleTracking : MonoBehaviour
{
    //SCRIPT REFS
    public PersistentCrossfade musicControls;

    //Timers
    public float timer;
    public float currentTimer;

    //Original bools prior to mute
    public bool player1;
    public bool player2;

    // Start is called before the first frame update
    void Start()
    {
        musicControls = FindObjectOfType<PersistentCrossfade>();

        //Set our bools when we land on the start screen.
        player1 = musicControls.player.mute;
        player2 = musicControls.player2.mute;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer -= Time.deltaTime;
        //If ANY movement or clicks are detected...
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) ||
            Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") > 0 || Input.GetAxis("Mouse Y") < 0)

        {
            //Reset the timer.
            currentTimer = timer;

            musicControls.player.mute = player1;
            musicControls.player2.mute = player2;
        }

        //We have hit the idle threshold, so turn off the music.
        if (currentTimer <= 0)
        {
            musicControls.player.mute = true;
            musicControls.player2.mute = true;
        }
    }
}
