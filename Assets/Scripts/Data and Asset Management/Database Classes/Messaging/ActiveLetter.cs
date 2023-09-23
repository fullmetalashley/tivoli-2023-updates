using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ActiveLetter 
{
    public string sender;
    public string sentDate;
    public string content;
    public bool letterRead;
    public bool hasItem;
    public string header;

    public string letterType;

    public string[] itemNames;
    public DateTime activeDate;

    public Sprite[] deliveredImages;

    public ActiveLetter(string sender, string sentDate, string content)
    {
        this.sender = sender;
        this.sentDate = sentDate;
        this.content = content;
        hasItem = false;
    }

    //When the letter has an updated header.
    public ActiveLetter(string sender, string sentDate, string content, string header, string type)
    {
        this.sender = sender;
        this.sentDate = sentDate;
        this.content = content;
        hasItem = false;
        this.header = header;
        this.letterType = type;
    }

    //This constructor is more of a convert from the inactive letter type.
    public ActiveLetter(string sender, string sentDate, string content, bool hasItem, string[] itemInfo, DateTime activeDate, Sprite[] deliveredImages, string type)
    {
        this.sender = sender;
        this.sentDate = sentDate;
        this.content = content;

        this.hasItem = hasItem;
        this.itemNames = itemInfo;
        this.activeDate = activeDate;
        this.deliveredImages = deliveredImages;
        this.letterType = type;
    }
}
