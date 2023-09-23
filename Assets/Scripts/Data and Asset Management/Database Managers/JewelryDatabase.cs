using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JewelryDatabase : MonoBehaviour
{
    public static JewelryDatabase control;

    public List<ActiveDatabaseList> activeJewelry;
    public List<InactiveDatabaseList> inactiveJewelry;

    //Initialize this instance of the Jewelry Database so it can persist across scenes.
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
        ClothingDatabase clothes = FindObjectOfType<ClothingDatabase>();
        //What if we loop through each active item, then use that to toggle it on?
        //In theory, this is only checking every other item, so not checking each pose.
        for (int i = 0; i < activeJewelry.Count; i += 2)
        {
            //Now we break out the signifier for that thing.
            for (int j = 0; j < activeJewelry[i].assetSprites.Count; j++)
            {
                string[] splitString = activeJewelry[i].assetSprites[j].name.Split("$"[0]);
                if (splitString.Length > 1)
                {
                    splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                    string currSig = splitString[splitString.Length - 1];

                    for (int m = 0; m < clothes.displayItems.signifier.Count; m++)
                    {
                        if (clothes.displayItems.signifier[m] == currSig)
                        {
                            clothes.displayItems.activeStatus[clothes.displayItems.signifier[m]] = true;
                        }
                    }
                }
            }
        }
    }

    public void UpdateLists(DateTime currentTime)
    {
        List<InactiveDatabaseList> toBeRemoved = new List<InactiveDatabaseList>();
        foreach (InactiveDatabaseList thisJewel in inactiveJewelry)
        {
            if ((thisJewel.dateActive - currentTime).TotalDays < 0 || (thisJewel.dateActive - currentTime).TotalDays == 0)
            {
                ActiveDatabaseList tempList = new ActiveDatabaseList(thisJewel.itemClass, thisJewel.pose, thisJewel.assetSprites);
                {
                    bool listFound = false;
                    for (int i = 0; i < activeJewelry.Count; i++)
                    {
                        if (activeJewelry[i].itemClass == tempList.itemClass && activeJewelry[i].pose == tempList.pose)
                        {
                            listFound = true;
                            for (int m = 0; m < tempList.assetSprites.Count; m++)
                            {
                                activeJewelry[i].assetSprites.Add(tempList.assetSprites[m]);
                            }
                        }
                    }

                    if (!listFound)
                    {
                        activeJewelry.Add(new ActiveDatabaseList(thisJewel.itemClass, thisJewel.pose, thisJewel.assetSprites));
                    }
                    toBeRemoved.Add(thisJewel);
                }
            }
        }
        foreach (InactiveDatabaseList inactiveClothes in toBeRemoved)
        {
            inactiveJewelry.Remove(inactiveClothes);
        }
        UpdateDisplayStatus();
    }

    public void DebugJewelry()
    {
        for (int z = 0; z < inactiveJewelry.Count; z++)
        {
            Debug.Log("INACTIVE ITEM: " + inactiveJewelry[z].itemClass + " Pose: " + inactiveJewelry[z].pose + "Drop Date: " + inactiveJewelry[z].stringDate);
            foreach (Sprite sprrr in inactiveJewelry[z].assetSprites)
            {
                Debug.Log("\nSprite name: " + sprrr.name);
            }
        }
        for (int z = 0; z < activeJewelry.Count; z++)
        {
            Debug.Log("ACTIVE ITEM: " + activeJewelry[z].itemClass + " Pose: " + activeJewelry[z].pose);
            foreach (Sprite sprrr in activeJewelry[z].assetSprites)
            {
                Debug.Log("\nSprite name: " + sprrr.name);
            }
        }
    }
}
