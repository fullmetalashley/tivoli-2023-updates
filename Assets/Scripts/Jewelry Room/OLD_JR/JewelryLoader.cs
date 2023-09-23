using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JewelryLoader : MonoBehaviour
{
    public List<ClothingPanel> thePanels;

    private JewelryCloset jewelryCloset;
    private JewelryDollManager dollControls;

    private JewelryDatabase jewelryLists;

    public ActiveDatabaseList currentList;

    public Sprite unknownAsset;

    public string currentCategory;

    public JewelrySpreadController spreadItems;
    public ClothingDatabase clothingLists;

    public int hairIndex;
    public int earringIndex;
    public int necklaceIndex;
    

    // Start is called before the first frame update
    void Start()
    {
        jewelryCloset = FindObjectOfType<JewelryCloset>();
        dollControls = FindObjectOfType<JewelryDollManager>();
        jewelryLists = FindObjectOfType<JewelryDatabase>();
        spreadItems = FindObjectOfType<JewelrySpreadController>();
        clothingLists = FindObjectOfType<ClothingDatabase>();

        currentCategory = "Necklaces";

        for (int i = 0; i < jewelryLists.activeJewelry.Count; i++)
        {
            if (jewelryLists.activeJewelry[i].itemClass == "Necklaces")
            {
                if (jewelryLists.activeJewelry[i].pose == "3Q")
                {
                    currentList = jewelryLists.activeJewelry[i];
                }
            }
        }

        //Establish the indexes for easier reference later.
        for (int m = 0; m < jewelryLists.activeJewelry.Count; m++)
        {
            if (jewelryLists.activeJewelry[m].itemClass == "Earrings" && jewelryLists.activeJewelry[m].pose == "3Q")
            {
                earringIndex = m;
            }
            if (jewelryLists.activeJewelry[m].itemClass == "Necklaces" && jewelryLists.activeJewelry[m].pose == "3Q")
            {
                necklaceIndex = m;
            }
            if (jewelryLists.activeJewelry[m].itemClass == "Combs" && jewelryLists.activeJewelry[m].pose == "3Q")
            {
                hairIndex = m;
            }

        }
        



    }


    public void PopulationDisplay()
    {
        //Okay, we have the index of each thing.  So let's set them up one at a time.  Let's start with hair.
        //We might have more items than we do active hair.  So we should not be doing it like this.

        //HAIR:
        //First, populate the amount that we have.
        int hairAssetLength = jewelryLists.activeJewelry[hairIndex].assetSprites.Count;
        for (int i = 0; i < hairAssetLength; i++)
        {
            //We are on the first hair image.  SO let's read this item from the active list and now get the signifier.
            string[] splitString = jewelryLists.activeJewelry[hairIndex].assetSprites[i].name.Split("$"[0]);
            splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
            string currentSig = splitString[splitString.Length - 1];

            //Okay, now we have the sig for that image.  The sprite is in display.
            spreadItems.hairItems[i].GetComponent<Image>().enabled = true;
            spreadItems.hairItems[i].GetComponent<Image>().sprite = clothingLists.displayItems.displayIcons[currentSig];
        }
        //NOW, turn off any additional sprites.
        for (int k = hairAssetLength; k < spreadItems.hairItems.Count; k++)
        {
            spreadItems.hairItems[k].GetComponent<Image>().enabled = false;
        }

        //PLEASE NOTE: You will need to do this turn off / enable anytime you have more inactive items than active.
        //So this will need to be done for necklaces and earrings at some point.

        //Next, necklaces.
        for (int i = 0; i < spreadItems.necklaceItems.Count; i++)
        {
            //We are on the first hair image.  SO let's read this item from the active list and now get the signifier.
            string[] splitString = jewelryLists.activeJewelry[necklaceIndex].assetSprites[i].name.Split("$"[0]);
            splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
            string currentSig = splitString[splitString.Length - 1];

            //Okay, now we have the sig for that image.  The sprite is in display.
            spreadItems.necklaceItems[i].GetComponent<Image>().sprite = clothingLists.displayItems.displayIcons[currentSig];
        }

        //Next, earrings.
        for (int i = 0; i < spreadItems.earringItems.Count; i++)
        {
            //We are on the first hair image.  SO let's read this item from the active list and now get the signifier.
            string[] splitString = jewelryLists.activeJewelry[earringIndex].assetSprites[i].name.Split("$"[0]);
            splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
            string currentSig = splitString[splitString.Length - 1];

            //Okay, now we have the sig for that image.  The sprite is in display.
            spreadItems.earringItems[i].GetComponent<Image>().sprite = clothingLists.displayItems.displayIcons[currentSig];
        }
    }


    //This version is outdated.
    public void Population()
    {
        if (currentList.empty)
        {
            foreach (ClothingPanel thePanel in thePanels)
            {
                thePanel.clothingActive = false;
                thePanel.thePanel.enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < currentList.assetSprites.Count; i++)
            {

                thePanels[i].thePanel.sprite = currentList.assetSprites[i];
                thePanels[i].clothingActive = true;
            }
            for (int j = currentList.assetSprites.Count; j < thePanels.Count; j++)
            {
                thePanels[j].clothingActive = false;
            }
            for (int m = 0; m < thePanels.Count; m++)
            {
                if (!thePanels[m].clothingActive)
                {
                    thePanels[m].thePanel.enabled = false;
                }
                else
                {
                    thePanels[m].thePanel.enabled = true;
                }
            }
        }
    }

    //Since all jewelry is available at the start, is this necessary?
    public void SwitchLists(string newCategory)
    {
        currentCategory = newCategory;
        bool listActive = false;
        //Loop through the dictionary.
        for (int i = 0; i < jewelryLists.activeJewelry.Count; i++)
        {
            if (jewelryLists.activeJewelry[i].itemClass == newCategory)
            {
                if (jewelryLists.activeJewelry[i].pose == dollControls.currentDoll.pose)
                {
                    currentList = jewelryLists.activeJewelry[i];
                    listActive = true;
                    currentList.empty = false;
                }
            }
        }
        if (!listActive)
        {
            currentList.empty = true;
        }

        jewelryCloset.NewSlotActive();

    }

    public void CheckForClothes()
    {

    }

}
