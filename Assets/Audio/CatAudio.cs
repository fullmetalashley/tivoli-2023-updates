using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Control the random audio values for the cat.
public class CatAudio : MonoBehaviour
{
    //SCRIPT REFS
    private SFXController soundEffects;

    //Audio clips
    public List<SoundEffect> catClips;
    public List<SoundEffect> catPurrs;

    //Ints to track our timer.
    public float maxWaitTime;
    public float minWaitTime;
    public float currentTime;

    //DEBUG bool for when we are manually set to nighttime.
    public bool debugAllowed = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (CatAllowed() && FindObjectOfType<DataManager>().SFXOn && debugAllowed)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                //Our timer has run out, play a random sound.

                int randomSound = Random.Range(0, catClips.Count - 1);
                FindObjectOfType<SFXController>().PlayPassed(catClips[randomSound]);

                currentTime = Random.Range(minWaitTime, maxWaitTime);
            }
        }
    }

    //Play the purr sound effect when the cat is clicked.
    public void PlayPurr()
    {
        FindObjectOfType<SFXController>().PlayPassed(catPurrs[0]);
    }

    //Is it earlier than 6 PM?
    public bool CatAllowed()
    {
        int currentTime = System.DateTime.Now.Hour;

        if (currentTime < 18)
        {
            //It's still daytime.
            return true;
        }
        return false;
    }
}
