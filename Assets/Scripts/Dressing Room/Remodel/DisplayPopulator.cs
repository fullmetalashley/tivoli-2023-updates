using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Initialize all images with their corresponding indexes based on the garment type.
//Does this upon loading the dressing room, and does not need to do it again.

public class DisplayPopulator : MonoBehaviour
{
    //All lists of the individual clothing panels.
    public List<ClothingPanel> dresses;
    public List<ClothingPanel> hats;
    public List<ClothingPanel> accessories;
    public List<ClothingPanel> gloves;
    public List<ClothingPanel> handhelds;
    public List<ClothingPanel> outerwear;
    public List<ClothingPanel> shoes;

    //SCRIPT REFS
    private ClothingDatabase clothingDatabase;
    private DollSkin dollControls;
    private ClosetControls theCloset;
    private PaginationDirector pageDirection;

    //List of our current clothing.
    public List<ActiveDatabaseList> currentList;
    
    public List<ClothingPanel> currentPanelList;

    //A sprite reference for the mystery / unlocked item.
    public Sprite unknownAsset;

    // Start is called before the first frame update
    void Start()
    {
        clothingDatabase = FindObjectOfType<ClothingDatabase>();
        dollControls = FindObjectOfType<DollSkin>();
        theCloset = FindObjectOfType<ClosetControls>();
        pageDirection = FindObjectOfType<PaginationDirector>();

        PopulateLists();
    }

    //Run this when the dolls change so that the lists can be accurate to their indexes.
    public void PopulateLists()
    {
        SinglePopulation("Dresses", dresses);

        SinglePopulation("Shoes", shoes);

        SinglePopulation("Overdresses", accessories);

        StaggerPopulation("Hats", hats);

        StaggerPopulation("Gloves", gloves);

        OuterwearPopulation(outerwear);

        HandheldPopulation(handhelds);

        //Once all lists are populated, turn off the pages as necessary.
        pageDirection.SetupControllers();
    }

    //Populate clothing items that load without a staggered / separate sprite.
    //For lists with just one clothing type and single sprites.
    public void SinglePopulation(string category, List<ClothingPanel> panels)
    {
        ActiveDatabaseList actives = ReturnClothingList(category);
        InactiveDatabaseList inactives = ReturnInactiveList(category);

        //Always clear all panels prior to running this to make sure they're empty.
        ClearAllPanels(panels);

        //Is this list empty?
        if (actives.empty)
        {
            EmptyList(panels, inactives);
        }
        else
        {
            for (int i = 0; i < actives.assetSprites.Count; i++)
            {
                string[] splitString = actives.assetSprites[i].name.Split("$"[0]);
                splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                string currentSig = splitString[splitString.Length - 1];

                SetPanel(currentSig, i, panels);
                panels[i].amendedIndex = i;
                panels[i].GetComponent<DragController>().subcategory = actives.itemClass;
            }
        }
        //If we have a few inactives, we need ONE inactive to represent it.
        if (inactives != null && inactives.assetSprites.Count > 0)
        {
            //Just one mystery panel is present.
            MysteryPanel(actives.assetSprites.Count, panels);

            //Turn off the rest of the panels.
            for (int m = (actives.assetSprites.Count + 1); m < panels.Count; m++)
            {
                DisablePanel(m, panels);
            }
        }
        else                       
        {
            //If we have no inactives, we can just set the rest of the panels from specific points.
            //Turn off the rest of the panels.
            for (int m = (actives.assetSprites.Count); m < panels.Count; m++)
            {
                DisablePanel(m, panels);
            }
        }
    }

