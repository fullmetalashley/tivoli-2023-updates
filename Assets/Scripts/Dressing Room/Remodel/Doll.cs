using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Contains a doll, which stores a name and garment assets for load in respective rooms.
[System.Serializable]
public class Doll
{ 
    public string name;

    public Dictionary<string, Sprite> clothing;
    public Dictionary<string, Sprite> jewelry;

    //The constructor takes in a string for the doll's name.  The rest will track the doll's data throughout the player's session.
    public Doll (string name)
    {
        this.name = name;

        clothing = new Dictionary<string, Sprite>();
        jewelry = new Dictionary<string, Sprite>();
    }
}
