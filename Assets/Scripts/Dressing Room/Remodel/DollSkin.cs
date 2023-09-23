using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Controls all of the images for the different dolls and controls their garments based on the scene.
    //Skins the different doll images based on the sprites stored in the Doll class.
    //Removes clothing from individual dolls.
    //Resets clothing from individual dolls.
    //Adds clothing for individual dolls.
    //Replaces the Doll Manager.

public class DollSkin : MonoBehaviour
{
    //LISTS FOR INITIALIZATION
    //These lists are used for set up to create the doll image dictionaries.
    public List<string> clothingCategoriesElizabeth;
    public List<string> clothingCategoriesJane;

    //Note that each image has a clothing slot attached to it, so we do not need a separate list for those.
    public List<Image> elizabethImages;
    public List<Image> janeImages;

    //Jewelry loads and lists to help initialization.
    //Dictionaries aren't as necessary here since they won't be accessed more than once.
    public List<string> jewelry;
    public List<Image> eJewels;
    public List<Image> jJewels;

    //These are the actual dictionaries that will store all of the doll images / string.
    public Dictionary<string, Image> elizabethGarments;
    public Dictionary<string, Image> janeGarments;

    //-------------------------------
    //The two doll objects that can be toggled as necessary.
    public GameObject elizabeth;
    public GameObject jane;
    public string pose;

    //-------------------------------
    //SCRIPT REFERENCES
    private DataManager playerData;
    private SlotManager slotManager;
    private ClosetControls theCloset;
    private DisplayPopulator displayPopulator;


    private void Start()
    {
        playerData = FindObjectOfType<DataManager>();
        slotManager = FindObjectOfType<SlotManager>();
        theCloset = FindObjectOfType<ClosetControls>();
        displayPopulator = FindObjectOfType<DisplayPopulator>();

        //Establish Elizabeth's dictionary.
        elizabethGarments = new Dictionary<string, Image>();
        for (int i = 0; i < clothingCategoriesElizabeth.Count; i++)
        {
            elizabethGarments.Add(clothingCategoriesElizabeth[i], elizabethImages[i]);
        }

        //Establish Jane's dictionary.
        janeGarments = new Dictionary<string, Image>();
        for (int i = 0; i < clothingCategoriesJane.Count; i++)
        {
            janeGarments.Add(clothingCategoriesJane[i], janeImages[i]);
        }

        //Skin the images based on player data.
        LoadDolls();
    }


    //Take player data and load it onto the existing dolls.
    public void LoadDolls()
    {
        //Loop through the clothes in player data.
        //ELIZABETH LOOP
        foreach (KeyValuePair<string, Sprite> garment in playerData.elizabeth.clothing)
        {
            //If there is a sprite here, load it into the garments.
            if (garment.Value != null)
            {
                Debug.Log("We found a value at " + garment.Key);
                elizabethGarments[garment.Key].sprite = garment.Value;
            }
        }

        //Loop through the clothes in player data.
        //JANE LOOP
        foreach (KeyValuePair<string, Sprite> garment2 in playerData.jane.clothing)
        {
            //If there is a sprite here, load it into the garments.
            if (garment2.Value != null)
            {
                janeGarments[garment2.Key].sprite = garment2.Value;
            }
        }

        //Now we turn off images that are not in use.
        //ELIZABETH LOOP
        foreach(KeyValuePair<string, Image> img in elizabethGarments)
        {
            img.Value.enabled = (img.Value.sprite != null);
        }

        //JANE LOOP
        foreach (KeyValuePair<string, Image> img2 in janeGarments)
        {
            img2.Value.enabled = (img2.Value.sprite != null);
        }
        LoadJewelry();

        //Set the appropriate doll based on whoever was accessed last.
        elizabeth.SetActive(playerData.lastAccessedDoll == "Elizabeth");
        jane.SetActive(playerData.lastAccessedDoll == "Jane");

        pose = SetPose();

        //Set the doll slots to the right doll now that we have loaded them.
        slotManager.InitializeSlotLists();

        //Have to do this in a bit of a janky way because of startup.
        theCloset.resetAll.interactable = CheckForClothes();
    }