    //Populate clothing items that load with a staggered / separate sprite.
    //For lists with multiple sprites per display sprite (i.e. gloves, etc).
    public void StaggerPopulation(string category, List<ClothingPanel> panels)
    {
        ActiveDatabaseList actives = ReturnClothingList(category);
        InactiveDatabaseList inactives = ReturnInactiveList(category);

        ClearAllPanels(panels);

        if (actives.empty)
        {
            //First go through inactives.
            int halvedAssets = inactives.assetSprites.Count / 2;
            for (int k = halvedAssets; k < (actives.assetSprites.Count + halvedAssets); k++)
            {
                MysteryPanel(k, panels);
            }
            //Then disable all remaining.
            for (int j = (actives.assetSprites.Count + halvedAssets); j < panels.Count; j++)
            {
                DisablePanel(j, panels);
            }
        }
        else
        {

            //Setting the images.  Get the sig from the sprite, and find the corresponding image in the display items.
            for (int i = 0; i < actives.assetSprites.Count; i += 2)
            {
                string[] splitString = actives.assetSprites[i].name.Split("$"[0]);
                splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                string currentSig = splitString[splitString.Length - 1];

                SetPanel(currentSig, (i / 2), panels);
                panels[i / 2].amendedIndex = i;
                panels[i / 2].GetComponent<DragController>().subcategory = actives.itemClass;
            }

            //Do we have inactives?
            if (inactives != null)
            {
                //How many inactives do we have?  We know that.  So divide it by 2.
                int halvedAssets = inactives.assetSprites.Count / 2;

                //Set ONE panel to be the mystery panel.
                MysteryPanel(actives.assetSprites.Count / 2, panels);

                //Disable the rest of the panels.
                for (int m = ((actives.assetSprites.Count / 2) + 1); m < panels.Count; m++)
                {
                    DisablePanel(m, panels);
                }
            }
            else
            {
                for (int m = (actives.assetSprites.Count); m < panels.Count; m++)
                {
                    DisablePanel(m, panels);
                }
            }
        }
    }

    //A combination category for putting multiple types of clothing in one list.
    //For the outerwear categories.
    public void OuterwearPopulation(List<ClothingPanel> panels)
    {
        int leftOffIndex = 0;
        ActiveDatabaseList activeJackets = ReturnClothingList("Jackets");
        ActiveDatabaseList activeShawls = ReturnClothingList("Shawls");
        ActiveDatabaseList activeCapes = ReturnClothingList("Capes");
        ActiveDatabaseList activeCoats = ReturnClothingList("Coats");

        InactiveDatabaseList inactiveJackets = ReturnInactiveList("Jackets");
        InactiveDatabaseList inactiveShawls = ReturnInactiveList("Shawls");
        InactiveDatabaseList inactiveCapes = ReturnInactiveList("Capes");
        InactiveDatabaseList inactiveCoats = ReturnInactiveList("Coats");

        ClearAllPanels(outerwear);

        if (activeJackets.empty && activeShawls.empty && activeCapes.empty)
        {
            EmptyList(outerwear, inactiveJackets);
        }

        //Let's start with jackets.
        for (int i = 0; i < activeJackets.assetSprites.Count; i++)
        {
            string[] splitString = activeJackets.assetSprites[i].name.Split("$"[0]);
            splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
            string currentSig = splitString[splitString.Length - 1];

            SetPanel(currentSig, i, panels);
            panels[i].amendedIndex = i;
            panels[i].GetComponent<DragController>().subcategory = activeJackets.itemClass;
        }

        leftOffIndex = activeJackets.assetSprites.Count;

        //Now we start shawls, which are staggered.
        //Setting the images.  Get the sig from the sprite, and find the corresponding image in the display items.
        //NOTE: Only staggered if you're elizabeth.  Otherwise, straight.
        if (dollControls.elizabeth.activeSelf)
        {
            for (int j = 0; j < (activeShawls.assetSprites.Count); j += 2)
            {
                string[] splitString = activeShawls.assetSprites[j].name.Split("$"[0]);
                splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                string currentSig = splitString[splitString.Length - 1];

                SetPanel(currentSig, (leftOffIndex + (j / 2)), panels);
                panels[leftOffIndex + (j / 2)].amendedIndex = j;
                panels[leftOffIndex + (j / 2)].GetComponent<DragController>().subcategory = activeShawls.itemClass;
            }

            leftOffIndex += (activeShawls.assetSprites.Count / 2);

        }
        else
        {

            //Jane population is straightforward.
            for (int j = 0; j < (activeShawls.assetSprites.Count); j ++)
            {
                string[] splitString = activeShawls.assetSprites[j].name.Split("$"[0]);
                splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                string currentSig = splitString[splitString.Length - 1];

                SetPanel(currentSig, (leftOffIndex + j), panels);
                panels[leftOffIndex + j].amendedIndex = j;
                panels[leftOffIndex + j].GetComponent<DragController>().subcategory = activeShawls.itemClass;
            }

            leftOffIndex += (activeShawls.assetSprites.Count);
        }

        //Now we finish with capes, which are staggered.
        //Setting the images.  Get the sig from the sprite, and find the corresponding image in the display items.
        for (int k = 0; k < activeCapes.assetSprites.Count; k += 2)
        {
            string[] splitString = activeCapes.assetSprites[k].name.Split("$"[0]);
            splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
            string currentSig = splitString[splitString.Length - 1];

            SetPanel(currentSig, (leftOffIndex + (k / 2)), panels);
            panels[leftOffIndex + (k / 2)].amendedIndex = k;
            panels[leftOffIndex + (k / 2)].GetComponent<DragController>().subcategory = activeCapes.itemClass;
        }

        leftOffIndex += (activeCapes.assetSprites.Count / 2);

        //One more: Coats!  They are not staggered at all.
        for (int q = 0; q < activeCoats.assetSprites.Count; q++)
        {
            string[] splitString = activeCoats.assetSprites[q].name.Split("$"[0]);
            splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
            string currentSig = splitString[splitString.Length - 1];

            SetPanel(currentSig, (leftOffIndex + q), panels);
            panels[leftOffIndex + q].amendedIndex = q;
            panels[leftOffIndex + q].GetComponent<DragController>().subcategory = activeCoats.itemClass;
        }

        leftOffIndex += activeCoats.assetSprites.Count;

        //SO: If ANY of these exist, we need to add one and set that as a mystery panel.
        if (inactiveCapes != null || inactiveCoats != null || inactiveJackets != null || inactiveShawls != null)
        {
            MysteryPanel(leftOffIndex, panels);
            leftOffIndex++;
        }

        //Now we finish off disabling the rest of the panels.
        for (int z = leftOffIndex; z < panels.Count; z++)
        {
            DisablePanel(z, panels);
        }
        
    }

