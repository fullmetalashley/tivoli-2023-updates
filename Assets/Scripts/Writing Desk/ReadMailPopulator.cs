using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Controls the loading of the initial mail stack when the player first lands on the writing desk.
public class ReadMailPopulator : MonoBehaviour
{
    //SCRIPT REFS-----------------------------------
    private MailDatabase mailDatabase;
    private DataManager playerData;
    private MailController mailController;

    //UI REFS-----------------------------------
    public GameObject mailLists;
    public List<GameObject> letterPanels;

    //Indexing REFS-----------------------------------
    public int totalMail;
    public int currentIndex;

    //Pagination REFS-----------------------------------
    public int visibleLetterValue;
    public GameObject pageLeft;
    public GameObject pageRight;

    public int currentPage;
    public int maxPages;

    public bool initialized;

    // Start is called before the first frame update
    void Start()
    {
        mailDatabase = FindObjectOfType<MailDatabase>();
        playerData = FindObjectOfType<DataManager>();
        mailController = FindObjectOfType<MailController>();
    }


    //A dummy initializer so that when Start doesn't run first, we can do it here.
    public void Initialize()
    {
        totalMail = mailDatabase.readMail.Count;    //How much mail do we need to skin?

        //Max pages: How much mail do we have, divided by our page max?
        maxPages = totalMail / visibleLetterValue;
        if (totalMail % visibleLetterValue > 1)
        {
            maxPages++; //If we have a little leftover, add an extra page.
        }

        CheckPagination();
        SkinList();

    }

    //Set up the mail list.
    public void ToggleDisplay()
    {
        if (!initialized)
        {
            initialized = true;
            Initialize();
        }
        //Turn the game object on and off.
        mailLists.SetActive(!mailLists.activeSelf);


        SkinList();
    }

    //Run through the entire list and show all of it.
    public void SkinList()
    {
        //For each letter panel, check to see if a letter exists for this, then skin it.
        for (int i = 0; i < letterPanels.Count; i++)
        {
            //OUR ACTUAL CURRENT INDEX: 
            currentIndex = i + (currentPage * visibleLetterValue);
            if (i < totalMail)
            {
                letterPanels[i].SetActive(true);
                SkinPanel(i, currentIndex);
            }
            else
            {
                letterPanels[i].SetActive(false);
            }
        }
    }

    //When the player clicks a letter, set that letter up.
    public void ShowReadLetter(int index)
    {
        mailController.SkinLetterBase(letterPanels[index].GetComponent<ReadLetterPanel>().letterIndex);
    }

    //Use the current value to skin the panel.
    public void SkinPanel(int panel, int letterIndex)
    {
        //Actually: is this the last letter in the panel?  That's the only one with a body.
        if (panel % visibleLetterValue == 0)
        {
            letterPanels[panel].GetComponent<ReadLetterPanel>().body.text = mailDatabase.readMail[letterIndex].content;
            letterPanels[panel].GetComponent<ReadLetterPanel>().body.enabled = true;
        }
        else
        {
            letterPanels[panel].GetComponent<ReadLetterPanel>().body.enabled = false;
        }
        letterPanels[panel].GetComponent<ReadLetterPanel>().letterIndex = letterIndex;

        if (mailDatabase.readMail[panel].letterType == "CHAR")
        {
            letterPanels[panel].GetComponent<ReadLetterPanel>().sender.enabled = true;
            letterPanels[panel].GetComponent<ReadLetterPanel>().dateSent.enabled = true;

            letterPanels[panel].GetComponent<ReadLetterPanel>().sender.text = mailDatabase.readMail[letterIndex].sender;
            letterPanels[panel].GetComponent<ReadLetterPanel>().dateSent.text = mailDatabase.readMail[letterIndex].sentDate;

            letterPanels[panel].GetComponent<ReadLetterPanel>().header.enabled = false;
        }
        else
        {
            //A regency letter.
            letterPanels[panel].GetComponent<ReadLetterPanel>().header.enabled = true;

            letterPanels[panel].GetComponent<ReadLetterPanel>().header.text = mailDatabase.readMail[letterIndex].header;

            letterPanels[panel].GetComponent<ReadLetterPanel>().sender.enabled = false;
            letterPanels[panel].GetComponent<ReadLetterPanel>().dateSent.enabled = false;
        }
    }

    //Do we even need to paginate?  Are there enough letters?
    public void CheckPagination()
    {
        //We have more letters than we have panels for.
        if (totalMail > visibleLetterValue)
        {
            pageLeft.SetActive(true);
            pageRight.SetActive(true);
        }
        else
        {
            pageLeft.SetActive(false);
            pageRight.SetActive(false);
        }
    }

    //Move to the next page.
    public void PageRight()
    {
        currentPage++;
        if (currentPage >= maxPages)
        {
            currentPage = 0;
        }
        SkinList();
    }

    //Move to the previous page.
    public void PageLeft()
    {
        currentPage--;
        if (currentPage < 0)
        {
            currentPage = maxPages;
        }
        SkinList();
    }
}
