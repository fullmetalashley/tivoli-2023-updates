using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Controls an advent icon.
[System.Serializable]
public class Advent
{
    public Sprite icon;
    public string name;
    public string description;

    public Advent(Sprite icon, string name, string desc)
    {
        this.icon = icon;
        this.name = name;
        this.description = desc;
    }
}