    //Populate the handhelds category, specifically fans and handbags.
    //Neither need to be staggered.
    public void HandheldPopulation(List<ClothingPanel> panels)
    {
        int leftOffIndex = 0;
        ActiveDatabaseList activeFans = ReturnClothingList("Fans");
        ActiveDatabaseList activeBags = ReturnClothingList("Handbags");

        InactiveDatabaseList inactiveFans = ReturnInactiveList("Fans");
        InactiveDatabaseList inactiveBags = ReturnInactiveList("Handbags");

        ClearAllPanels(handhelds);

        int activeFanCount = 0;
        if (activeFans != null)
        {
            activeFanCount = activeFans.assetSprites.Count;
            //Let's start with fans.
            for (int i = 0; i < activeFans.assetSprites.Count; i++)
            {
                string[] splitString = activeFans.assetSprites[i].name.Split("$"[0]);
                splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                string currentSig = splitString[splitString.Length - 1];

                SetPanel(currentSig, i, panels);
                panels[i].amendedIndex = i;
                panels[i].GetComponent<DragController>().subcategory = activeFans.itemClass;
            }
        }

        //Now we populate any inactive fans.
        //If we have a few inactives, we need to set them up.

        leftOffIndex = activeFanCount;

        //Now we start bags, which are not staggered.
        //Setting the images.  Get the sig from the sprite, and find the corresponding image in the display items.
        int activeBagCount = 0;
        if (activeBags != null)
        {
            activeBagCount = activeBags.assetSprites.Count;
            for (int j = 0; j < (activeBags.assetSprites.Count); j++)
            {
                string[] splitString = activeBags.assetSprites[j].name.Split("$"[0]);
                splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                string currentSig = splitString[splitString.Length - 1];

                SetPanel(currentSig, (j + leftOffIndex), panels);
                panels[(j + leftOffIndex)].amendedIndex = j;
                panels[(j + leftOffIndex)].GetComponent<DragController>().subcategory = activeBags.itemClass;
            }
        }


        leftOffIndex += activeBagCount;

        
//Set up ONE mystery sprite if it exists.
        if (inactiveBags != null || inactiveFans != null)
        {
            MysteryPanel(leftOffIndex, panels);
            leftOffIndex++;
        }

        //Now we finish off disabling the rest of the panels.
        for (int z = leftOffIndex; z < panels.Count; z++)
        {
            DisablePanel(z, panels);
        }
    }
 

    //-----------------
    //DATABASE RETURNS
    //Return the clothing list for this category.  Pose IS relevant.
    public ActiveDatabaseList ReturnClothingList(string category)
    {
        for (int i = 0; i < clothingDatabase.activeClothing.Count; i++)
        {
            if (clothingDatabase.activeClothing[i].itemClass == category && clothingDatabase.activeClothing[i].pose == dollControls.pose)
            {
                return clothingDatabase.activeClothing[i];
            }
        }
        return null;
    }

    //Return the inclothing list for this category.  Pose is irrelevant because this is purely for population display.
    public InactiveDatabaseList ReturnInactiveList(string category)
    {
        for (int i = 0; i < clothingDatabase.inactiveClothing.Count; i++)
        {
           if (clothingDatabase.inactiveClothing[i].itemClass == category && clothingDatabase.inactiveClothing[i].pose == dollControls.pose)
            {
                return clothingDatabase.inactiveClothing[i];
            }
        }
        return null;
    }


