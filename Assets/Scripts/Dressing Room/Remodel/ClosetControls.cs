using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the images and slots that populate doll clothing.
    //Control when the clothing category changes.
    //Trigger the flyout controllers when necessary.
public class ClosetControls : MonoBehaviour
{
    //SCRIPT REFERENCES
    private DollSkin dollControls;
    private ClothingDatabase clothingDatabase;
    private DisplayPopulator displayPopulator;

    //The current category that influences doll dressing.
    public string currentCategory;

    public string subcategory; //This is the more specific category, the subcategory within the major category (i.e. fans, main category is handhelds)

    //The current list that the clothes will pull from.
    public ActiveDatabaseList currentClothingList;

    //Flyout initialization
    public List<string> categories;
    public List<FlyoutManager> flyouts;
    public Dictionary<string, FlyoutManager> flyoutControllers;

    //Reset buttons
    public List<GameObject> resets;
    public Button resetAll;
    public Dictionary<string, GameObject> resetButtons;




    // Start is called before the first frame update
    void Start()
    {
        dollControls = FindObjectOfType<DollSkin>();
        clothingDatabase = FindObjectOfType<ClothingDatabase>();
        displayPopulator = FindObjectOfType<DisplayPopulator>();

        //Initialize the flyout controller system.
        flyoutControllers = new Dictionary<string, FlyoutManager>();
        for (int i = 0; i < categories.Count; i++)
        {
            flyoutControllers.Add(categories[i], flyouts[i]);
        }

        //Setup the reset buttons.
        resetButtons = new Dictionary<string, GameObject>();
        for (int i =0; i < categories.Count; i++)
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

    //Play a sound effect when we change a category.
    public void CategoryChangeSFX(string key)
    {
        FindObjectOfType<SFXController>().PlaySpecific(key);
    }

    //Changes the category, and updates the current clothing items based on this category.
    //This works, but I don't think it's as smooth as it could be, so let's review once things are functional.
    public void ChangeCategory(string category)
    {
        if (currentCategory != category)
        {
            //Whatever the current category is has to be turned off.
            if (currentCategory != "")
            {
                if (flyoutControllers[currentCategory].active)
                {
                    flyoutControllers[currentCategory].active = false;
                    flyoutControllers[currentCategory].TransitionItems();

                    FindObjectOfType<PaginationDirector>().DisablePagination(currentCategory);
                }
            }
        }



        currentCategory = category;
        SetSubcategory(currentCategory);

        flyoutControllers[currentCategory].active = !flyoutControllers[category].active;
        flyoutControllers[currentCategory].TransitionItems();

        //Change the lists for easier dressing.
        displayPopulator.ChangePanelList(category);

        CheckButtons();
        if (flyoutControllers[currentCategory].active)
        {
            FindObjectOfType<PaginationDirector>().CheckPagination(currentCategory);
        }
        else
        {
            FindObjectOfType<PaginationDirector>().DisablePagination(currentCategory);
        }
    }

    //Change the list based on what type of garment is currently being held.
    //Called when an item is being dragged.
    //We cannot do this with the clothing buttons because those categories are broad and do not match the individual garments
    //(i.e. handhelds is a category, but belts / over dresses are contained within it and are what are represented in the active clothing)
    public void ChangeClothingList()
    {
        for (int i = 0; i < clothingDatabase.activeClothing.Count; i++)
        {
            if (clothingDatabase.activeClothing[i].itemClass == subcategory)
            {
                if (clothingDatabase.activeClothing[i].pose == dollControls.pose)
                {
                    currentClothingList = clothingDatabase.activeClothing[i];
                }
            }
        }
        displayPopulator.ChangePanelList(currentCategory);
    }

    public void CheckButtons()
    {
        if (currentCategory != "")
        {
            resetButtons[currentCategory].GetComponent<Button>().interactable = dollControls.CheckForClothes(currentCategory);
        }
        resetAll.interactable = dollControls.CheckForClothes();

    }

    //Update the buttons depending on whether or not any clothes exist on the current doll.
    public void CheckResetAllButton()
    {
        resetAll.interactable = dollControls.CheckForClothes();
    }

    //Turns off the reset one button.
    public void DisableResetOne()
    {
        resetButtons[currentCategory].GetComponent<Button>().interactable = false;
    }

    //Turns on the reset one button.
    public void EnableResetOne()
    {
        resetButtons[currentCategory].GetComponent<Button>().interactable = true;
        resetAll.interactable = true;
    }

    //Establish the subcategory based on the current category.
    public void SetSubcategory(string category)
    {
        subcategory = category;

        switch (category)
        {
            case "Outerwear":
                if (dollControls.CheckForSubcategoryClothing("Capes"))
                {
                    //This means we have a cape on.  Subcategory is capes.
                    subcategory = "Capes";
                }else if (dollControls.CheckForSubcategoryClothing("Shawls"))
                {
                    subcategory = "Shawls";
                }else if (dollControls.CheckForSubcategoryClothing("Jackets"))
                {
                    subcategory = "Jackets";
                }
                break;
            case "Accessories":
                if (dollControls.CheckForSubcategoryClothing("Belts"))
                {
                    subcategory = "Belts";
                }
                break;
            case "Handhelds":
                if (dollControls.CheckForSubcategoryClothing("Handbags"))
                {
                    subcategory = "Handbags";
                }else if (dollControls.CheckForSubcategoryClothing("Fans"))
                {
                    subcategory = "Fans";
                }
                break;
            default:
                break;
        }
    }
}