    //Load any existing jewelry from the previous room.
    public void LoadJewelry()
    {
        if (playerData.elizabeth.jewelry != null)
        {
            for (int i = 0; i < jewelry.Count; i++)
            {
                if (playerData.elizabeth.jewelry.ContainsKey(jewelry[i]))
                {
                    //If there is a sprite here...
                    if (playerData.elizabeth.jewelry[jewelry[i]] != null)
                    {
                        eJewels[i].sprite = playerData.elizabeth.jewelry[jewelry[i]];
                    }
                    else
                    {
                        eJewels[i].enabled = false;
                    }
                }
                else
                {
                    eJewels[i].enabled = false;
                }
            }
        }

        if (playerData.jane.jewelry != null)
        {
            for (int j = 0; j < jewelry.Count; j++)
            {
                if (playerData.jane.jewelry.ContainsKey(jewelry[j]))
                {
                    //If there is a sprite here...
                    if (playerData.jane.jewelry[jewelry[j]] != null)
                    {
                        jJewels[j].sprite = playerData.jane.jewelry[jewelry[j]];
                    }
                    else
                    {
                        jJewels[j].enabled = false;

                    }
                }
                else
                {
                    jJewels[j].enabled = false;
                }             
            }
        }
    }


    //Switch which doll is active and which is not.
    public void SwitchDolls()
    {
        elizabeth.SetActive(!elizabeth.activeSelf);
        jane.SetActive(!jane.activeSelf);
        pose = SetPose();

        FindObjectOfType<SlotManager>().ChangeSlotLists();
        FindObjectOfType<DisplayPopulator>().PopulateLists();

        theCloset.CheckButtons();
    }

    public string SetPose()
    {
        if (elizabeth.activeSelf)
        {
            return "3Q";
        }
        else
        {
            return "Straight";
        }
    }


