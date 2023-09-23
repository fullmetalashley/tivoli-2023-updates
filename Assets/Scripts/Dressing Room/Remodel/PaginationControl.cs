using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Controls the pagination for each individual garment category.
public class PaginationControl : MonoBehaviour
{
    //The main list of each list of panels.
    public List<List<Image>> allPanels;

    //Each individual list of panels.  Not all will be used depending on category.
    public List<Image> panels1;
    public List<Image> panels2;
    public List<Image> panels3;
    public List<Image> panels4;

    //Tracking indexes.
    public int currentPage;
    public int currentItemCount;
    public int maxPageCount;

    //Script refs
    public ClothingDatabase clothing;
    public ClosetControls theCloset;
    public DisplayPopulator display;

    //UI Refs to pagination buttons
    public GameObject leftPage;
    public GameObject rightPage;

    //Turn off all lists except the first one at start.
    public void Initialize()
    {
        clothing = FindObjectOfType<ClothingDatabase>();
        theCloset = FindObjectOfType<ClosetControls>();
        display = FindObjectOfType<DisplayPopulator>();

        currentItemCount = (currentPage * 9) + 9;

        allPanels = new List<List<Image>>
        {
            panels1,
            panels2,
            panels3,
            panels4
        };

        for (int i = 1; i < allPanels.Count; i++)
        {
            DisableList(allPanels[i]);
        }
    }

    //Determine our max amount of pages when the category is selected.
    //This almost works, but what if we have too many leftover?
    public void SetMaxPages()
    {
        int totalItems = display.ReturnActiveCount(theCloset.currentCategory);
        //Great.  So now we know how many items we have.  Say we have 10 for example:
        int pages = totalItems / 9; //That's how many pages we can fit.
        int leftover = totalItems % 9;  //That's if we have any leftover.

        maxPageCount = pages;
        if (leftover > 0)
        {
            maxPageCount++;
        }
    }

    public void TurnOffPagination()
    {
        rightPage.SetActive(false);
        leftPage.SetActive(false);
//        currentPage = 0;  Let's leave this out to test so we can return to the previous page.
        currentItemCount = 9;
    }

    //Runs a check to see if we need to turn the pagination buttons on.
    public void CheckForPagination()
    {
        SetMaxPages();
        //We should set up the initial page display.
        for (int i = 0; i < allPanels.Count; i++)
        {
            if (i != currentPage)
            {
                DisableList(allPanels[i]);
            }
            else
            {
                EnableList(allPanels[i]);
            }
        }
        if (display.ReturnActiveCount(theCloset.currentCategory) > currentItemCount)
        {
            //We have more active items than we are curently tracking in this list, so enable pagination to the right.
            rightPage.SetActive(true);
            leftPage.SetActive(true);
        }
    }

    //We skip forward in the index.
    public void ChangePageRight()
    {
        currentPage++;

        //We are on the last page.  Reset to move to the first page.
        if (currentPage >= maxPageCount)
        {
            currentPage = 0;
        }
        currentItemCount = (currentPage * 9) + 9;


        for (int i =0; i < maxPageCount; i++)
        {
            if (i != currentPage)
            {
                DisableList(allPanels[i]);
            }
            else
            {
                EnableList(allPanels[i]);
            }
        }

        CheckForPagination();
    }

    public void ChangePageLeft()
    {
        currentPage--;

        //We are on the first page.
        if (currentPage < 0)
        {
            currentPage = maxPageCount - 1;
        }
        currentItemCount = (currentPage * 9) + 9;


        for (int i = 0; i < maxPageCount; i++)
        {
            if (i != currentPage)
            {
                DisableList(allPanels[i]);
            }
            else
            {
                EnableList(allPanels[i]);
            }
        }

        CheckForPagination();
    }

    //Toggles on all elements in that list.
    public void EnableList(List<Image> list)
    {
        foreach(Image obj in list)
        {
            //If the image has a sprite, let's turn it on.
            if (obj.sprite != null)
            {
                obj.enabled = true;
            }
        }
    }

    //Toggles off each element in a list.
    public void DisableList(List<Image> list)
    {
        foreach(Image obj in list)
        {
            obj.enabled = false;
        }
    }
}
