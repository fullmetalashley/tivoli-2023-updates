using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

//SCRIPT PURPOSE: Assists in controlling the dynamic music system utilizing the Unity Audio Mixer.
public class PersistentAudio : MonoBehaviour
{
    public static PersistentAudio control;  //Assists in deleting this object if it already exists.
    public bool listInit;

    //MUSIC WORK
    public AudioMixer audioMixer;
    public Dictionary<string, AudioClip> audioTracks;
    public AudioSource player;

    public AudioMixerSnapshot[] fullAudio, muteBGM;
    public float audioTransitionTime;
    public float delayedStartTime;


    //Lists used for setup for the audio dictionary.
    public AudioClip[] audioValues;
    public string[] sceneValues;

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
    }

    //Called by scene loading tools.
    public void ChangeMusic(string scene)
    {
        Debug.Log("Scene to switch to: " + scene);

        if (!listInit)
        {
            InitializeLists();
        }
        StartCoroutine(RunChangeMusic(scene));
    }

    //Coroutine that actually engages the music.
    IEnumerator RunChangeMusic(string scene)
    {
        if (player.clip == audioTracks[scene])
        {
            //We're already playing the right music, so exit the loop.
            yield break;
        }
        else
        {
            audioMixer.TransitionToSnapshots(muteBGM, new float[] { .5f }, audioTransitionTime);

            
            yield return new WaitForSeconds(delayedStartTime);

            player.clip = audioTracks[scene];
            player.Play();

            audioMixer.TransitionToSnapshots(fullAudio, new float[] { 1.0f }, audioTransitionTime);
            
        }
    }
}
