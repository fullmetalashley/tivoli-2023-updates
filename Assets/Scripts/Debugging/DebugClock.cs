using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//Script purpose: Control the functionality of the debug clock.
public class DebugClock : MonoBehaviour
{
    //UI elements
    public Text systemTime;
    public Text systemDate;
    public Text inGameDate;
    public Text lastDayPlayedRealTime;
    public Text lastHourPlayedRealTime;
    public Text currentAct;
    public Text dateMessage;
    public Text achievementToggle;
    public Text timeToggle;
    public Toggle timeControl;

    //Values that will control the clock setting.
    private int hours;
    private int seconds;
    private int minutes;

    private int month;
    private int day;
    private int year;

    //Script refs
    private TimeCheck runningClock;
    private DataManager playerData;
    private ClockTracker mainClock;

    //Some internal bools.
    public bool daytime;


    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();
        mainClock = FindObjectOfType<ClockTracker>();
        runningClock = FindObjectOfType<TimeCheck>();

        //We can get the time from the player data.  It is present on every scene.  The Main clock is only present on the story map.
        UpdateText();
    }

    //Runs the initial update for the text on the game.
    public void UpdateText()
    {
        systemDate.text = "System date: " + playerData.currentPlayedRealTime.Month + "/" + playerData.currentPlayedRealTime.Day + "/" + playerData.currentPlayedRealTime.Year;

        //Minutes don't always display if it's a single digit, so adding the 0 here helps for display purposes.
        if (playerData.currentPlayedRealTime.Minute < 10)
        {
            systemTime.text = "System time: " + playerData.currentPlayedRealTime.Hour + ":0" + playerData.currentPlayedRealTime.Minute;

        }
        else
        {
            systemTime.text = "System time: " + playerData.currentPlayedRealTime.Hour + ":" + playerData.currentPlayedRealTime.Minute;
        }

        inGameDate.text = "Current in-game day: " + playerData.currentPlayedInGame.Month + "/" + playerData.currentPlayedInGame.Day + "/" + playerData.currentPlayedInGame.Year;
        if (mainClock.currentActIndex != 5)
        {
            currentAct.text = "Current act: " + (mainClock.currentActIndex + 1);
        }
        else
        {
            currentAct.text = "Current act: " + mainClock.currentActIndex;
        }
        lastDayPlayedRealTime.text = "Last Session: " + playerData.lastPlayedRealTime.Month + "/" + playerData.lastPlayedRealTime.Day + "/" + playerData.lastPlayedRealTime.Year;
        lastHourPlayedRealTime.text = "Last Session Time: " + playerData.lastPlayedRealTime.Hour + ":" + playerData.lastPlayedRealTime.Minute;

        dateMessage.text = "It has been " + mainClock.DebuggingTimeDiff(playerData.currentPlayedRealTime, playerData.lastPlayedRealTime) + " days since you last played!";

    }

    //Resets the game to the first day of play.
    public void ResetGameDay()
    {
        mainClock.ResetAtBeginning();
        playerData.currentPlayedRealTime = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);
        UpdateText();
    }

    //Updates the main clock once a time change has been made.
    public void ClockUpdate()
    {
        mainClock.CalculateTimeDifference(playerData.currentPlayedRealTime, playerData.lastPlayedRealTime);
        UpdateText();
    }

    //Advances the game by one day.
    public void AdvanceGameDay()
    {
        mainClock.AdvanceGameDay();
        UpdateText();

        //ALSO: Let's reset the doll's clothing.
        playerData.ClothingReset();
    }

    //Advances the game by one act.
    public void AdvanceCurrentAct()
    {
        mainClock.StartNextAct();
        UpdateText();
    }

    //Set the game to Christmas, therefore unlocking all content.
    public void UnlockContent()
    {
        mainClock.AdvanceToSpecific(new DateTime(1811, 12, 25));
        UpdateText();
    }

    //Updates the passive elements once this is closed.
    public void OnCloseUpdate()
    {
        PassiveElementManager thePassive = FindObjectOfType<PassiveElementManager>();
        runningClock.DateCheck(mainClock.currentGameDay);
        thePassive.UpdatePassives();
    }

    //Reset the player's achievement status so that no achievements are unlocked.
    public void ResetAchievements()
    {
        playerData.ResetAllAchievements();
        dateMessage.text += "\n " + "You have reset your achievements.";
    }

    //Toggle achievements between natural unlock and manually unlocked.
    //Manually unlocked means all achievements are available already without triggers.
    public void ToggleAchievements()
    {
        playerData.AchievementUnlockToggle();
        if (playerData.achievementsManual)
        {
            achievementToggle.text = "MANUAL Achievements";
        }
        else
        {
            achievementToggle.text = "UNLOCKED Achievements";
        }
    }

    //Sets the game to run at nighttime or daytime.  
    public void TogglePassiveTime()
    {
        daytime = !daytime;
        playerData.timeOverride = true;
        FindObjectOfType<PassiveElementManager>().UpdatePassivesDebug();
        if (daytime)
        {
            timeToggle.text = "Currently: DAYTIME";
            FindObjectOfType<CatAudio>().debugAllowed = true;
        }
        else
        {
            timeToggle.text = "Currently: NIGHTTIME";
            FindObjectOfType<CatAudio>().debugAllowed = false;
        }
    }

    //Give control of the passives back to the game.
    public void ResumeNormalTime()
    {
        TimeCheck();
        playerData.timeOverride = false;
        FindObjectOfType<PassiveElementManager>().UpdatePassives();
    }

    public void TimeCheck()
    {
        //We need to set our daytime check to whatever time it actually is.
        if (playerData.currentPlayedRealTime.Hour >= 6 && playerData.currentPlayedRealTime.Hour < 18)
        {
            //It's daytime.
            daytime = true;
            timeToggle.text = "Current: DAYTIME";
        }
        else
        {
            daytime = false;
            timeToggle.text = "Current: NIGHTTIME";
        }
        timeControl.isOn = daytime;

    }

    //Turns the dev console on and off.
    public void ToggleConsole()
    {
        Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
    }

    //REAL TIME DATA MANIPULATION.  No longer controlled in the debug menu as of right now.

    //Add time to the current real time value.
    public void AddHours()
    {
        playerData.currentPlayedRealTime = playerData.currentPlayedRealTime.AddHours(1.0);
        UpdateText();
    }

    //Remove time from the current real time value.
    public void SubtractHours()
    {
        playerData.currentPlayedRealTime = playerData.currentPlayedRealTime.AddHours(-1.0);
        UpdateText();
    }

    public void AddYear()
    {
        playerData.currentPlayedRealTime = playerData.currentPlayedRealTime.AddYears(1);
        UpdateText();
    }

    public void SubtractYear()
    {
        playerData.currentPlayedRealTime = playerData.currentPlayedRealTime.AddYears(-1);
        UpdateText();
    }

    public void AddMonth()
    {
        playerData.currentPlayedRealTime = playerData.currentPlayedRealTime.AddMonths(1);
        UpdateText();
    }

    public void SubtractMonth()
    {
        playerData.currentPlayedRealTime = playerData.currentPlayedRealTime.AddMonths(-1);
        UpdateText();
    }

    public void AddDay()
    {
        playerData.currentPlayedRealTime = playerData.currentPlayedRealTime.AddDays(1.0);
        UpdateText();
    }

    public void SubtractDay()
    {
        playerData.currentPlayedRealTime = playerData.currentPlayedRealTime.AddDays(-1.0);
        UpdateText();
    }

    //REAL TIME LAST PLAYED MANIPULATION.  Not currently used in the new debug menu.
    public void AddHoursPrevSave()
    {
        playerData.lastPlayedRealTime = playerData.lastPlayedRealTime.AddHours(1.0);
        UpdateText();
    }

    public void SubtractHoursPrevSave()
    {
        playerData.lastPlayedRealTime = playerData.lastPlayedRealTime.AddHours(-1.0);
        UpdateText();
    }

    public void AddYearPrevSave()
    {
        playerData.lastPlayedRealTime = playerData.lastPlayedRealTime.AddYears(1);
        UpdateText();
    }

    public void SubtractYearPrevSave()
    {
        playerData.lastPlayedRealTime = playerData.lastPlayedRealTime.AddYears(-1);
        UpdateText();
    }

    public void AddMonthPrevSave()
    {
        playerData.lastPlayedRealTime = playerData.lastPlayedRealTime.AddMonths(1);
        UpdateText();
    }

    public void SubtractMonthPrevSave()
    {
        playerData.lastPlayedRealTime = playerData.lastPlayedRealTime.AddMonths(-1);
        UpdateText();
    }

    public void AddDayPrevSave()
    {
        playerData.lastPlayedRealTime = playerData.lastPlayedRealTime.AddDays(1.0);
        UpdateText();
    }

    public void SubtractDayPrevSave()
    {
        playerData.lastPlayedRealTime = playerData.lastPlayedRealTime.AddDays(-1.0);
        UpdateText();
    }
}
