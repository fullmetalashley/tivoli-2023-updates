using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script is intended to talk to all other necessary startup scripts to ensure that everything is happened in the appropriate 
//order without causing null errors or anything to that effect.
public class GameInitializer : MonoBehaviour
{
    //The scripts that will need to be called in a sequential order.
    public ClockTracker mainClock;
    public StartupDataLoad initialDataLoad;
    public DataManager playerData;
    public AssetLoader assetLoad;

    public MailDatabase theMail;
    public ClothingDatabase theClothing;
    public TableauDatabase theTableaus;
    public JewelryDatabase theJewelry;
    public AchievementDatabase achievements;
    public PersistentAudio persistentAudio;

    public Text continueButton;

    public MiniToolTipControl thisTip;

    // Start is called before the first frame update
    void Start()
    {
        mainClock = FindObjectOfType<ClockTracker>();
        initialDataLoad = FindObjectOfType<StartupDataLoad>();
        playerData = FindObjectOfType<DataManager>();
        assetLoad = FindObjectOfType<AssetLoader>();

        theMail = FindObjectOfType<MailDatabase>();
        theClothing = FindObjectOfType<ClothingDatabase>();
        theTableaus = FindObjectOfType<TableauDatabase>();
        theJewelry = FindObjectOfType<JewelryDatabase>();
        achievements = FindObjectOfType<AchievementDatabase>();
        persistentAudio = FindObjectOfType<PersistentAudio>();

        ResetButton();

        StartProcess();
    }

    //When the player starts the game, the reset button will assess if the player has data.
    public void ResetButton()
    {
        if (playerData.GetComponent<SaveSystem>().PlayerDataExists())
        {
            continueButton.text = "Continue";
        }
        else
        {
            continueButton.text = "Begin";
        }
    }

    public void StartProcess()
    {
        //All assets are loaded.
        assetLoad.InitializeLoad();

        //Player data is loaded.
        initialDataLoad.Loading();

        //Now, get the time values moving.
        mainClock.InitializeValues();

        //Then, run the date updates for every single database.
        theMail.UpdateLists(playerData.currentPlayedInGame);        
        theMail.InitializeReadMail();

        theClothing.UpdateLists(playerData.currentPlayedInGame);
        theTableaus.UpdateLists(playerData.currentPlayedInGame);
        theJewelry.UpdateLists(playerData.currentPlayedInGame);
        achievements.SetAchievements();

        playerData.mailAccessed = false;

        ResetButton();

        //Set the sound effects for the player data.
//        FindObjectOfType<SFXController>().EstablishPlayerSettings(playerData.SFXOn);
        FindObjectOfType<PersistentCrossfade>().EstablishPlayerSettings(playerData.musicOn);

 //       persistentAudio.ChangeMusic("Start Screen");
    }
}
