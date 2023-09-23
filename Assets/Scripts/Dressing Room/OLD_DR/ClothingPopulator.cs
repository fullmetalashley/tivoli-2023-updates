using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothingPopulator : MonoBehaviour
{

    //The clothing popular sets up all of the icons in the images corresponding with the current category.
    //With the new system, this would only need to happen at the start of entering the scene.

    //This is a list of the panels that fit into the scroll window.
    public List<ClothingPanel> thePanels;

    //Accesses the closet manager.
    private ClosetManager theCloset;
    private DollManager dollControls;


    private ClothingDatabase clothingLists;


    public ActiveDatabaseList currentList;

    public string currentCategory;

    public int timesRun;

    public Sprite unknownAsset;

    public bool outerwearOn;
    public int outerwearIndex;

    public int lastJacketIndex;
    public int lastShawlIndex;
    public int lastCapeIndex;
    public int lastCoatIndex;

    public List<int> jacketIndexes;
    public List<int> shawlIndexes;
    public List<int> capeIndexes;
    public List<int> coatIndexes;

    public Scrollbar panelScroll;




    // Start is called before the first frame update
    void Start()
    {
        theCloset = FindObjectOfType<ClosetManager>();
        dollControls = FindObjectOfType<DollManager>();
        clothingLists = FindObjectOfType<ClothingDatabase>();

        //Okay, so now we have our list of clothing, active and inactive.  So let's set up the current list to dresses, straight.
        for (int i = 0; i < clothingLists.activeClothing.Count; i++)
        {
            if (clothingLists.activeClothing[i].itemClass == currentCategory)
            {
                if (clothingLists.activeClothing[i].pose == dollControls.currentDoll.pose)
                {
                    currentList = clothingLists.activeClothing[i];
                }
            }
        }
        //Run the baseline population for the dresses category.
        PopulationDisplay();
    }

    //This method is called multiple times in order to set all panels to a baseline of being empty, with no clothing, and not being interactable.
    public void ClearAllPanels()
    {
        for (int p = 0; p < thePanels.Count; p++)
        {
            thePanels[p].clothingActive = false;
            thePanels[p].thePanel.enabled = false;
            thePanels[p].thePanel.sprite = null;
        }
    }

    //Sets a specific panel as a mystery panel.
    public void MysteryPanel(int index)
    {
        thePanels[index].thePanel.sprite = unknownAsset;
        thePanels[index].clothingActive = false;
        thePanels[index].thePanel.enabled = true;
        thePanels[index].thePanel.raycastTarget = false;
    }

    //Turns off a specific panel at the end of a list.
    public void DisablePanel(int index)
    {
        thePanels[index].clothingActive = false;
        thePanels[index].thePanel.enabled = false;
        thePanels[index].thePanel.sprite = null;
    }

    public void SetPanel(string sig, int index)
    {
        thePanels[index].thePanel.sprite = clothingLists.displayItems.displayIcons[sig];
        thePanels[index].clothingActive = true;

        //Turn the image on after everything else has happened.
        thePanels[index].thePanel.enabled = true;
        thePanels[index].thePanel.raycastTarget = true;


    }

    //Return how many inactive assets are currently in this list.
    public int ReturnInactives()
    {
        int inactiveAssets = 0;
        for (int i = 0; i < clothingLists.inactiveClothing.Count; i++)
        {
            if (clothingLists.inactiveClothing[i].itemClass == currentCategory)
            {
                if (clothingLists.inactiveClothing[i].pose == dollControls.currentDoll.pose)
                {
                    inactiveAssets += clothingLists.inactiveClothing[i].assetSprites.Count;
                }
            }
        }
        return inactiveAssets;
    }

    public void PopulationDisplay()
    {
        //Clear all panels at the start of each population.
        ClearAllPanels();
        //Establish how many inactives are in this particular list.
        int inactiveAssets = ReturnInactives();

        //If the list is empty, then we need to clear out the images.
        if (currentList.empty)
        {

            //Go through the inactive panels and set them to be the mystery sprite.
            for (int k = 0; k < inactiveAssets; k++)
            {
                MysteryPanel(k);
            }

            //Go through remaining panels and turn them off.
            //I don't think we really need to do this, but it's an extra safeguard.
            for (int m = inactiveAssets; m < thePanels.Count; m++)
            {
                DisablePanel(m);
            }
        }
        else
        {
            //Current list is not empty.
            //Setting the images.  Get the sig from the sprite, and find the corresponding image in the display items.
            Debug.Log("We have this many: " + currentList.assetSprites.Count);
            Debug.Log("Current category: " + currentCategory);
            Debug.Log("How many panels are there: " + thePanels.Count);
            for (int i = 0; i < currentList.assetSprites.Count; i++)
            {
                string[] splitString = currentList.assetSprites[i].name.Split("$"[0]);
                splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                string currentSig = splitString[splitString.Length - 1];

                Debug.Log("Current sig: " + currentSig + " at index: " + i);
                SetPanel(currentSig, i);
            }

            //We are setting each panel to be the mystery sprite.
            for (int k = currentList.assetSprites.Count; k < (currentList.assetSprites.Count + inactiveAssets); k++)
            {
                MysteryPanel(k);
            }
            //Turn off the rest of the panels.
            for (int m = (currentList.assetSprites.Count + inactiveAssets); m < thePanels.Count; m++)
            {
                DisablePanel(m);
            }        
        }
    }



    //JACKETS DO NOT NEED TO BE STAGGERED FOR EITHER 3Q OR STR8 POSES.
    void PopulationJackets()
    {
        ClearAllPanels();
        outerwearIndex = 0;     //This can always be 0 because we will always start with jackets.
        int inactiveAssets = ReturnInactives();

        //If the list is empty, then we need to clear out the images.
        if (currentList.empty)
        {
            //If this list is empty, we do the mystery sprites first.
            //Go through the inactive panels and set them to be the mystery sprite.
            for (int k = 0; k < inactiveAssets; k++)
            {
                MysteryPanel(k);
            }
            //Go through remaining panels and turn them off.
            //I don't think we really need to do this, but it's an extra safeguard.
            for (int m = inactiveAssets; m < thePanels.Count; m++)
            {
                DisablePanel(m);
            }
        }
        else
        {
            //Setting the images.  Get the sig from the sprite, and find the corresponding image in the display items.
            //We can use the current list for this because it will be jackets.
            for (int i = 0; i < currentList.assetSprites.Count; i++)
            {
                string[] splitString = currentList.assetSprites[i].name.Split("$"[0]);
                splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                string currentSig = splitString[splitString.Length - 1];

                SetPanel(currentSig, i);
            }




            //Now that we know inactive jackets, we start at the last index of the jacket, make that the mystery sprite, and use that for the next piece.
            for (int m = currentList.assetSprites.Count; m < (currentList.assetSprites.Count + inactiveAssets); m++)
            {
                MysteryPanel(m);
            }

            //Tracks where we placed our last jacket, so we can start the next set of outerwear after this.
            outerwearIndex += currentList.assetSprites.Count + inactiveAssets;
            //Tracks where the last jacket is in the list.
            lastJacketIndex = outerwearIndex - 1;
        }
    }

    //SHAWLS NEED TO BE STAGGERED FOR 3Q P1.  THIS IS A STAGGERED METHOD.
    void PopulationShawls()
    {
        int inactiveAssets = ReturnInactives();
        if (currentList.empty)
        {
            //DO NOTHING.  Leave the outerwear index as it is, and move along.  This isn't last in the order, so don't turn anything off.
        }
        else
        {
            //Setting the images.  Get the sig from the sprite, and find the corresponding image in the display items.
            //The issue: We need to change our list, and we need to start at the next panel.
            ActiveDatabaseList thisList = OuterwearListChange("Shawls");

            for (int i = 0; i < thisList.assetSprites.Count; i += 2)
            {
                string[] splitString = thisList.assetSprites[i].name.Split("$"[0]);
                splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                string currentSig = splitString[splitString.Length - 1];

                SetPanel(currentSig, outerwearIndex);
                outerwearIndex++;
            }
            //How many inactives do we have?  We know that.  So divide it by 2.  THIS IS MYSTERY SPRITES FOR A STAGGERED METHOD.
            int halvedAssets = inactiveAssets / 2;
            for (int k = currentList.assetSprites.Count; k < (currentList.assetSprites.Count + halvedAssets); k++)
            {
                MysteryPanel(k);
            }
            lastShawlIndex = outerwearIndex - 1;
        }
    }

    //SHAWLS DO NOT NEED TO BE STAGGERED FOR THE STR8 P2 POSE.  THIS IS UNSTAGGERED.
    void PopulationShawlsStraight()
    {
        int inactiveAssets = ReturnInactives();
        //Setting the images.  Get the sig from the sprite, and find the corresponding image in the display items.
        //We can use the current list for this because it will be jackets.
        ActiveDatabaseList thisList = OuterwearListChange("Shawls");

        int length = thisList.assetSprites.Count + outerwearIndex;

        for (int i = 0; i < thisList.assetSprites.Count; i++)
        {
            string[] splitString = thisList.assetSprites[i].name.Split("$"[0]);
            splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
            string currentSig = splitString[splitString.Length - 1];

            SetPanel(currentSig, (i + outerwearIndex));
        }

        //Now that we know inactive jackets, we start at the last index of the jacket, make that the mystery sprite, and use that for the next piece.
        for (int m = currentList.assetSprites.Count; m < (currentList.assetSprites.Count + inactiveAssets); m++)
        {
            MysteryPanel(m);
        }
        //Tracks where we placed our last jacket, so we can start the next set of outerwear after this.
        outerwearIndex += currentList.assetSprites.Count;
        //Tracks where the last jacket is in the list.
        lastShawlIndex = outerwearIndex - 1;

    }

    //COATS DO NOT NEED TO BE STAGGERED FOR THE STR8 P2 POSE.  THIS IS UNSTAGGERED.
    void PopulationCoats()
    {
        int inactiveAssets = ReturnInactives();
        //Setting the images.  Get the sig from the sprite, and find the corresponding image in the display items.
        //We can use the current list for this because it will be jackets.
        ActiveDatabaseList thisList = OuterwearListChange("Coats");
        Debug.Log("Starting outerwear index: " + outerwearIndex);
        Debug.Log("We have this many coats to parse: " + thisList.assetSprites.Count);
        int length = thisList.assetSprites.Count + outerwearIndex;

        for (int i = 0; i < thisList.assetSprites.Count; i++)
        {
            string[] splitString = thisList.assetSprites[i].name.Split("$"[0]);
            splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
            string currentSig = splitString[splitString.Length - 1];

            SetPanel(currentSig, (i + outerwearIndex));
        }

        //Now that we know inactive jackets, we start at the last index of the jacket, make that the mystery sprite, and use that for the next piece.
        for (int m = currentList.assetSprites.Count; m < (currentList.assetSprites.Count + inactiveAssets); m++)
        {
            MysteryPanel(m);
        }
        //Tracks where we placed our last jacket, so we can start the next set of outerwear after this.
        Debug.Log("Outerwear index prior to addition: " + outerwearIndex);
        outerwearIndex += thisList.assetSprites.Count;
        Debug.Log("Outerwear index after addition: " + outerwearIndex);
        //Tracks where the last jacket is in the list.
        lastCoatIndex = outerwearIndex - 1;

    }

    //CAPES NEED TO BE STAGGERED FOR BOTH 3Q AND STR8 POSES.  THIS IS A STAGGERED LOOP.
    void PopulationCapes()
    {
        int inactiveAssets = ReturnInactives();
        if (currentList.empty)
        {
            //Do nothing.  Leave the outerwear index as it is, and move along.  This isn't last in the order, so don't turn anything off.
        }
        else
        {
            //Setting the images.  Get the sig from the sprite, and find the corresponding image in the display items.
            //The issue: We need to change our list, and we need to start at the next panel.
            ActiveDatabaseList thisList = OuterwearListChange("Capes");

            for (int i = 0; i < thisList.assetSprites.Count; i += 2)
            {
                string[] splitString = thisList.assetSprites[i].name.Split("$"[0]);
                splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                string currentSig = splitString[splitString.Length - 1];

                SetPanel(currentSig, outerwearIndex);
                outerwearIndex++;
            }

            //How many inactives do we have?  We know that.  So divide it by 2.  THIS IS MYSTERY SPRITES FOR A STAGGERED METHOD.
            int halvedAssets = inactiveAssets / 2;
            for (int k = currentList.assetSprites.Count; k < (currentList.assetSprites.Count + halvedAssets); k++)
            {
                MysteryPanel(k);
            }

            //Now, we can disable the remaining panels.
            for (int j = outerwearIndex; j < thePanels.Count; j++)
            {
                DisablePanel(j);
            }

            lastCapeIndex = outerwearIndex - 1;
        }
    }

    //Population fills the panels with the images of the current loaded list.
    void PopulationStagger()
    {
        ClearAllPanels();
        int inactiveAssets = ReturnInactives();

        if (currentList.empty)
        {
            //First go through inactives.
            int halvedAssets = inactiveAssets / 2;
            for (int k = halvedAssets; k < (currentList.assetSprites.Count + halvedAssets); k++)
            {
                MysteryPanel(k);
            }
            //Then disable all remaining.
            for (int j = (currentList.assetSprites.Count + halvedAssets); j < thePanels.Count; j++)
            {
                DisablePanel(j);
            }
        }
        else
        {
            //Setting the images.  Get the sig from the sprite, and find the corresponding image in the display items.
            for (int i = 0; i < currentList.assetSprites.Count; i += 2)
            {
                string[] splitString = currentList.assetSprites[i].name.Split("$"[0]);
                splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                string currentSig = splitString[splitString.Length - 1];

                SetPanel(currentSig, (i / 2));
            }

            //How many inactives do we have?  We know that.  So divide it by 2.
            int halvedAssets = inactiveAssets / 2;
            Debug.Log("Starting mystery sprites at " + (currentList.assetSprites.Count / 2));
            for (int k = (currentList.assetSprites.Count / 2); k < ((currentList.assetSprites.Count / 2)+ halvedAssets); k++)
            {
                MysteryPanel(k);


            }
            Debug.Log("Starting turn off of panels at " + ((currentList.assetSprites.Count / 2) + halvedAssets));
            for (int m = ((currentList.assetSprites.Count / 2)+ halvedAssets); m < thePanels.Count; m++)
            {
                DisablePanel(m);
            }
        }
    }


        public ActiveDatabaseList OuterwearListChange(string outerwear)
        {
            for (int i = 0; i < clothingLists.activeClothing.Count; i++)
            {
                if (clothingLists.activeClothing[i].itemClass == outerwear)
                {
                    if (clothingLists.activeClothing[i].pose == dollControls.currentDoll.pose)
                    {
                        return clothingLists.activeClothing[i];
                    }
                }
            }
            return null;
        }

        public void SwitchToOuterwear(string outerwear)
        {
            panelScroll.value = 1;
            outerwearOn = true;
            currentCategory = "Jackets";
            outerwearIndex = 0;

            bool listActive = false;
            //Loop through the dictionary.
            for (int i = 0; i < clothingLists.activeClothing.Count; i++)
            {
                if (clothingLists.activeClothing[i].itemClass == "Jackets")
                {
                    if (clothingLists.activeClothing[i].pose == dollControls.currentDoll.pose)
                    {
                        currentList = clothingLists.activeClothing[i];
                        listActive = true;
                        currentList.empty = false;
                    }
                }
            }
            if (!listActive)
            {
                currentList.empty = true;
            }
            PopulationOuterwear();

        dollControls.CheckRemovalButtons();

    }

    public void OuterwearSwapLists(string outerwear)
        {
            bool listActive = false;
            //Loop through the dictionary.
            for (int i = 0; i < clothingLists.activeClothing.Count; i++)
            {
                if (clothingLists.activeClothing[i].itemClass == outerwear)
                {
                    if (clothingLists.activeClothing[i].pose == dollControls.currentDoll.pose)
                    {
                        currentList = clothingLists.activeClothing[i];
                        listActive = true;
                        currentList.empty = false;
                    }
                }
            }
            if (!listActive)
            {
                currentList.empty = true;
            }


    }



    public void PopulationOuterwear()
    {
        //First, jackets.  These can use a standard population method.
        PopulationJackets();
        if (dollControls.currentDoll.pose == "Straight")
        {
            PopulationShawlsStraight();
        }
        else
        {
            PopulationShawls();

        }
 //       PopulationCoats();
        PopulationCapes();

        jacketIndexes.Clear();
        shawlIndexes.Clear();
        capeIndexes.Clear();

        //Now create the index lists.
        for (int a = 0; a < lastJacketIndex + 1; a++)
        {
            jacketIndexes.Add(a);
        }
        for (int b = lastJacketIndex + 1; b < lastShawlIndex + 1; b++)
        {
            shawlIndexes.Add(b);
        }
        for (int c = lastShawlIndex + 1; c < lastCapeIndex + 1; c++)
        {
            capeIndexes.Add(c);
        }


    }

    //This is called when a new button is pressed.  It uses the category as set by this script.
    //Adjusted to utilize a dictionary system to reduce complication of if statements when additional categories are added.
    public void SwitchLists(string newCategory, List<ClothingPanel> newPanels)
    {

        thePanels = newPanels;

        outerwearOn = false;
        currentCategory = newCategory;
        bool listActive = false;
        //Loop through the dictionary.
        for (int i = 0; i < clothingLists.activeClothing.Count; i++)
        {
            if (clothingLists.activeClothing[i].itemClass == newCategory)
            {
                if (clothingLists.activeClothing[i].pose == dollControls.currentDoll.pose)
                {
                    currentList = clothingLists.activeClothing[i];
                    listActive = true;
                    currentList.empty = false;
                }
            }
        }
        if (!listActive)
        {
            currentList.empty = true;
        }

        if (currentCategory == "Outerwear")
        {
            SwitchToOuterwear("outerwear");
        }
        else if (currentCategory == "Handhelds")
        {
            //We will build this out later.
        }
        else if (currentCategory == "Gloves")
        {
            PopulationStagger();
        }
        else
        {
            PopulationDisplay();
        }
        theCloset.NewSlotActive();
        dollControls.CheckRemovalButtons();
    }
}
