using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayItemList 
{
    public Dictionary<string, bool> activeStatus;
    public Dictionary<string, Sprite> displayIcons;
    public Dictionary<string, string> categoryValues;
    public List<string> signifier;

    public DisplayItemList(List<string> names, Sprite[] sprites)
    {
        //Initialize these containers.
        this.activeStatus = new Dictionary<string, bool>();
        this.displayIcons = new Dictionary<string, Sprite>();
        this.signifier = new List<string>();
        this.categoryValues = new Dictionary<string, string>();


        //Start setting up the dictionaries.
        for (int i = 0; i < names.Count; i++)
        {

            //We need to take the names and get rid of everything except the last chunk.
            string[] splitString = names[i].Split("$"[0]);

            splitString[0] = splitString[splitString.Length - 1].Trim();
            signifier.Add(splitString[0]);

            //Temporarily taking this out until I decide if it's still useful.
 //           categoryValues.Add(splitString[0], splitString[splitString.Length - 2]);

            
        }
        for (int m = 0; m < signifier.Count; m++)
        {
            displayIcons.Add(signifier[m], sprites[m]);
            activeStatus.Add(signifier[m], false);


        }

    }

    public DisplayItemList(Dictionary<string, bool> actives, Dictionary<string, Sprite> icons, List<string> sigs)
    {
        //Initialize these containers.
        this.activeStatus = actives;
        this.displayIcons = icons;
        this.signifier = sigs;

        
    }
}