    //Add the article of clothing based on the current category.
    //Index is determined by the index attached to the dragged item.
    //Gets increasingly complicated based on what the current category is.
    public void AddClothing(int index)
    {
        //FIRST: Run the OOBE checks.
        OOBEManager oobe = FindObjectOfType<OOBEManager>();
        if (!oobe.discoveryCodes["Remove Clothing Found"])
        {
            oobe.discoveryCodes["Remove Clothing Found"] = true;
            //Also, tell the Scene OOBE to run this specifically.
            FindObjectOfType<SceneOOBE>().InitiateActionTip("Remove Clothing Found");
        }
        else
        {
                oobe.discoveryCodes["Reset Doll Found"] = true;
                FindObjectOfType<SceneOOBE>().InitiateActionTip("Reset Doll Found");
            
        }

        int amendedIndex = displayPopulator.currentPanelList[index].amendedIndex;

        //Subcategories with double sprites.
        if (theCloset.subcategory == "Capes" || theCloset.subcategory == "Shawls")
        {
            if (CheckForClothes("Outerwear"))
            {
                if (elizabeth.activeSelf)
                {
                    RemoveGarmentWithoutSlot("Elizabeth", "Cape Front");
                    RemoveGarmentWithoutSlot("Elizabeth", "Cape Back");
                    RemoveGarmentWithoutSlot("Elizabeth", "Shawl Front");
                    RemoveGarmentWithoutSlot("Elizabeth", "Shawl Back");
                    RemoveGarmentWithoutSlot("Elizabeth", "Jackets");
                    RemoveGarmentWithoutSlot("Elizabeth", "Coats");
                }
                else
                {
                    RemoveGarmentWithoutSlot("Jane", "Cape Front");
                    RemoveGarmentWithoutSlot("Jane", "Cape Back");
                    RemoveGarmentWithoutSlot("Jane", "Shawls");
                    RemoveGarmentWithoutSlot("Jane", "Jackets");
                    RemoveGarmentWithoutSlot("Jane", "Coats");
                }
            }

            if (elizabeth.activeSelf && CheckForClothes("Overdresses"))
            {
                RemoveGarmentWithoutSlot("Elizabeth", "Overdresses");
            }
            if (jane.activeSelf && CheckForClothes("Overdresses"))
            {
                RemoveGarmentWithoutSlot("Jane", "Overdresses");
            }


            AddClothingDoubleSlot(amendedIndex);
        }else if (theCloset.subcategory == "Gloves" || theCloset.subcategory == "Hats")
        {
            AddClothingDoubleSlot(amendedIndex);
            if (theCloset.subcategory == "Hats" && elizabeth.activeSelf)
            {
                RemoveComb("Elizabeth");
            }else if (theCloset.subcategory == "Hats" && jane.activeSelf)
            {
                RemoveComb("Jane");
            }
        }
        else
        {
            if (elizabeth.activeSelf)
            {
                if (theCloset.subcategory == "Handhelds" && CheckForClothes("Handhelds"))
                {
                    RemoveGarmentWithoutSlot("Elizabeth", "Fans");
                    RemoveGarmentWithoutSlot("Elizabeth", "Handbags");
                }
                if (theCloset.subcategory == "Jackets" && CheckForClothes("Outerwear"))
                {
                    RemoveGarmentWithoutSlot("Elizabeth", "Cape Front");
                    RemoveGarmentWithoutSlot("Elizabeth", "Cape Back");
                    RemoveGarmentWithoutSlot("Elizabeth", "Shawl Front");
                    RemoveGarmentWithoutSlot("Elizabeth", "Shawl Back");
                    RemoveGarmentWithoutSlot("Elizabeth", "Coats");
                }

                //These two go here because they are single sprite outerwear.
                if ((theCloset.subcategory == "Jackets" || theCloset.subcategory == "Coats") && CheckForClothes("Overdresses"))
                {
                    RemoveGarmentWithoutSlot("Elizabeth", "Overdresses");
                }
                if (theCloset.subcategory == "Coats" && CheckForClothes("Outerwear"))
                {
                    RemoveGarmentWithoutSlot("Elizabeth", "Cape Front");
                    RemoveGarmentWithoutSlot("Elizabeth", "Cape Back");
                    RemoveGarmentWithoutSlot("Elizabeth", "Shawl Front");
                    RemoveGarmentWithoutSlot("Elizabeth", "Shawl Back");
                    RemoveGarmentWithoutSlot("Elizabeth", "Jackets");
                }
                if (theCloset.subcategory == "Overdresses" && CheckForClothes("Outerwear"))
                {
                    RemoveGarmentWithoutSlot("Elizabeth", "Cape Front");
                    RemoveGarmentWithoutSlot("Elizabeth", "Cape Back");
                    RemoveGarmentWithoutSlot("Elizabeth", "Shawl Front");
                    RemoveGarmentWithoutSlot("Elizabeth", "Shawl Back");
                    RemoveGarmentWithoutSlot("Elizabeth", "Jackets");
                }
                GarmentOn(theCloset.subcategory, amendedIndex, "Elizabeth");

            }
            else
            {
                if (theCloset.subcategory == "Handhelds" && CheckForClothes("Handhelds"))
                {
                    RemoveGarmentWithoutSlot("Jane", "Fans");
                    RemoveGarmentWithoutSlot("Jane", "Handbags");
                }
                if (theCloset.subcategory == "Jackets" && CheckForClothes("Outerwear"))
                {
                    RemoveGarmentWithoutSlot("Jane", "Cape Front");
                    RemoveGarmentWithoutSlot("Jane", "Cape Back");
                    RemoveGarmentWithoutSlot("Jane", "Shawls");
                    RemoveGarmentWithoutSlot("Jane", "Coats");
                }
                if (theCloset.subcategory == "Coats" && CheckForClothes("Outerwear")){
                    RemoveGarmentWithoutSlot("Jane", "Cape Front");
                    RemoveGarmentWithoutSlot("Jane", "Cape Back");
                    RemoveGarmentWithoutSlot("Jane", "Shawls");
                    RemoveGarmentWithoutSlot("Jane", "Jackets");
                }
                if (theCloset.subcategory == "Overdresses" && CheckForClothes("Outerwear"))
                {
                    RemoveGarmentWithoutSlot("Jane", "Cape Front");
                    RemoveGarmentWithoutSlot("Jane", "Cape Back");
                    RemoveGarmentWithoutSlot("Jane", "Shawls");
                    RemoveGarmentWithoutSlot("Jane", "Jackets");
                }

                //Now check for overdresses.
                if ((theCloset.subcategory == "Jackets" || theCloset.subcategory == "Coats" || theCloset.subcategory == "Shawls") && CheckForClothes("Overdresses")){
                    RemoveGarmentWithoutSlot("Jane", "Overdresses");
                }
                   
                GarmentOn(theCloset.subcategory, amendedIndex, "Jane");
            }
        }
    }

