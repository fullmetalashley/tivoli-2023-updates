using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PersistentCrossfade : MonoBehaviour
{

    public static PersistentCrossfade control;  //Assists in deleting this object if it already exists.
    public bool listInit;

    //MUSIC WORK
    public AudioMixer audioMixer;
    public Dictionary<string, AudioClip> audioTracks;

    public AudioSource player;
    public AudioSource player2;


    public float audioTransitionTime;

    //Lists used for setup for the audio dictionary.
    public AudioClip[] audioValues;
    public string[] sceneValues;

    public float musicVolume;

    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Setup the dictionary.
    public void InitializeLists()
    {
        //Sets up the dictionary containing our audio tracks.
        audioTracks = new Dictionary<string, AudioClip>();
        for (int i = 0; i < sceneValues.Length; i++)
        {
            audioTracks.Add(sceneValues[i], audioValues[i]);
        }
        listInit = true;

        player.volume = musicVolume;
        player2.volume = 0f;
    }

    //Read player settings and determine whether or not this is muted.
    public void EstablishPlayerSettings(bool setting)
    {
        player.mute = !setting;
        player2.mute = !setting;
    }

    //Called by scene loading tools.
    public void ChangeMusic(string scene)
    {

        if (!listInit)
        {
            InitializeLists();
        }

        //Let's also mute the bird audio as needed.
        SFXController sfx = FindObjectOfType<SFXController>();
        if (sfx != null)
        {
            if (scene != "Story Map")
            {
                sfx.ambient.mute = true;
            }
            else
            {
                //Turn the ambient noise on or off if the player allows it.
                sfx.ambient.mute = !FindObjectOfType<DataManager>().SFXOn;
            }
        }

        //FIRST: Do we need to switch music?  Is either player currently playing this track?
        if (player.clip == audioTracks[scene] || player2.clip == audioTracks[scene])
        {
            //One of the players is currently playing this song, so do nothing.
        }
        else
        {
            //Determine which player is playing.
            if (player.isPlaying)
            {
                //This player is currently on, so it becomes track 1.  Track 2 will have the new music.
                player2.clip = audioTracks[scene];
                StartCoroutine(Crossfade(player, player2, audioTransitionTime));
            }
            else
            {
                player.clip = audioTracks[scene];
                StartCoroutine(Crossfade(player2, player, audioTransitionTime));
            }
        }
    }

    IEnumerator Crossfade(AudioSource track1, AudioSource track2, float interval)
    {
        //Calculate the duration of each step.
        float stepInterval = interval / 20.0f;
        float volInterval = musicVolume / 20.0f;

       track2.Play();

        //Fade between the two, taking A to 0 and b to full volume
        for (int i = 0; i < 20; i++)
        {
            track1.volume -= volInterval;
            track2.volume += volInterval;

            //Wait one interval, then do it again
            yield return new WaitForSeconds(stepInterval);
        }
        track1.Stop();
        track1.clip = null;

        yield break;
    }
}
