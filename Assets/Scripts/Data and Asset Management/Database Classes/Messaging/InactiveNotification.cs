using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InactiveNotification 
{
    
        public string header;
        public string description;
        public DateTime activeDate;

        public bool hasImage;
        public bool hasLink;

        public string imageRef;
        public string linkRef;

        public Sprite image;

        public InactiveNotification(string header, string description, DateTime active)
        {
            this.header = header;
            this.description = description;
        activeDate = active;
        }

    public InactiveNotification(string header, string description, string imageRef, string linkRef, DateTime active)
    {
        this.header = header;
        this.description = description;
        activeDate = active;

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
