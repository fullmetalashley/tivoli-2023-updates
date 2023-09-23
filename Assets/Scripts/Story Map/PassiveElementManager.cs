using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

//Script purpose: Update the story map based on the appropriate time of day.
public class PassiveElementManager : MonoBehaviour
{
    //All of the sprites for the environment that will change based on the time of day.
    public List<Sprite> backdropAssets;
    public List<Sprite> jewelryAssets;
    public List<Sprite> wardrobeAssets;
    public List<Sprite> writingAssets;
    public List<Sprite> book1Assets;
    public List<Sprite> book2Assets;
    public List<Sprite> galleryAssets;

    //The strings that reference the times of day that will help establish the dictionaries.
    public List<String> stateChanges;

    //The image references for the objects that will change their sprites multiple times.
    public Image backdrop;
    public Image jewelry;
    public Image wardrobe;
    public Image writing;
    public Image book1;
    public Image book2;
    public Image gallery;

    //The dictionaries that will store the sprites and strings accordingly.
    public Dictionary<string, Sprite> backdropChanges;
    public Dictionary<string, Sprite> jewelryChanges;
    public Dictionary<string, Sprite> wardrobeChanges;
    public Dictionary<string, Sprite> writingChanges;
    public Dictionary<string, Sprite> book1Changes;
    public Dictionary<string, Sprite> book2Changes;
    public Dictionary<string, Sprite> galleryChanges;

    private DataManager playerData;

    //Changes the main owl icon based on the time of day.
    public Image mainMenuOwl;
    public Sprite dayOwl;
    public Sprite nightOwl;

    //Activates the animations.
    public GameObject fire;
    public GameObject cat;

    public GameObject dustParticles;
    public GameObject fireParticles;

    //A BOOL FOR DEBUGGING THE TIME
    public bool daytime;

    //Mail icon info
    public GameObject unreadMailIndicator;

    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();

        backdropChanges = new Dictionary<string, Sprite>();
        jewelryChanges = new Dictionary<string, Sprite>();
        wardrobeChanges = new Dictionary<string, Sprite>();
        writingChanges = new Dictionary<string, Sprite>();
        book1Changes = new Dictionary<string, Sprite>();
        book2Changes = new Dictionary<string, Sprite>();
        galleryChanges = new Dictionary<string, Sprite>();


        //Establish the dictionaries for each set of images.
        for (int i = 0; i < stateChanges.Count; i++)
        {
            backdropChanges.Add(stateChanges[i], backdropAssets[i]);
            jewelryChanges.Add(stateChanges[i], jewelryAssets[i]);
            wardrobeChanges.Add(stateChanges[i], wardrobeAssets[i]);
            writingChanges.Add(stateChanges[i], writingAssets[i]);
            book1Changes.Add(stateChanges[i], book1Assets[i]);
            book2Changes.Add(stateChanges[i], book2Assets[i]);
            galleryChanges.Add(stateChanges[i], galleryAssets[i]);
        }

        if (playerData.unreadLettersProcessed < (FindObjectOfType<MailDatabase>().MailToRead()))
        {
            unreadMailIndicator.SetActive(true);
        }

        UpdatePassives();
    }

    //Update all of the story map images based on the right time of day.
    public void UpdatePassives()
    {
        if (playerData.unreadLettersProcessed < (FindObjectOfType<MailDatabase>().MailToRead()))
        {
            unreadMailIndicator.SetActive(true);
        }

        //This check is a DEBUG ONLY check.  Remove it for the final build.
        if (!playerData.timeOverride)
        {
            FindObjectOfType<SFXController>().TimeToggle();

            int newHour = System.DateTime.Now.Hour;
            //If it is between 6 PM and 6 AM, set to nighttime.

            if (6 < newHour && newHour < 18)
            {
                mainMenuOwl.sprite = dayOwl;
                cat.SetActive(true);
                dustParticles.SetActive(true);

                fire.SetActive(false);
                fireParticles.SetActive(false);
                SwitchPassives("Afternoon");
            }
            else
            {
                mainMenuOwl.sprite = nightOwl;
                cat.SetActive(false);
                dustParticles.SetActive(false);

                fire.SetActive(true);
                fireParticles.SetActive(true);
                SwitchPassives("Night");
            }
        }
        else
        {
            UpdatePassivesDebug();
        }
    }

    //Update all of the story map images based on the right time of day.
    public void UpdatePassivesDebug()
    {
        daytime = FindObjectOfType<DebugClock>().daytime;
        FindObjectOfType<SFXController>().TimeToggleDebug(daytime);
        if (daytime)
        {
            mainMenuOwl.sprite = dayOwl;
            cat.SetActive(true);
            dustParticles.SetActive(true);

            fire.SetActive(false);
            fireParticles.SetActive(false);
            SwitchPassives("Afternoon");
        }
        else
        {
            mainMenuOwl.sprite = nightOwl;
            cat.SetActive(false);
            dustParticles.SetActive(false);

            fire.SetActive(true);
            fireParticles.SetActive(true);
            SwitchPassives("Night");
        }
    }

    //Updates the appropriate sprites based on a string.
    public void SwitchPassives(string time)
    {
        jewelry.sprite = jewelryChanges[time];
        writing.sprite = writingChanges[time];
        wardrobe.sprite = wardrobeChanges[time];
        backdrop.sprite = backdropChanges[time];

        book1.sprite = book1Changes[time];
        book2.sprite = book2Changes[time];
        gallery.sprite = galleryChanges[time];
    }

    //Toggles the daytime / nighttime particles on and off based on whether or not the main menu is open.
    public void ToggleParticles(bool status)
    {
        //If true, that means the main menu is on.
        dustParticles.SetActive(!status);
        fireParticles.SetActive(!status);

        //If the main menu is off, update the passives again to make sure the right things are operating.
        if (!status)
        {
            UpdatePassives();
        }
    }
}
