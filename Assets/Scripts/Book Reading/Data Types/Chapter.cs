using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class that contains the information pertaining to a specific chapter in the novel.
[System.Serializable]
public class Chapter 
{
    //Data values contained in the chapter.
    public int chapterNumber;
    public string synopsis;
    public string pullQuote;
    public string bodyText;
    public float readTime;
    public int section; //If the chapter is broken up into multiple parts, each part will have a unique section number.


    //Base constructor for making a new chapter.
    //Index 0: Chapter (int)
    //Index 1: Section (int)
    //Index 2: Read time (float)
    //Index 3: Body (string)
    //Index 4: Pull quote (string)
    //Index 5: Summary (string)

    public Chapter(int chap, int section, float readTime, string body, string pull, string summary)
    {
        chapterNumber = chap;
        synopsis = summary;
        pullQuote = pull;
        bodyText = body;
        this.readTime = readTime;
        this.section = section;
    }
}
