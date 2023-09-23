using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ActiveNotification 
{
    public string header;
    public string description;
    public DateTime activeDate;

    public bool hasImage;
    public bool hasLink;

    public string imageRef;
    public string linkRef;

    public Sprite image;

    public ActiveNotification(string header, string description)
    {
        this.header = header;
        this.description = description;
    }

    public ActiveNotification(string header, string description, string imageRef, string linkRef)
    {
        this.header = header;
        this.description = description;

        if (imageRef != "")
        {
            hasImage = true;
            this.imageRef = imageRef;
        }

        if (linkRef != "")
        {
            hasLink = true;
            this.linkRef = linkRef;
        }
    }
}
