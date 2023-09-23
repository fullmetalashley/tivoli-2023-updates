using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InactiveTableauList 
{
    public Sprite tableau;
    public DateTime dateActive;
    public string signifier;

    public InactiveTableauList(DateTime dateActive, Sprite tableau, string signifier)
    {
        this.dateActive = dateActive;
        this.tableau = tableau;
        this.signifier = signifier;
    }


    public InactiveTableauList(Sprite tableau)
    {
        this.tableau = tableau;
    }
}
