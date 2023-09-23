using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

//This script works in tandem with the ClockTracker in order to update content based on the current time.
//This script specifically updates the content, while the ClockTracker stores the times themselves.
//This script is not saved between scenes, and only exists in the Story Map.  Because of this, it updates every time
//the Story Map is loaded.
public class TimeCheck : MonoBehaviour { 

    //The current time value, stored in YYYY/MM/DD format as a DateTime.  The real world time.
    public DateTime currentTime;

    //All necessary script references.
    private PassiveElementManager thePassives;
    private MailController mailUI;
    private ClothingDatabase theClothing;
    private TableauDatabase theTableaus;
    private MailDatabase theMail;
    private JewelryDatabase theJewelry;
    private DataManager theData;
    private ClockTracker mainClock;
    private AdventManager advent;

    //THE FOLLOWING ITEMS are useful for debugging.  They don't appear when the player is in the main menu.
    //The text object that displays the current game day.  Located in the main menu.
    public Text currentGameDay;

    //The text object that shows how many days passed since the last log in.
    public Text daysPassed;


    //A DateTime storing when the player has started playing the game NOW.
    public DateTime loginTime;

    //A DateTime storing when the player last played the game in a separate, closed play session prior to the loginTime.
    public DateTime exitTime;

    //This is a debug dummy value.
    public int[] debugTime;

    public Text newStartDate;

    public DateTime preAdjustmentInGameDate;
    public int preAdjustmentInGameIndex;
    public int preAdjustmentInGameAct;


    // Start is called before the first frame update
    void Start()
    {
        //Initialize all script references.
        thePassives = FindObjectOfType<PassiveElementManager>();
        theClothing = FindObjectOfType<ClothingDatabase>();
        theTableaus = FindObjectOfType<TableauDatabase>();
        theMail = FindObjectOfType<MailDatabase>();
        mainClock = FindObjectOfType<ClockTracker>();
        mailUI = FindObjectOfType<MailController>();
        theJewelry = FindObjectOfType<JewelryDatabase>();
        theData = FindObjectOfType<DataManager>();
        advent = FindObjectOfType<AdventManager>();

        //The current time is set to the system time.  
        currentTime = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day,
            System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);

        //This way, if the player has played before, tool tips are turned off.
        if (theData.dataExists)
        {
            /* Tool tips are currently disabled.  The script attached to the gameObject will need to be turned back on when this is reset.
            CanvasToolTipInterface toolTips = FindObjectOfType<CanvasToolTipInterface>();
            toolTips.DataExists();
            */
        }

        //Once the current time has been set, the passive elements in the room can be changed depending on what time of day it is.
        thePassives.UpdatePassives();

        debugTime = new int[3];
        debugTime[0] = currentTime.Month;
        debugTime[1] = currentTime.Day;
        debugTime[2] = currentTime.Year;

        DateComparison();
        //Every time we land on the story map, the data manager times are updated.
        theData.currentPlayedRealTime = currentTime;


        //First, let's set the actual in game day to the last played in game day.
        mainClock.AdvanceToSpecific(theData.currentPlayedInGame);


        //Let's calculate the time difference between our last real time and our current real time, and use that to establish
        //the current game date.
        mainClock.CalculateTimeDifference(theData.currentPlayedRealTime, theData.lastPlayedRealTime);

        //An update is run depending on the actual in-game time to adjust other content.
        DateCheck(mainClock.currentGameDay);

        //Save the data of the in-game date after it has been updated.
        theData.currentPlayedInGame = mainClock.currentGameDay;

        //Run a check on the advent calendar to make sure we're checking if we're in December.
        FindObjectOfType<AdventManager>().CheckDecemberDay();
    }

    public void DateComparison()
    {
        //We will use the system time for the actual game.
        DateTime newRealTime = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);

        //If we have changed the date at all, very specifically by the int format, we will run the update process.
        if (newRealTime.Month != theData.currentPlayedRealTime.Month || newRealTime.Day != theData.currentPlayedRealTime.Day || newRealTime.Year != theData.currentPlayedRealTime.Year)
        {
            theData.currentPlayedRealTime = newRealTime;
            mainClock.DateUpdate();
        }
    }

    //This DateCheck runs based on the current in-game date stored within this script.
    //NOTE: 1/29/2020.  I don't know that this is even useful.  I believe updates are run from the main clock, as called from the DateComparison method in
    //this script, but not from here.
    //So this might be pointless.
    public void DateCheck()
    {
        theClothing.UpdateLists(currentTime);
        theTableaus.UpdateLists(currentTime);
        theMail.UpdateLists(currentTime);
        advent.CheckDecemberDay();
    }


    //The following functions are purely for debugging purposes.
    //They will most likely be altered or removed based on future debugging needs.
    //This DateCheck runs based on a passed in-game date.  Called from the DummyRestart script.
    public void DateCheck(DateTime newTime)
    {
        theClothing.UpdateLists(newTime);                                   
        theTableaus.UpdateLists(newTime);
        theMail.UpdateLists(newTime);
        advent.CheckDecemberDay();

    }

    //If the game day needs to be moved, it is advanced.  This is called from the Advance Game Day button in the main menu.
    public void ChangeGameDay()
    {
        mainClock.AdvanceGameDay();
        currentGameDay.text = String.Format("{0:yyyy-MM-dd}", mainClock.currentGameDay);
        theData.currentPlayedInGame = mainClock.currentGameDay;
        DateCheck(mainClock.currentGameDay);
    }

    //This will establish when to set the next start time, for debugging.
    public void SetStartTime()
    {
        currentTime = currentTime.AddDays(1.0);

        mainClock.currentGameDayIndex = preAdjustmentInGameIndex;
        mainClock.currentActIndex = preAdjustmentInGameAct;
        mainClock.currentDateList = mainClock.allActs[preAdjustmentInGameAct];
        mainClock.currentGameDay = preAdjustmentInGameDate;

        mainClock.CalculateTimeDifference(currentTime, theData.lastPlayedRealTime);
        theData.currentPlayedInGame = mainClock.currentGameDay;
    }

    public void PreBacklogValues()
    {
        preAdjustmentInGameAct = mainClock.currentActIndex;
        preAdjustmentInGameDate = mainClock.currentGameDay;
        preAdjustmentInGameIndex = mainClock.currentGameDayIndex;
    }
}