    //Return the current number of actives based on category.
    public int ReturnActiveCount(string category)
    {
        switch (category) {
            case "Dresses":
                return ReturnClothingList("Dresses").assetSprites.Count;
            case "Hats":
                return ReturnClothingList("Hats").assetSprites.Count / 2;   //Needs to be divided by 2 now that we have fronts and backs for hats.
            case "Overdresses":
                return ReturnClothingList("Overdresses").assetSprites.Count;
            case "Gloves":
                return ReturnClothingList("Gloves").assetSprites.Count / 2; //Needs to be divided by 2 or it'll return every glove sprite.
            case "Handhelds":
                return ReturnClothingList("Handbags").assetSprites.Count +
                    ReturnClothingList("Fans").assetSprites.Count;
            case "Outerwear":

                //This one is specific because we need to divide some of these in certain cases.
                int jackets = ReturnClothingList("Jackets").assetSprites.Count;

                int shawls = ReturnClothingList("Shawls").assetSprites.Count;
                if (dollControls.pose == "3Q")
                {
                    //We can divide the shawl count by 2.
                    shawls /= 2;
                }

                int capes = ReturnClothingList("Capes").assetSprites.Count / 2;

                int coats = ReturnClothingList("Coats").assetSprites.Count;


                return jackets + shawls + capes + coats;
            case "Shoes":
                return ReturnClothingList("Shoes").assetSprites.Count;
            default:
                return 0;
        }
    }


    //-----------------
    //PANEL ACTIONS
    public void SetPanel(string sig, int index, List<ClothingPanel> panelList)
    {
        panelList[index].thePanel.sprite = clothingDatabase.displayItems.displayIcons[sig];
        panelList[index].clothingActive = true;

        //Turn the image on after everything else has happened.
        panelList[index].thePanel.enabled = true;
//        panelList[index].thePanel.raycastTarget = true;
    }

    //This method is called multiple times in order to set all panels to a baseline of being empty, with no clothing, and not being interactable.
    public void ClearAllPanels(List<ClothingPanel> panelList)
    {
        for (int p = 0; p < panelList.Count; p++)
        {
            panelList[p].clothingActive = false;
            if (panelList[p].thePanel != null)
            {
                panelList[p].thePanel.enabled = false;
                panelList[p].thePanel.sprite = null;
            }
        }
    }

    //Sets a specific panel as a mystery panel.
    public void MysteryPanel(int index, List<ClothingPanel> panelList)
    {
        panelList[index].thePanel.sprite = unknownAsset;
        panelList[index].clothingActive = false;
        panelList[index].thePanel.enabled = true;
        panelList[index].thePanel.raycastTarget = false;
        panelList[index].GetComponent<ItemFlyout>().mysteryIcon = true;
    }

    //Turns off a specific panel at the end of a list.
    public void DisablePanel(int index, List<ClothingPanel> panelList)
    {
        panelList[index].clothingActive = false;
        if (panelList[index].thePanel != null)
        {
            panelList[index].thePanel.enabled = false;
            panelList[index].thePanel.sprite = null;
        }
    }

    //Change the current list of panels for easy access from other scripts.
    public void ChangePanelList(string list)
    {
        switch (list)
        {
            case "Dresses":
                currentPanelList = dresses;
                break;
            case "Handhelds":
                currentPanelList = handhelds;
                break;
            case "Outerwear":
                currentPanelList = outerwear;
                break;
            case "Shoes":
                currentPanelList = shoes;
                break;
            case "Overdresses":
                currentPanelList = accessories;
                break;
            case "Hats":
                currentPanelList = hats;
                break;
            case "Gloves":
                currentPanelList = gloves;
                break;
            default:
                break;
        }
    }

    //Set up for an empty list.
    public void EmptyList(List<ClothingPanel> panelList, InactiveDatabaseList inactiveAssets)
    {
        if (inactiveAssets != null)
        {
            //Go through the inactive panels and set them to be the mystery sprite.
            for (int k = 0; k < inactiveAssets.assetSprites.Count; k++)
            {
                MysteryPanel(k, panelList);
            }

            //Go through remaining panels and turn them off.
            //I don't think we really need to do this, but it's an extra safeguard.
            for (int m = inactiveAssets.assetSprites.Count; m < panelList.Count; m++)
            {
                DisablePanel(m, panelList);
            }
        }
        else
        {
            for (int m = 0; m < panelList.Count; m++)
            {
                DisablePanel(m, panelList);
            }
        }
    }

}
