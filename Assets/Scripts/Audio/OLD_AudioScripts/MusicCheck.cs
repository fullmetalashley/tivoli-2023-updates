using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCheck : MonoBehaviour
{
    public bool allowed;
    // Start is called before the first frame update
    void Start()
    {
        
        FindObjectOfType<PersistentAudio>().gameObject.GetComponent<AudioSource>().mute = allowed;
        Debug.Log("Current status of mute: " + FindObjectOfType<PersistentAudio>().gameObject.GetComponent<AudioSource>().mute);
    }
}
