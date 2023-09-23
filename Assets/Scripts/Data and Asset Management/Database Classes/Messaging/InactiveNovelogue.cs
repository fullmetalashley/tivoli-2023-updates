using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InactiveNovelogue 
{
    public string header;
    public string chapterDescription;
    public string readTime;
    public string publishDate;
    public int chapterNumber;
    public int chapterIndex;
    public int section;
    public DateTime availableDate;


    public InactiveNovelogue(string header, string chapterDescription, string readTime, string publishDate, int chapterNumber, 
        int chapterIndex, int section, DateTime date)
    {
        this.header = header;
        this.chapterDescription = chapterDescription;
        this.readTime = readTime;
        this.publishDate = publishDate;
        this.chapterNumber = chapterNumber;
        this.chapterIndex = chapterIndex;
        availableDate = date;
        this.section = section;
    }
}
