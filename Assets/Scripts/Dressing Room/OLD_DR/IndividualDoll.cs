using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndividualDoll {

    

    public Dictionary<string, Image> dollClothing;
    public Dictionary<string, Sprite> dollAttire;
    public Dictionary<string, ClothingSlot> dollSlots;


    public string pose;
    public string name;

    public IndividualDoll()
    {

    }

    public IndividualDoll(Dictionary<string, Image> dollClothing, Dictionary<string, Sprite> dollAttire, Dictionary<string, ClothingSlot> dollSlots)
    {
        this.dollClothing = dollClothing;
        this.dollAttire = dollAttire;
        this.dollSlots = dollSlots;
    }
}
