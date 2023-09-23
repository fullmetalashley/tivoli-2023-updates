using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Turn on the fireplace audio when the scene is in nighttime.
public class FireAudio : MonoBehaviour
{
    //SCRIPT REFS
    private SFXController soundEffects;

    //Audio clips
    public SoundEffect fireSound;

    private void Start()
    {
        soundEffects = FindObjectOfType<SFXController>();
    }

    //Can we be running fire right now?  This is asked by the passive elements manager.
    public void CheckForFire()
    {
        if (FindObjectOfType<DataManager>().SFXOn)
        {
            //We are allowed to play fire.
        }
    }
}