    //Used when we have to add sprites to two clothing items.
    public void AddClothingDoubleSlot(int index)
    {
        switch (theCloset.subcategory)
        {
            case "Gloves":
                if (elizabeth.activeSelf)
                {
                    //Gloves are LEFT, then RIGHT in the resource load
                    GarmentOn("Left Glove", index, "Elizabeth");
                    GarmentOn("Right Glove", index + 1, "Elizabeth");
                }
                else
                {
                    GarmentOn("Left Glove", index, "Jane");
                    GarmentOn("Right Glove", index + 1, "Jane");
                }
                break;
            case "Capes":
                if (elizabeth.activeSelf)
                {
                    //Capes are FRONT, then BACK in resource load
                    GarmentOn("Cape Front", index, "Elizabeth");
                    GarmentOn("Cape Back", index + 1, "Elizabeth");
                }
                else
                {
                    GarmentOn("Cape Front", index, "Jane");
                    GarmentOn("Cape Back", index + 1, "Jane");
                }
                break;
            case "Shawls":
                if (elizabeth.activeSelf)
                {
                    //Shawls are FRONT, then BACK in resource load
                    GarmentOn("Shawl Front", index, "Elizabeth");
                    GarmentOn("Shawl Back", index + 1, "Elizabeth");
                }
                else
                {
                    GarmentOn("Shawls", index, "Jane");
                }
                break;
            case "Hats":
                if (elizabeth.activeSelf)
                {
                    //Shawls are FRONT, then BACK in resource load
                    GarmentOn("Hat Front", index, "Elizabeth");
                    GarmentOn("Hat Back", index + 1, "Elizabeth");
                }
                else
                {
                    GarmentOn("Hat Front", index, "Jane");
                    GarmentOn("Hat Back", index + 1, "Jane");
                }
                break;
        }
    }

    public void GarmentOn(string category, int index, string doll)
    {
        if (doll == "Elizabeth")
        {
            elizabethGarments[category].enabled = true;
            elizabethGarments[category].sprite = theCloset.currentClothingList.assetSprites[index];
        }
        else
        {
            janeGarments[category].enabled = true;
            janeGarments[category].sprite = theCloset.currentClothingList.assetSprites[index];
        }
        slotManager.AddClothing(category);
        theCloset.EnableResetOne();
        SaveDolls();
    }

    //Remove an article of clothing based on the current category.
    public void RemoveClothing()
    {
        switch (theCloset.subcategory)
        {
            case "Hats":
                if (elizabeth.activeSelf)
                {
                    RemoveGarment("Elizabeth", "Hat Front");
                    RemoveGarment("Elizabeth", "Hat Back");
                }
                else
                {
                    RemoveGarment("Jane", "Hat Front");
                    RemoveGarment("Jane", "Hat Back");
                }
                break;
            case "Gloves":
                if (elizabeth.activeSelf)
                {
                    RemoveGarment("Elizabeth", "Right Glove");
                    RemoveGarment("Elizabeth", "Left Glove");
                }
                else
                {
                    RemoveGarment("Jane", "Right Glove");
                    RemoveGarment("Jane", "Left Glove");
                }
                break;
            case "Shawls":
                if (elizabeth.activeSelf)
                {
                    RemoveGarment("Elizabeth", "Shawl Back");
                    RemoveGarment("Elizabeth", "Shawl Front");
                }
                else
                {
                    RemoveGarment("Jane", "Shawls");
                }
                break;
            case "Capes":
                if (elizabeth.activeSelf)
                {
                    RemoveGarment("Elizabeth", "Cape Back");
                    RemoveGarment("Elizabeth", "Cape Front");
                }
                else
                {
                    RemoveGarment("Jane", "Cape Front");
                    RemoveGarment("Jane", "Cape Back");
                }
                break;
            default:
                if (elizabeth.activeSelf)
                {
                    RemoveGarment("Elizabeth", theCloset.subcategory);
                }
                else
                {
                    RemoveGarment("Jane", theCloset.subcategory);
                }
                break;
        }
    }

