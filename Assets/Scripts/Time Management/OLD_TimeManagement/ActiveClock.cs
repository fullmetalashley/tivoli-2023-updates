using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActiveClock : MonoBehaviour
{
    public static ActiveClock control;
    public TimeStorage lastRecordedTime;

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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TimePassed(TimeStorage timeOnEntry)
    {
        //Take the last time we entered the room, and compare the new time.
        //If at least one day has passed, we need to run a check and compare what all is available now.

        DateTime time1 = new DateTime(lastRecordedTime.year, lastRecordedTime.month, lastRecordedTime.day);
        DateTime time2 = new DateTime(timeOnEntry.year, timeOnEntry.month, timeOnEntry.day);

        int daysPassed = DateTime.Compare(time1, time2);

        if (daysPassed > 0)
        {
            Debug.Log("At least one day has passed");
        }
        else
        {
            Debug.Log("It's still the same day");
        }

    }
}
