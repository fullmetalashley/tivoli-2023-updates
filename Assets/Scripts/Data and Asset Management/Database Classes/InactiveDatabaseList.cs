using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InactiveDatabaseList 
{
    public DateTime dateActive;
    public string itemClass;
    public string pose;
    public List<Sprite> assetSprites;
    public List<DateTime> activeDates;
    public string stringDate;


    public InactiveDatabaseList()
    {

    }

    public InactiveDatabaseList(DateTime dateActive, string itemClass, string pose)
    {
        this.dateActive = dateActive;
        this.itemClass = itemClass;
        this.pose = pose;
        this.assetSprites = new List<Sprite>();
        this.activeDates = new List<DateTime>();
        this.stringDate = this.dateActive.ToString();
    }
}