    public void RemoveGarment(string doll, string category)
    {
        if (doll == "Elizabeth")
        {
            elizabethGarments[category].enabled = false;
            elizabethGarments[category].sprite = null;
        }
        else
        {
            janeGarments[category].enabled = false;
            janeGarments[category].sprite = null;
        }
        slotManager.ResetSlot(category);

        //Disable reset one, since clothing has been removed in this category.
        //Also check the closet for all buttons.
        theCloset.DisableResetOne();
        theCloset.CheckButtons();
        SaveDolls();

    }


    //Used for removing items if the same category is being used.
    //I.e., if they're wearing a shawl but trying to put on a jacket.
    public void RemoveGarmentWithoutSlot(string doll, string category)
    {
        if (doll == "Elizabeth")
        {
            elizabethGarments[category].enabled = false;
            elizabethGarments[category].sprite = null;
        }
        else
        {
            janeGarments[category].enabled = false;
            janeGarments[category].sprite = null;
        }
        SaveDolls();

    }

    //Hat index is always the last in the list.
    public void RemoveComb(string doll)
    {
        if (doll == "Elizabeth")
        {
            eJewels[eJewels.Count - 1].sprite = null;
            eJewels[eJewels.Count - 1].enabled = false;
            playerData.elizabeth.jewelry["Combs"] = null;
        }
        else
        {
            jJewels[jJewels.Count - 1].sprite = null;
            jJewels[jJewels.Count - 1].enabled = false;
            playerData.jane.jewelry["Combs"] = null;
        }
    }

    //All items for the currently placed doll are reset.
    public void ResetOutfit()
    {
        //Disable every image for the currently active doll.
        //Run a check through the removal buttons to enable / disable them as needed now that no clothes are available.
        //Reset the active doll slot.
        if (elizabeth.activeSelf)
        {
            foreach(KeyValuePair<string, Image> pair in elizabethGarments)
            {
                pair.Value.enabled = false;
                pair.Value.sprite = null;
            }
        }
        else
        {
            foreach (KeyValuePair<string, Image> pair2 in janeGarments)
            {
                pair2.Value.enabled = false;
                pair2.Value.sprite = null;
            }
        }
        slotManager.ShutDownSlots();


        //Disable the buttons.
        theCloset.CheckButtons();

 //       ResetJewelry();
        SaveDolls();

    }

    //Reset the jewelry that the player came in with.
    public void ResetJewelry()
    {
        if (elizabeth.activeSelf)
        {
            for(int i = 0; i < eJewels.Count; i++)
            {
                if (eJewels[i].enabled)
                {
                    eJewels[i].sprite = null;
                    eJewels[i].enabled = false;
                    playerData.elizabeth.jewelry[jewelry[i]] = null;
                }
            }
        }
        else
        {
            for (int j = 0; j < eJewels.Count; j++)
            {
                if (jJewels[j].enabled)
                {
                    jJewels[j].sprite = null;
                    jJewels[j].enabled = false;
                    playerData.jane.jewelry[jewelry[j]] = null;
                }
            }
        }
    }

    //Get the pose of the current doll.
    public string ActivePose()
    {
        if (elizabeth.activeSelf)
        {
            return "3Q";
        }
        else
        {
            return "Straight";
        }
    }

