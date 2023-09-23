using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ClockTracker : MonoBehaviour
{
    //A static variable to track all instances of the clock and prevent duplicates across scenes.
    public static ClockTracker control;

    //A series of lists of when the individual acts start.
    public List<DateTime> act1Times;
    public List<DateTime> act2Times;
    public List<DateTime> act3Times;
    public List<DateTime> act4Times;
    public List<DateTime> act5Times;

    //A list storing all of the individual act time date lists.
    public List<List<DateTime>> allActs;

    //The index for what act we are in.
    public int currentActIndex;
    //The index for the current game day.
    public int currentGameDayIndex;

    //The specific date list we are pulling from right now.
    public List<DateTime> currentDateList;

    //A list of the dates that are in the backlog, in order to prevent the player from pulling further.
    public List<DateTime> dayBacklog;

    //This is the current day in game time.
    public DateTime currentGameDay;

    //This bool tracks if we've run out of playable game time.
    public bool outOfTime;


    //Tracks if other instances of this script already exist and deletes as necessary.
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

    //Called at the load point on the start screen.  Will establish the load times.
    public void InitializeValues()
    {
        currentDateList = new List<DateTime>();

        allActs = new List<List<DateTime>>
        {
            act1Times,
            act2Times,
            act3Times,
            act4Times,
            act5Times
        };

        DataManager playerData = FindObjectOfType<DataManager>();

        if (playerData.dataExists)
        {
            currentGameDay = playerData.currentPlayedInGame;

            DateTime bugDate = new DateTime(1, 1, 1);
            if (currentGameDay == bugDate)
            {
                //This happens if the game was closed after encountering a bug.  The bug will cause Unity to safe the date time as 1/1/1, so if that is
                //recognized upon load, it will be changed to the default of 9/29/1811.
                Debug.Log("BUG BUG BUG DATE");
                //This date should be 9/29/1811 for the actual build.
                currentGameDay = new DateTime(1811, 09, 29);
                ResetAtBeginning();
            }
            //We need to get the current index and everything from the current game day.
            for (int i = 0; i < allActs.Count; i++)
            {
                //This act has the game day in it.
                if (allActs[i].Contains(currentGameDay))
                {
                    currentActIndex = i;
                    currentDateList = allActs[i];
                }
            }
            for (int j = 0; j < currentDateList.Count; j++)
            {
                if (currentDateList[j] == currentGameDay)
                {
                    currentGameDayIndex = j;
                }
            }
        }
        else
        {
            ResetAtBeginning();
        }
        CalculateTimeDifference(playerData.currentPlayedRealTime, playerData.lastPlayedRealTime);
    }

    //Sets the game back to default 0 values.
    public void ResetAtBeginning()
    {
        currentGameDay = new DateTime();

        currentGameDayIndex = 0;
        currentDateList = act1Times;
        currentActIndex = 0;

        currentGameDay = currentDateList[currentGameDayIndex];

        DataManager playerData = FindObjectOfType<DataManager>();
        playerData.currentPlayedInGame = currentGameDay;
        playerData.ResetRealTime();
    }

    //Sends an update ping to every database so their lists can be refreshed based on the new date.
    public void DateUpdate()
    {
        MailDatabase theMail = FindObjectOfType<MailDatabase>();
        MailController mailUI = FindObjectOfType<MailController>();
        ClothingDatabase theClothing = FindObjectOfType<ClothingDatabase>();
        TableauDatabase theTableaus = FindObjectOfType<TableauDatabase>();
        JewelryDatabase theJewelry = FindObjectOfType<JewelryDatabase>();

        theMail.UpdateLists(currentGameDay);

        theClothing.UpdateLists(currentGameDay);
        theTableaus.UpdateLists(currentGameDay);
        theJewelry.UpdateLists(currentGameDay);
    }

    //Creates a backlog based on how long its been between play sessions.
    public void AddToBacklog(int timeGap)
    {
        //If the time gap is at 1 or less than 1, we can change the game day to that time gap specifically.
        //NOT ANYMORE!  Now we only ever change it by 1.  We should only do this if the measurement is over 1; otherwise, we're going to
        //move days if you log in on the same day.
        if (timeGap > 1 || timeGap == 1)
        {
            currentGameDayIndex += 1;
        }

        //Now that we have our new index, we can change the lists as necessary.
        if (currentGameDayIndex < currentDateList.Count)
        {
            //We can change the game day to another value in the current list; it doesn't exceed this act.
            currentGameDay = currentDateList[currentGameDayIndex];
            DateUpdate();
            SetCurrentInGameDate();
        }
        else
        {
            //In case the list needs to roll over because the previous act ended.
            int overflow = currentDateList.Count - currentGameDayIndex;
            AccomodateOverflow(overflow);
        }
    }

    //Changes the current in game date in Player Data to whatever it is in here.
    public void SetCurrentInGameDate()
    {
        DataManager playerData = FindObjectOfType<DataManager>();
        playerData.currentPlayedInGame = new DateTime(currentGameDay.Year, currentGameDay.Month, currentGameDay.Day);
    }

    //This moves the game to the last in-game date the player left off at.
    public void AdvanceToSpecific(DateTime newDate)
    {
        currentGameDay = newDate;
        //Find the act that this game day is in.
        for (int i = 0; i < allActs.Count; i++)
        {
            if (allActs[i].Contains(newDate))
            {
                //We have found the act that contains this date.
                //Set the date list and the current act to this value.
                currentDateList = allActs[i];
                currentActIndex = i;
            }
        }
        //Now we can set the date.
        for (int j = 0; j < currentDateList.Count; j++)
        {
            if (currentDateList[j] == newDate)
            {
                //The index is set.
                currentGameDayIndex = j;
            }
        }

        SetCurrentInGameDate();
    }

    //Allows for quick debugging.
    public int DebuggingTimeDiff(DateTime currentTime, DateTime lastPlayed)
    {
        float unroundedDif = (float)(currentTime - lastPlayed).TotalDays;
        //Get the backlog calculated to make sure we don't go over our time.
        int timeGap = Mathf.RoundToInt((float)((currentTime - lastPlayed).TotalDays));

        return timeGap;
    }

    //Get the last time the player logged in in real-time, and determine how long it's been since they last played.
    public void CalculateTimeDifference(DateTime currentTime, DateTime lastPlayed)
    {
        float unroundedDif = (float)(currentTime - lastPlayed).TotalDays;
        //Get the backlog calculated to make sure we don't go over our time.
        int timeGap = Mathf.RoundToInt((float)((currentTime - lastPlayed).TotalDays));
        //Using the unroundedDif allows us to avoid strange instances with fractionary numbers.
        AddToBacklog((int)unroundedDif);
    }

    //Acts will be changed and lists adjust accordingly.  This happens on natural change and does not affect the game date.
    public void ChangeActList()
    {
        //This is almost unnecessary, but it prevents us from increasing the act index if it's already at max.  This way it can't move to 6, it'll always be
        //at 5.
        if (currentActIndex < allActs.Count)
        {
            currentActIndex++;
        }
        //Changing to another act within the game.
        if (currentActIndex < allActs.Count)
        {
            currentDateList = allActs[currentActIndex];
        }
        else
        {
            //There are no more acts, and no more days.  The player has run out of time.
            outOfTime = true;
        }
    }

    //The next act is started and the in-game date is changed to reflect the first day of that act.  Different than the above 
    //because the in-game date is started at the start of this act, regardless of what day it currently is.
    public void StartNextAct()
    {
        //If the player hasn't run out of acts and days...
        if (!outOfTime)
        {
                currentActIndex++;
            //Changing to another act within the game.
            if (currentActIndex < allActs.Count)
            {
                currentDateList = allActs[currentActIndex];
                currentGameDay = currentDateList[0];
                currentGameDayIndex = 0;
                DateUpdate();
                SetCurrentInGameDate();
            }
            else
            {
                currentActIndex--;
                //There are no more acts, and no more days.  The player has run out of time.
            }
        }
    }

    /*This is used in case the player has run through the remainder of one act list, and needs to move into the next one
     * by a specific amount of days.  It is called by AddToBacklog.*/
    public void AccomodateOverflow(int overflow)
    {
        int residual = overflow;
        /*We move through the current date list as much as possible and subtract from the residual value for
        each day.  This gives us a residual value for how many days are left once we've ended the current
        date list.*/
        for (int i = overflow; i < currentDateList.Count; i++)
        {
            residual--;
        }
        ChangeActList();
        //If the residual amount is smaller than the current act, we can move to a date in this new act.
        if (residual < currentDateList.Count)
        {
            currentGameDay = currentDateList[residual];
            currentGameDayIndex = residual;
            DateUpdate();
            SetCurrentInGameDate();
        }
        else
        {
            //If the residual is very large, we need to run this method again until the residual occurs within an act range.
            //NOTE: If we have an insane overflow, there won't be anymore time to change to.  This should not happen.
            if (!outOfTime)
            {
                int newOverflow = currentDateList.Count - residual;
                AccomodateOverflow(newOverflow);
            }
            else
            {
                Debug.Log("We cannot accomodate overflow because the player is out of time.");
            }
        }
    }

    //The game day is moved to the next day.
    //This only advances the game day by one.
    public void AdvanceGameDay()
    {
        DataManager playerData = FindObjectOfType<DataManager>();
        if (!outOfTime)
        {
            //We still have time, so we advance the game day index.
            currentGameDayIndex++;
            if (currentGameDayIndex < currentDateList.Count)
            {
                //We are within the parameters of our current list, so we can move to this index within the list.
                currentGameDay = currentDateList[currentGameDayIndex];
            }
            else
            {
                //We need to move to the next act.  The GameDayIndex is set to -1 to keep it starting at 0 for the next act.
                ChangeActList();
                currentGameDayIndex = -1;
            }
            DateUpdate();
            playerData.currentPlayedInGame = currentGameDay;
        }
    }
}
