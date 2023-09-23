using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class containing the data needed to mark a section header in the table of contents.
public class SectionBlock 
{
    public string header;
    public string summary;
    public string chapters;
    public int readTime;   

    //Base constructor
    public SectionBlock(string _header, string _summary, string _chapters, int _readTime)
    {
        header = _header;
        summary = _summary;
        chapters = _chapters;
        readTime = _readTime;
    }
}
