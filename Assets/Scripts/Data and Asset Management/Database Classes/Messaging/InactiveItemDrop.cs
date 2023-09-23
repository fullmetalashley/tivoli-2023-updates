using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InactiveItemDrop 
{
    //Core elements
    public DateTime availableDate;
    public string signifier;
    public string itemName;
    public string location;
    public string category;
    public bool hasItem;
    public Sprite icon;

    //Flavor elements
    public string header;
    public string subheader;
    public string sender;
    public string ctaText;

    public InactiveItemDrop(DateTime date, string signifier, string name, string room, string category, string header, string subheader, string sender, string cta)
    {
        availableDate = date;
        this.signifier = signifier;
        this.itemName = name;
        this.location = room;
        this.category = category;

        this.header = header;
        this.subheader = subheader;
        this.sender = sender;
        this.ctaText = cta;

        //We have an item attached to this drop.
        if (signifier != "")
        {
            hasItem = true;
        }
    }

    public InactiveItemDrop(DateTime date, string signifier, string name, string room, string category, string header, string subheader, string sender, string cta, Sprite icon)
    {
        availableDate = date;
        this.signifier = signifier;
        this.itemName = name;
        this.location = room;
        this.category = category;
        this.icon = icon;

        this.header = header;
        this.subheader = subheader;
        this.sender = sender;
        this.ctaText = cta;

        //We have an item attached to this drop.
        if (signifier != "")
        {
            hasItem = true;
        }
    }
}