    //Return whether or not there are any clothes on the doll currently.
    //Return true if there is clothing on the doll.
    //Return false if there is NO clothing on the doll.
    public bool CheckForClothes()
    {
        if (elizabeth.activeSelf)
        {
            foreach(KeyValuePair<string, Image> pair in elizabethGarments)
            {
                if (pair.Value.sprite != null)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            foreach (KeyValuePair<string, Image> pair2 in janeGarments)
            {
                if (pair2.Value.sprite != null)
                {
                    return true;
                }
            }
            return false;
        }
    }

    //Checks for specific clothing.
    //Returns true if there is clothing at that category.
    //Returns false if there is no clothing at that category.
    public bool CheckForClothes(string category)
    {
        if (elizabeth.activeSelf)
        {
            switch (category)
            {
                case "Outerwear":
                    //If there is a jacket, cape, or shawl, we can return true.
                    if (elizabethGarments["Cape Front"].sprite != null || elizabethGarments["Shawl Front"].sprite != null || elizabethGarments["Jackets"].sprite != null
                        || elizabethGarments["Coats"].sprite != null)
                    {
                        return true;
                    }
                    else if (elizabethGarments["Cape Front"].sprite == null && elizabethGarments["Shawl Front"].sprite == null && elizabethGarments["Jackets"].sprite == null
                        && elizabethGarments["Coats"] == null)
                        return false;
                    break;
                case "Overdresses":
                    return (elizabethGarments["Overdresses"].sprite != null);
                case "Handhelds":
                    if (elizabethGarments["Fans"].sprite != null || elizabethGarments["Handbags"].sprite != null)
                    {
                        return true;
                    }else if (elizabethGarments["Fans"].sprite == null && elizabethGarments["Handbags"].sprite == null)
                    {
                        return false;
                    }

                    break;
                case "Gloves":
                    return (elizabethGarments["Right Glove"].sprite != null);
                case "Hats":
                    return (elizabethGarments["Hat Front"].sprite != null);
                default:
                    return (elizabethGarments[category].sprite != null);
            }
            
            return false;
        }
        else
        {
            switch (category)
            {
                case "Outerwear":
                    //If there is a jacket, cape, or shawl, we can return true.
                    if (janeGarments["Cape Front"].sprite != null || janeGarments["Shawls"].sprite != null || janeGarments["Jackets"].sprite != null || janeGarments["Coats"].sprite != null)
                    {
                        return true;
                    }else if (janeGarments["Cape Front"].sprite == null && janeGarments["Shawls"].sprite == null && janeGarments["Jackets"].sprite == null && janeGarments["Coats"].sprite == null)
                    {
                        return false;
                    }

                    break;
                case "Overdresses":
                    return (janeGarments["Overdresses"].sprite != null);
                case "Handhelds":
                    if (janeGarments["Fans"].sprite != null || janeGarments["Handbags"].sprite != null)
                    {
                        return true;
                    }else if (janeGarments["Fans"].sprite == null && janeGarments["Handbags"].sprite == null)
                    {

                    }

                    break;
                case "Gloves":
                    return (janeGarments["Right Glove"].sprite != null);
                case "Hats":
                    return (janeGarments["Hat Front"].sprite != null);
                default:
                    return (janeGarments[category].sprite != null);
            }

            return false;
        }
    }

    //Look for a very specific type of garment, in a subcategory.
    public bool CheckForSubcategoryClothing(string category)
    {
        if (elizabeth.activeSelf)
        {
            switch (category)
            {
                case "Hats":
                    return (elizabethGarments["Hat Front"].sprite != null);
                case "Capes":
                    return (elizabethGarments["Cape Front"].sprite != null);
                case "Accessories":
                    return (elizabethGarments["Belts"].sprite != null);
                case "Gloves":
                    return (elizabethGarments["Right Glove"].sprite != null);
                case "Shawls":
                    return (elizabethGarments["Shawl Front"].sprite != null);
                default:
                    return (elizabethGarments[category].sprite != null);
            }
        }
        else
        {
            switch (category)
            {
                case "Hats":
                    return (janeGarments["Hat Front"].sprite != null);
                case "Capes":
                    return (janeGarments["Cape Front"].sprite != null);
                case "Accessories":
                    return (janeGarments["Belts"].sprite != null);
                case "Gloves":
                    return (janeGarments["Right Glove"].sprite != null);
                default:
                    return (janeGarments[category].sprite != null);
            }
        }
    }

    //Save the dolls based on their current garments.
    public void SaveDolls()
    {
        //Save Elizabeth's data based on her current garments.
        foreach(KeyValuePair<string, Image> pair in elizabethGarments)
        {
            if (pair.Value.sprite != null)
            {
                playerData.elizabeth.clothing[pair.Key] = pair.Value.sprite;
            }
            else
            {
                playerData.elizabeth.clothing[pair.Key] = null;
            }
        }

        //Save Jane's data based on her current garments.
        foreach (KeyValuePair<string, Image> pair2 in janeGarments)
        {
            if (pair2.Value.sprite != null)
            {
                playerData.jane.clothing[pair2.Key] = pair2.Value.sprite;
            }
            else
            {
                playerData.jane.clothing[pair2.Key] = null;
            }
        }

        //Whoever is active currently is the saved doll.
        if (elizabeth.activeSelf)
        {
            playerData.lastAccessedDoll = "Elizabeth";
        }
        else
        {
            playerData.lastAccessedDoll = "Jane";
        }
    }
}
