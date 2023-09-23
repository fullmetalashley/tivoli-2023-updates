using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ActiveNovelogue 
{
    public string header;
    public string chapterDescription;
    public string readTime;
    public string publishDate;
    public int chapterNumber;
    public int chapterIndex;
    public int section;

    public ActiveNovelogue(string header, string chapterDescription, string readTime, string publishDate, int chapterNumber, int chapterIndex, int section)
    {
        this.header = header;
        this.chapterDescription = chapterDescription;
        this.readTime = readTime;
        this.publishDate = publishDate;
        this.chapterNumber = chapterNumber;
        this.chapterIndex = chapterIndex;
        this.section = section;
    }

    public ActiveNovelogue(InactiveNovelogue inactive)
    {
        this.header = inactive.header;
        this.chapterDescription = inactive.chapterDescription;
        this.readTime = inactive.readTime;
        this.publishDate = inactive.publishDate;
        this.chapterNumber = inactive.chapterNumber;
        this.chapterIndex = inactive.chapterIndex;
        this.section = inactive.section;
    }
}
