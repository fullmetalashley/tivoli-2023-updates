using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the images and slots that populate doll jewelry.
//Control when the clothing category changes.
//Trigger the flyout controllers when necessary.
public class JewelryClosetController : MonoBehaviour
{
    //SCRIPT REFERENCES
    private JewelryDollSkin dollControls;
    private JewelryDatabase jewelryDatabase;
    private JewelryDisplayPopulator displayPopulator;

    //The current category that influences doll dressing.
    public string currentCategory;

    public string subcategory; //This is the more specific category, the subcategory within the major category (i.e. fans, main category is handhelds)

    //The current list that the clothes will pull from.
    public ActiveDatabaseList currentClothingList;

    //Reset buttons
    public Button resetOne;

    //UPDATED FLYOUTS
    public List<FlyoutManager> flyouts;
    public Dictionary<string, FlyoutManager> flyoutControllers;
    public List<string> categories;
    public string tempCategory;

    //Reset buttons
    public List<GameObject> resets;
    public Button resetAll;
    public Dictionary<string, GameObject> resetButtons;

    // Start is called before the first frame update
    void Start()
    {
        dollControls = FindObjectOfType<JewelryDollSkin>();
        jewelryDatabase = FindObjectOfType<JewelryDatabase>();
        displayPopulator = FindObjectOfType<JewelryDisplayPopulator>();

        //Initialize the flyout controller system.
        flyoutControllers = new Dictionary<string, FlyoutManager>();
        for (int i = 0; i < categories.Count; i++)
        {
            flyoutControllers.Add(categories[i], flyouts[i]);
        }

        //Setup the reset buttons.
        resetButtons = new Dictionary<string, GameObject>();
        for (int i = 0; i < categories.Count; i++)
        {
            resetButtons.Add(categories[i], resets[i]);
        }

        //If the player has been sent here by an item delivery, let's open up that category.  IF it's one that belongs in this room.
        if (FindObjectOfType<DataManager>().categoryToOpen != "" && categories.Contains(FindObjectOfType<DataManager>().categoryToOpen))
        {
            ChangeCategory(FindObjectOfType<DataManager>().categoryToOpen);
            FindObjectOfType<DataManager>().categoryToOpen = "";
        }

    }


    //Play a specific sound when a category is changed.
    public void ChangeCategorySFX(string category)
    {
        FindObjectOfType<SFXController>().PlaySpecific(category);
    }

    //Changes the category, and updates the current clothing items based on this category.
    //This works, but I don't think it's as smooth as it could be, so let's review once things are functional.
    public void ChangeCategory(string category)
    {

        //Matches the dressing category.
        if (currentCategory != category)
        {
            //Whatever the current category is has to be turned off.
            if (currentCategory != "")
            {
                if (flyoutControllers[currentCategory].active)
                {
                    flyoutControllers[currentCategory].active = false;
                    flyoutControllers[currentCategory].TransitionItems();
                }
            }

        }
        currentCategory = category;
        subcategory = category;
        flyoutControllers[currentCategory].active = !flyoutControllers[currentCategory].active;
        flyoutControllers[currentCategory].TransitionItems();




        //Change the lists for easier dressing.
        displayPopulator.ChangePanelList(currentCategory);

        CheckButtons();

    }

    //Change the list based on what type of garment is currently being held.
    //Called when an item is being dragged.
    //We cannot do this with the clothing buttons because those categories are broad and do not match the individual garments
    //(i.e. handhelds is a category, but belts / over dresses are contained within it and are what are represented in the active clothing)
    public void ChangeClothingList()
    {
        for (int i = 0; i < jewelryDatabase.activeJewelry.Count; i++)
        {
            if (jewelryDatabase.activeJewelry[i].itemClass == subcategory)
            {
                if (jewelryDatabase.activeJewelry[i].pose == dollControls.pose)
                {
                    currentClothingList = jewelryDatabase.activeJewelry[i];
                }
            }
        }
        displayPopulator.ChangePanelList(currentCategory);
    }

    //Update the buttons depending on whether or not any clothes exist on the current doll.
    public void CheckButtons()
    {

        if (currentCategory != "")
        {
            resetButtons[currentCategory].GetComponent<Button>().interactable = dollControls.CheckForClothes(currentCategory);
            resetAll.interactable = dollControls.CheckForClothes();
        }

    
    }

    //Turns off the reset one button.
    public void DisableResetOne()
    {
        resetOne.interactable = false;
    }

    //Turns on the reset one button.
    public void EnableResetOne()
    {
        if (currentCategory != "")
        {
            resetButtons[currentCategory].GetComponent<Button>().interactable = true;
        }
        resetAll.interactable = true;
    }

    public void CheckResetAllButton()
    {
        resetAll.interactable = dollControls.CheckForClothes();
    }
}
