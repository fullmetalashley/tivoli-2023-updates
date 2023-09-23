using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the dolls in the jewelry room.
//Works from the newer framework of the DollSkin script in the dressing room.
//Skins the different doll images based on the sprites stored in the Doll class.
//Removes jewelry from individual dolls.
//Resets jewelry from individual dolls.
//Adds jewelry for individual dolls.
//Replaces the JewelryDoll Manager.

public class JewelryDollSkin : MonoBehaviour
{

    //LISTS FOR INITIALIZATION
    //These lists are used for set up to create the doll image dictionaries.
    //We can use one list for all categories, because both dolls have the same categories.
    public List<string> jewelryCategories;

    //Note that each image has a clothing slot attached to it, so we do not need a separate list for those.
    public List<Image> elizabethImages;
    public List<Image> janeImages;

    //These are the actual dictionaries that will store all of the doll images / string.
    public Dictionary<string, Image> elizabethJewels;
    public Dictionary<string, Image> janeJewels;

    //-------------------------------
    //The two doll objects that can be toggled as necessary.
    public GameObject elizabeth;
    public GameObject jane;
    public string pose;

    //-------------------------------
    //SCRIPT REFERENCES
    private DataManager playerData;
    private JewelryClosetController theCloset;
    private JewelryDisplayPopulator jewelryDisplay;
    private JewelrySlotManager slotManager;

    //Specific items for hats.
    public Image eHat;
    public Image jHat;
    public Image eHatBack;
    public Image jHatBack;


    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();
        theCloset = FindObjectOfType<JewelryClosetController>();
        slotManager = FindObjectOfType<JewelrySlotManager>();
        jewelryDisplay = FindObjectOfType<JewelryDisplayPopulator>();

        //Establish Elizabeth's dictionary.
        elizabethJewels = new Dictionary<string, Image>();
        janeJewels = new Dictionary<string, Image>();

        for (int i = 0; i < jewelryCategories.Count; i++)
        {
            janeJewels.Add(jewelryCategories[i], janeImages[i]);
            elizabethJewels.Add(jewelryCategories[i], elizabethImages[i]);
        }

