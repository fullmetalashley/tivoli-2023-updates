using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStorage
{
    public int hours;
    public int minutes;
    public int seconds;

    public int month;
    public int day;
    public int year;

    public string timeWritten;


    public TimeStorage()
    {

    }

    public TimeStorage(int hours, int minutes, int seconds, int month, int day, int year)
    {
        this.hours = hours;
        this.minutes = minutes;
        this.seconds = seconds;
        this.month = month;
        this.day = day;
        this.year = year;

        timeWritten = this.month + "/" + this.day + "/" + this.year;

    }

    public TimeStorage(int month, int day, int year)
    {
        this.month = month;
        this.day = day;
        this.year = year;

        timeWritten = this.month + "/" + this.day + "/" + this.year;
    }
}

