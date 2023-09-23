using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClothingDatabase : MonoBehaviour
{
    public static ClothingDatabase control;

    public List<ActiveDatabaseList> activeClothing;
    public List<InactiveDatabaseList> inactiveClothing;

    public DisplayItemList displayItems;

    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateDisplayStatus()
    {
        //What if we loop through each active item, then use that to toggle it on?
        //In theory, this is only checking every other item, so not checking each pose.
        for (int i = 0; i < activeClothing.Count; i+=2)
        {
            //Now we break out the signifier for that thing.
            for (int j = 0; j < activeClothing[i].assetSprites.Count; j++)
            {
                string[] splitString = activeClothing[i].assetSprites[j].name.Split("$"[0]);
                if (splitString.Length > 1)
                {
                    splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                    string currSig = splitString[splitString.Length - 1];

                    for (int m = 0; m < displayItems.signifier.Count; m++)
                    {
                        if (displayItems.signifier[m] == currSig)
                        {
                            displayItems.activeStatus[displayItems.signifier[m]] = true;
                        }
                    }
                }
            }
        }     
    }

    public void UpdateLists(DateTime currentTime)
    {
        List<InactiveDatabaseList> toBeRemoved = new List<InactiveDatabaseList>();
        foreach (InactiveDatabaseList clothingType in inactiveClothing)
        {
            if ((clothingType.dateActive - currentTime).TotalDays < 0 || (clothingType.dateActive - currentTime).TotalDays == 0)
            {
                ActiveDatabaseList tempList = new ActiveDatabaseList(clothingType.itemClass, clothingType.pose, clothingType.assetSprites);
                {
                    bool listFound = false;
                    for (int i = 0; i < activeClothing.Count; i++)
                    {
                        if (activeClothing[i].itemClass == tempList.itemClass && activeClothing[i].pose == tempList.pose)
                        {
                            listFound = true;
                            for (int m = 0; m < tempList.assetSprites.Count; m++)
                            {
                                activeClothing[i].assetSprites.Add(tempList.assetSprites[m]);
                            }
                        }
                    }

                    if (!listFound)
                    {
                        activeClothing.Add(new ActiveDatabaseList(clothingType.itemClass, clothingType.pose, clothingType.assetSprites));
                    }
                    toBeRemoved.Add(clothingType);
                }
            }
        }
        foreach(InactiveDatabaseList inactiveClothes in toBeRemoved)
        {
            inactiveClothing.Remove(inactiveClothes);
        }
        UpdateDisplayStatus();
    }

    //A thorough debug method for assessing issues.
    public void DebugClothingAssets()
    {
        for (int z = 0; z < inactiveClothing.Count; z++)
        {
            Debug.Log("INACTIVE ITEM: " + inactiveClothing[z].itemClass + " Pose: " + inactiveClothing[z].pose + "Drop Date: " + inactiveClothing[z].stringDate);
            foreach (Sprite sprrr in inactiveClothing[z].assetSprites)
            {
                Debug.Log("\nSprite name: " + sprrr.name);
            }
        }
        for (int z = 0; z < activeClothing.Count; z++)
        {
            Debug.Log("ACTIVE ITEM: " + activeClothing[z].itemClass + " Pose: " + activeClothing[z].pose);
            foreach (Sprite sprrr in activeClothing[z].assetSprites)
            {
                Debug.Log("\nSprite name: " + sprrr.name);
            }
        }
    }

    //A quick debug method for fast issue checks.
    public void QuickDebug()
    {
        Debug.Log("STARTING A NEW PRINT!\n");
        Debug.Log("Starting active assets with..." + activeClothing.Count);
        for (int i = 0; i < inactiveClothing.Count; i++)
        {
            Debug.Log("\nINACTIVE ITEM: " + inactiveClothing[i].itemClass + " Pose: " + inactiveClothing[i].pose + " Drop Date: " + inactiveClothing[i].stringDate +
                " Total Sprites: " + inactiveClothing[i].assetSprites.Count);
        }
    }
}
