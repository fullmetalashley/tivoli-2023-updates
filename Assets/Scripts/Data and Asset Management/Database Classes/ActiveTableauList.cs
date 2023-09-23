using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveTableauList
{
    public Sprite tableau;
    public string signifier;

    public ActiveTableauList(Sprite tableau, string signifier)
    {
        this.tableau = tableau;
        this.signifier = signifier;
    }
}
