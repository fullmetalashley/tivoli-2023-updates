using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InactiveLetter 
{
    public string sender;
    public DateTime dateActive;
    public string sentDate;
    public string content;
    public bool hasItem;
    public string[] deliveryNames;
    public string header;

    public string letterType;

    public Sprite[] deliveryIcons;

    public InactiveLetter(DateTime dateActive, string sender, string sentDate, string content)
    {
        this.sender = sender;
        this.dateActive = dateActive;
        this.sentDate = sentDate;
        this.content = content;
        this.hasItem = false;
    }

    public InactiveLetter(DateTime dateActive, string sender, string sentDate, string content, string header, string type)
    {
        this.sender = sender;
        this.dateActive = dateActive;
        this.sentDate = sentDate;
        this.content = content;
        this.hasItem = false;
        this.header = header;
        this.letterType = type;
    }

    public InactiveLetter(DateTime dateActive, string sender, string sentDate, string content, string[] deliveryNames)
    {
        this.sender = sender;
        this.dateActive = dateActive;
        this.sentDate = sentDate;
        this.content = content;
        this.deliveryNames = deliveryNames;
        this.hasItem = true;

        deliveryIcons = new Sprite[deliveryNames.Length];
    }
}