        //Load existing images onto the dolls.
        LoadDolls();
    }

    //Load the existing jewelry onto the dolls.
    public void LoadDolls()
    {

        //Loop through the jewels in player data.
        //ELIZABETH LOOP
        foreach (KeyValuePair<string, Sprite> jewel in playerData.elizabeth.jewelry)
        {
            //If there is a sprite here, load it into the garments.
            if (jewel.Value != null)
            {
                elizabethJewels[jewel.Key].sprite = jewel.Value;
            }
        }

        //Loop through the clothes in player data.
        //JANE LOOP
        foreach (KeyValuePair<string, Sprite> jewel2 in playerData.jane.jewelry)
        {
            //If there is a sprite here, load it into the garments.
            if (jewel2.Value != null)
            {
                janeJewels[jewel2.Key].sprite = jewel2.Value;
            }
        }

        //Now we turn off images that are not in use.
        //ELIZABETH LOOP
        foreach (KeyValuePair<string, Image> img in elizabethJewels)
        {
            img.Value.enabled = (img.Value.sprite != null);
        }

        //JANE LOOP
        foreach (KeyValuePair<string, Image> img2 in janeJewels)
        {
            img2.Value.enabled = (img2.Value.sprite != null);
        }

        //Set up hats if they exist on the dolls.
        if (playerData.elizabeth.clothing != null)
        {
            if (playerData.elizabeth.clothing.ContainsKey("Hat Front"))
            {
                if (playerData.elizabeth.clothing["Hat Front"] != null)
                {
                    eHat.enabled = true;
                    eHat.sprite = playerData.elizabeth.clothing["Hat Front"];

                    eHatBack.enabled = true;
                    eHatBack.sprite = playerData.elizabeth.clothing["Hat Back"];
                }
            }
        }
        if (playerData.jane.clothing != null){
            if (playerData.jane.clothing.ContainsKey("Hat Front"))
            {
                if (playerData.jane.clothing["Hat Front"] != null)
                {
                    jHat.enabled = true;
                    jHat.sprite = playerData.jane.clothing["Hat Front"];

                    jHatBack.enabled = true;
                    jHatBack.sprite = playerData.jane.clothing["Hat Back"];
                }
            }
        }

        //Set the appropriate doll based on whoever was accessed last.
        elizabeth.SetActive(playerData.lastAccessedDoll == "Elizabeth");
        jane.SetActive(playerData.lastAccessedDoll == "Jane");

        pose = SetPose();
        SwitchHats();

        //Set the doll slots to the right doll now that we have loaded them.
        slotManager.InitializeSlotLists();

        //Check to see if the removal button needs to be turned on / off.
        if (!CheckForClothes())
        {
            theCloset.DisableResetOne();
        }
        else
        {
            theCloset.EnableResetOne();
        }


    }

    //Set the pose based on the current doll.
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

    //Switch which doll is active and which is not.
    public void SwitchDolls()
    {
        elizabeth.SetActive(!elizabeth.activeSelf);
        jane.SetActive(!jane.activeSelf);
        pose = SetPose();
        SwitchHats();

        jewelryDisplay.PopulateLists();
        slotManager.ChangeSlotLists();
        theCloset.CheckButtons();

    }

    //Switch what hats are turned on, if either are active.
    public void SwitchHats()
    {
        if (pose == "3Q")
        {
            jHat.enabled = false;
            jHatBack.enabled = false;
            if (eHat.sprite != null)
            {
                //If a hat exists, turn this back on.
                eHat.enabled = true;
                eHatBack.enabled = true;
            }
        }
        else
        {
            eHat.enabled = false;
            eHatBack.enabled = false;
            if (jHat.sprite != null)
            {
                jHat.enabled = true;
                jHatBack.enabled = true;
            }
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
            oobe.discoveryCodes["Doll Toggle Found"] = true;
            FindObjectOfType<SceneOOBE>().InitiateActionTip("Reset Doll Found");

        }

        int amendedIndex = jewelryDisplay.currentPanelList[index].amendedIndex;
        if (theCloset.subcategory == "Earrings")
        {
            AddClothingDoubleSlot(amendedIndex);
        }
        else
        {
            if (elizabeth.activeSelf)
            {
                if (theCloset.currentCategory == "Combs")
                {
                    RemoveHat();
                }
                GarmentOn(theCloset.subcategory, index, "Elizabeth");

            }
            else
            {
                if (theCloset.currentCategory == "Combs")
                {
                    RemoveHat();
                }
                GarmentOn(theCloset.subcategory, index, "Jane");
            }
        }
    }

    //Used when we have to add sprites to two clothing items.
    //This will only ever happen for earrings in here, but we'll leave it built in just in case.
    public void AddClothingDoubleSlot(int index)
    {
        switch (theCloset.subcategory)
        {
            case "Earrings":
                if (elizabeth.activeSelf)
                {
                    GarmentOn("Left Earring", index, "Elizabeth");
                    GarmentOn("Right Earring", index, "Elizabeth");
                }
                else
                {
                    GarmentOn("Left Earring", index, "Jane");
                    GarmentOn("Right Earring", index, "Jane");
                }
                break;
            default:
                break;
        }
    }

    public void GarmentOn(string category, int index, string doll)
    {
        if (doll == "Elizabeth")
        {
            elizabethJewels[category].enabled = true;
            elizabethJewels[category].sprite = theCloset.currentClothingList.assetSprites[index];
        }
        else
        {
            janeJewels[category].enabled = true;
            janeJewels[category].sprite = theCloset.currentClothingList.assetSprites[index];
        }
        slotManager.AddClothing(category);
        theCloset.EnableResetOne();
        SaveDolls();

    }

    //Remove an article of clothing based on the current category.
    public void RemoveClothing()
    {
        switch (theCloset.currentCategory)
        {
            case "Earrings":
                if (elizabeth.activeSelf)
                {
                    RemoveGarment("Elizabeth", "Right Earring");
                    RemoveGarment("Elizabeth", "Left Earring");
                }
                else
                {
                    RemoveGarment("Jane", "Right Earring");
                    RemoveGarment("Jane", "Left Earring");
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

    //Remove the hat if the player adds a comb.
    public void RemoveHat()
    {
        if (elizabeth.activeSelf)
        {
            eHat.sprite = null;
            eHat.enabled = false;

            eHatBack.sprite = null;
            eHatBack.enabled = false;

            if (playerData.elizabeth.clothing.ContainsKey("Hat Front"))
            {
                playerData.elizabeth.clothing["Hat Front"] = null;
                playerData.elizabeth.clothing["Hat Back"] = null;
            }
        }
        else
        {
            jHat.sprite = null;
            jHat.enabled = false;

            jHatBack.sprite = null;
            jHatBack.enabled = false;

            if (playerData.jane.clothing.ContainsKey("Hat Front"))
            {
                playerData.jane.clothing["Hat Front"] = null;
                playerData.jane.clothing["Hat Back"] = null;
            }

        }
        SaveDolls();

    }

    public void RemoveGarment(string doll, string category)
    {
        Debug.Log("Reset pressed with doll " + doll + " and category " + category);
        if (category == "Earrings")
        {
            if (doll == "Elizabeth")
            {
                elizabethJewels["Right Earring"].enabled = false;
                elizabethJewels["Left Earring"].enabled = false;

                elizabethJewels["Left Earring"].sprite = null;
                elizabethJewels["Right Earring"].sprite = null;
            }
            else
            {
                janeJewels["Right Earring"].enabled = false;
                janeJewels["Left Earring"].enabled = false;

                janeJewels["Left Earring"].sprite = null;
                janeJewels["Right Earring"].sprite = null;
            }
        }
        else
        {
            if (doll == "Elizabeth")
            {
                elizabethJewels[category].enabled = false;
                elizabethJewels[category].sprite = null;
            }
            else
            {
                janeJewels[category].enabled = false;
                janeJewels[category].sprite = null;
            }
        }
        slotManager.ResetSlot(category);

        theCloset.CheckButtons();
        SaveDolls();


        //Disable reset one, since clothing has been removed in this category.
        //Also check the closet for all buttons.
    }

    //All items for the currently placed doll are reset.
    public void ResetOutfit()
    {
        //Disable every image for the currently active doll.
        //Run a check through the removal buttons to enable / disable them as needed now that no clothes are available.
        //Reset the active doll slot.
        if (elizabeth.activeSelf)
        {
            foreach (KeyValuePair<string, Image> pair in elizabethJewels)
            {
                pair.Value.sprite = null;
            }
        }
        else
        {
            foreach (KeyValuePair<string, Image> pair2 in janeJewels)
            {
                pair2.Value.sprite = null;
            }
        }
        slotManager.ShutDownSlots();

        //Disable the buttons.
        theCloset.CheckButtons();
        SaveDolls();

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

    public string ActiveDoll()
    {
        if (elizabeth.activeSelf)
        {
            return "Elizabeth";
        }
        else
        {
            return "Jane";
        }
    }

    //Return whether or not there are any clothes on the doll currently.
    //Return true if there is clothing on the doll.
    //Return false if there is NO clothing on the doll.
    public bool CheckForClothes()
    {
        if (elizabeth.activeSelf)
        {
            foreach (KeyValuePair<string, Image> pair in elizabethJewels)
            {
                if (pair.Value.sprite != null)
                {
                    return true;
                }
            }
            if (eHat.sprite != null)
            {
                return true;
            }
            return false;
        }
        else
        {
            foreach (KeyValuePair<string, Image> pair2 in janeJewels)
            {
                if (pair2.Value.sprite != null)
                {
                    return true;
                }
            }
            if (jHat.sprite != null)
            {
                return true;
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
            /*
            if (eHat.sprite != null)
            {
                return true;
            }
            */
            if (category == "Earrings")
            {
                //If there is a jacket, cape, or shawl, we can return true.
                if (elizabethJewels["Right Earring"].sprite != null || elizabethJewels["Left Earring"].sprite != null)
                {
                    return true;
                }
            }
            else
            {
                return (elizabethJewels[category].sprite != null);
            }

            return false;
        }
        else
        {
            /*
            if (jHat.sprite != null)
            {
                return true;
            }
            */
            if (category == "Earrings")
            {
                //If there is a jacket, cape, or shawl, we can return true.
                if (janeJewels["Left Earring"].sprite != null || janeJewels["Right Earring"].sprite != null)
                {
                    return true;
                }
            }            
            else
            {
                return (janeJewels[category].sprite != null);
            }

            return false;
        }
    }

    //Save the dolls based on their current garments.
    public void SaveDolls()
    {
        //Save Elizabeth's data based on her current garments.
        foreach (KeyValuePair<string, Image> pair in elizabethJewels)
        {
            if (pair.Value.sprite != null)
            {
                Debug.Log("Jewelry saved: " + pair.Key);
                playerData.elizabeth.jewelry[pair.Key] = pair.Value.sprite;
            }
            else
            {
                playerData.elizabeth.jewelry[pair.Key] = null;
            }
        }

        //Save Jane's data based on her current garments.
        foreach (KeyValuePair<string, Image> pair2 in janeJewels)
        {
            if (pair2.Value.sprite != null)
            {
                playerData.jane.jewelry[pair2.Key] = pair2.Value.sprite;
            }
            else
            {
                playerData.jane.jewelry[pair2.Key] = null;
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
