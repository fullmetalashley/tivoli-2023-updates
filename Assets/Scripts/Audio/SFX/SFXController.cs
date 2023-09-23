using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Control the sound effects with a play function.
public class SFXController : MonoBehaviour
{
    //DATA INITIALIZATION
    public List<string> soundValues;
    public List<AudioClip> clips;
    public List<SoundEffect> soundEffects;
    public Dictionary<string, AudioClip> audioClips;
    public bool init;

    public static SFXController instance;
    public bool isPlaying;

    //Audio sources
    public AudioSource ambient;
    public AudioSource oneShot;

    public float defaultVolume;

    //The two ambient clips for day and night.
    public List<SoundEffect> timeClips;

    //Keep this object persistent throughout scenes.
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        defaultVolume = oneShot.volume;
        if (!init)
        {
            EstablishPlayerSettings();
            init = true;
        }
    }

    //Determine whether or not sound effects are allowed.
    public void EstablishPlayerSettings(bool setting)
    {
        ambient.mute = !setting;
        oneShot.mute = !setting;

        //Now we need to check and see if it's actually nighttime.
        DataManager playerData = FindObjectOfType<DataManager>();

    }

    //Determine whether or not sound effects are allowed.
    public void EstablishPlayerSettings()
    {
        DataManager playerData = FindObjectOfType<DataManager>();   

        ambient.mute = !playerData.musicOn;
        oneShot.mute = !playerData.SFXOn;

    }

    //Play the specific sound when called.
    public void PlaySpecific(string sound)
    {
        oneShot.volume = ClipToPlay(sound).volume;
        oneShot.PlayOneShot(ClipToPlay(sound).clip);
    }

    //Play a sound effect, not just an audio clip.
    public void PlayPassed(SoundEffect clip)
    {
        if (oneShot.clip != null)
        {
            //If we aren't already playing this sound...
            if (oneShot.clip != clip.clip)
            {
                oneShot.volume = clip.volume;
                oneShot.PlayOneShot(clip.clip);
            }
        }
        else
        {
            oneShot.volume = clip.volume;
            oneShot.PlayOneShot(clip.clip);
        }
    }

    //Called during a scene transition.
    public void StopOneShot()
    {
        oneShot.Stop();
    }

    //Find the designated sound effect and play it.
    public SoundEffect ClipToPlay(string key)
    {
        for (int i =0; i < soundEffects.Count; i++)
        {
            if (soundEffects[i].name == key)
            {
                return soundEffects[i];
            }
        }
        return null;
    }

    //Check what time it is, and play the appropriate ambient track.
    public void TimeToggle()
    {
        //This means it's nighttime.
        if (FindObjectOfType<DataManager>().currentPlayedRealTime.Hour > 18 || FindObjectOfType<DataManager>().currentPlayedRealTime.Hour <= 6)
        {
            ambient.clip = timeClips[1].clip;
        }
        else
        {
            ambient.clip = timeClips[0].clip;
        }

        ambient.Play();

        //Are we allowed to play this track?
        if (FindObjectOfType<DataManager>().SFXOn)
        {
            ambient.mute = false;

        }
        else
        {
            ambient.mute = true;
        }
    }

    //Check what time it is, and play the appropriate ambient track.
    public void TimeToggleDebug(bool daytime)
    {
        //This means it's day.
        if (daytime)
        {
            ambient.clip = timeClips[0].clip;
        }
        else
        {
            ambient.clip = timeClips[1].clip;
        }

        ambient.Play();

        //Are we allowed to play this track?
        if (FindObjectOfType<DataManager>().SFXOn)
        {
            ambient.mute = false;

        }
        else
        {
            ambient.mute = true;
        }
    }
}
