using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Initialize all images with their corresponding indexes based on the garment type.
//Does this upon loading the dressing room, and does not need to do it again.
public class JewelryDisplayPopulator : MonoBehaviour
{
    //All lists of the individual clothing panels.
    public List<ClothingPanel> earrings;
    public List<ClothingPanel> necklaces;
    public List<ClothingPanel> combs;

    //SCRIPT REFS
    private JewelryDollSkin dollControls;
    private JewelryDatabase jewelryDatabase;
    private ClothingDatabase clothingDatabase;

    //List of our current clothing.
    public List<ActiveDatabaseList> currentList;
    public List<ClothingPanel> currentPanelList;

    //The unknown image.
    public Sprite unknownAsset;

    // Start is called before the first frame update
    void Start()
    {
        dollControls = FindObjectOfType<JewelryDollSkin>();
        jewelryDatabase = FindObjectOfType<JewelryDatabase>();
        clothingDatabase = FindObjectOfType<ClothingDatabase>();

        PopulateLists();
    }

    //Populate lists with the necessary items.
    //Called upon start and when a doll's pose changes.
    public void PopulateLists()
    {
        SinglePopulation("Earrings", earrings);
        SinglePopulation("Necklaces", necklaces);
        SinglePopulation("Combs", combs);
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
                panels[i].GetComponent<JewelryDrag>().subcategory = actives.itemClass;
            }
        }
        //If we have a few inactives, we need to set them up.
        if (inactives != null && inactives.assetSprites.Count > 0)
        {
            //We are setting each panel to be the mystery sprite.
            for (int k = actives.assetSprites.Count; k < (actives.assetSprites.Count + inactives.assetSprites.Count); k++)
            {
                MysteryPanel(k, panels);
            }
            //Turn off the rest of the panels.
            for (int m = (actives.assetSprites.Count + inactives.assetSprites.Count); m < panels.Count; m++)
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



    //-----------------
    //DATABASE RETURNS
    //Return the clothing list for this category.  Pose IS relevant.
    public ActiveDatabaseList ReturnClothingList(string category)
    {
        for (int i = 0; i < jewelryDatabase.activeJewelry.Count; i++)
        {
            if (jewelryDatabase.activeJewelry[i].itemClass == category && jewelryDatabase.activeJewelry[i].pose == dollControls.pose)
            {
                return jewelryDatabase.activeJewelry[i];
            }
        }
        return null;
    }

    //Return the inclothing list for this category.  Pose is irrelevant because this is purely for population display.
    public InactiveDatabaseList ReturnInactiveList(string category)
    {
        for (int i = 0; i < jewelryDatabase.inactiveJewelry.Count; i++)
        {
            if (jewelryDatabase.inactiveJewelry[i].itemClass == category && jewelryDatabase.inactiveJewelry[i].pose == dollControls.pose)
            {
                return jewelryDatabase.inactiveJewelry[i];
            }
        }
        return null;
    }




    //-----------------
    //PANEL ACTIONS
    public void SetPanel(string sig, int index, List<ClothingPanel> panelList)
    {
        panelList[index].thePanel.sprite = clothingDatabase.displayItems.displayIcons[sig];
        panelList[index].clothingActive = true;

        //Turn the image on after everything else has happened.
        panelList[index].thePanel.enabled = true;
        panelList[index].thePanel.raycastTarget = true;
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
            case "Earrings":
                currentPanelList = earrings;
                break;
            case "Necklaces":
                currentPanelList = necklaces;
                break;
            case "Combs":
                currentPanelList = combs;
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
