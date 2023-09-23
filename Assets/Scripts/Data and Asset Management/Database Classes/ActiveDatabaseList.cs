using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveDatabaseList 
{
    public TimeStorage dateActive;
    public string itemClass;
    public string pose;
    public List<Sprite> assetSprites;
    public bool empty;

    public ActiveDatabaseList(string itemClass, string pose)
    {
        this.itemClass = itemClass;
        this.pose = pose;
        this.assetSprites = new List<Sprite>();
    }

    public ActiveDatabaseList(string itemClass, string pose, List<Sprite> newAssets)
    {
        this.itemClass = itemClass;
        this.pose = pose;
        this.assetSprites = new List<Sprite>();
        this.assetSprites = newAssets;
    }
}
