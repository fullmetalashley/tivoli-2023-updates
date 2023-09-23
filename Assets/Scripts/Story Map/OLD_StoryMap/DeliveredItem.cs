using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveredItem
{

    public DateTime dateDelivered;
    public Sprite icon;
    public string itemName;

    public DeliveredItem(DateTime dateDelivered, Sprite icon)
    {
        this.dateDelivered = dateDelivered;
        this.icon = icon;
    }

    public DeliveredItem(DateTime dateDelivered, Sprite icon, string itemName)
    {
        this.dateDelivered = dateDelivered;
        this.icon = icon;
        this.itemName = itemName;
    }
}
