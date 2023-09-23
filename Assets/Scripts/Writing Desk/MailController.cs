using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//SCRIPT PURPOSE: Controls the UI of the mail area.
public class MailController : MonoBehaviour
{
    //SCRIPT REFS-----------------------------------
    private MailDatabase mailLists;
    private DataManager playerData;
    private InitialMailDisplay initialLetters;
    private ReadMailPopulator readMailStack;

    //UI Groups for each template
    [Header("Notification Settings")]
    public GameObject notif;
    public Text n_header;
    public Text n_description;
    public Image n_icon;
    public GameObject n_next;

    [Header("Novelogue Settings")]
    public GameObject novelogue;
    public Text novel_header;
    public Text novel_chapdescription;
    public Text novel_readTime;
    public Text novel_bookDate;
    public Text novel_buttonText;
    public GameObject novel_next;

    [Header("Item Drop Settings")]
    public GameObject delivery;
    public Text i_header;
    public Text i_subheader;
    public Image i_image;
    public Text i_description;
    public Text i_location;
    public GameObject i_next;
    public GameObject i_loadScene;

    //Values that will determine where we move to if a delivery button is pressed.
    private string sceneToLoad;
    private string garmentCategory;
    public int chapterToLoad;
    public int sectionToLoad;

    [Header("Letter Settings")]
    public GameObject letter;
    public Text sender; //Used for character letters
    public Text date;   //Used for character letters
    public Text body;
    public Text header; //Used for regency letters
    public Text regencyBody;  //Used for regency letters
    public GameObject l_next;

    [Header("READ Letter Settings")]
    public GameObject readLetter;
    public Text readSender; //Used for character letters
    public Text readDate;   //Used for character letters
    public Text readBody;
    public Text readHeader; //Used for regency letters
    public Text readRegencyBody;


    // Start is called before the first frame update
    void Start()
    {
        mailLists = FindObjectOfType<MailDatabase>();
        playerData = FindObjectOfType<DataManager>();
        initialLetters = GetComponent<InitialMailDisplay>();
        readMailStack = GetComponent<ReadMailPopulator>();
    }

    //Called from the UI, turns off the current open value.
    //Since it is called from the BUFFER, it will turn everything off.
    public void ToggleInitialLetter(string code)
    {
        initialLetters.currentIndex = 0;    //Reset our current index.
        switch (code)
        {
            case "Letter":
                //Turn on the letter game object.
                initialLetters.letterIndex = initialLetters.lettersToRead;
                letter.SetActive(false);
                //Now, we need to check for notifications.
                CheckDocuments();
                break;
            case "Notification":
                //Turn on the notification.
                initialLetters.notifIndex = initialLetters.notifsToRead;
                notif.SetActive(false);
                CheckDocuments();
                break;
            case "Novelogue":
                //Turn on the novelogue.
                initialLetters.novelIndex = initialLetters.novelsToRead;
                novelogue.SetActive(false);
                CheckDocuments();
                break;
            case "Item Drop":
                //Turn on the delivery.
                initialLetters.itemIndex = initialLetters.itemsToRead;
                playerData.categoryToOpen = ""; //We can reset this back to 0 because there isn't anything left to open.
                delivery.SetActive(false);
                break;
            default:
                break;
        }
    }

    //Called from the initial mail display.
    //Turns on the letter stack if we have something to read.
    public void ToggleInitialLetter(int index, string code, bool value)
    {
        //We are going to start at the passed index, along with a code that tells us which template to set up.
        switch (code)
        {
            case "Letter":
                //Turn on the letter game object.
                letter.SetActive(value);
                if (value)
                {
                    SkinLetter(index);
                }
                break;
            case "Notification":
                //Turn on the notification.
                notif.SetActive(value);

                if (value)
                {
                    SkinNotification(index);
                }
                break;
            case "Novelogue":
                //Turn on the novelogue.
                novelogue.SetActive(value);

                if (value)
                {
                    SkinNovelogue(index);
                }
                break;
            case "Item Drop":
                //Turn on the delivery.
                delivery.SetActive(value);

                if (value)
                {
                    SkinDelivery(index);
                }
                break;
            default:
                break;
        }
    }

    //Set up the letter display.
    public void SkinLetter(int index)
    {
        //We use the letter index to set the UI elements to the right letter values.
        //First, let's get this letter's data as a reference.
        ActiveLetter letter = mailLists.unreadMail[index];

        if (letter.letterType == "CHAR")
        {
            //Character letter, skin appropriately.
            regencyBody.enabled = false;
            header.enabled = false;

            sender.enabled = true;
            date.enabled = true;

            sender.text = letter.sender;
            date.text = letter.sentDate;
            body.enabled = true;

            body.text = letter.content; 
        }
        else
        {
            sender.enabled = false;
            date.enabled = false;

            header.enabled = true;

            //Regency letter, skin appropriately.  Certain things get turned off.
            header.text = letter.header;
            regencyBody.enabled = true;

            regencyBody.text = letter.content;
            body.enabled = false;
        }

        //Update data now that the letter has been read.
        initialLetters.LetterRead(letter);

        //Are there letters remaining?  If so, keep the next button on.
        l_next.SetActive(initialLetters.LettersRemaining());

    }

    //Set up the notification display.
    public void SkinNotification(int index)
    {
        ActiveNotification notif = mailLists.activeNotifications[index];

        n_header.text = notif.header;
        n_description.text = notif.description;

        if (notif.image != null)
        {
            n_icon.enabled = true;
            n_icon.sprite = notif.image;
        }
        else
        {
            n_icon.enabled = false;
        }

        initialLetters.NotifRead(notif);

        n_next.SetActive(initialLetters.NotifsRemaining());
    }

    //Set up the novelogue display.
    public void SkinNovelogue(int index)
    {
        ActiveNovelogue novel = mailLists.activeNovelogues[index];

        novel_header.text = novel.header;
        novel_chapdescription.text = novel.chapterDescription;
        novel_readTime.text = novel.readTime + " minute read time";
        novel_bookDate.text = novel.publishDate;

        string section = " Section " + novel.section;
        if (novel.section == 0)
        {
            section = "";
        }

        novel_buttonText.text = "Read Chapter " + novel.chapterNumber + section;

        //We need to actually set this differently, because this might not be the right chapter index.  We need the actual list index.
        chapterToLoad = novel.chapterIndex;
        sectionToLoad = novel.section;


        initialLetters.NovelogueRead(novel);

        novel_next.SetActive(initialLetters.NovelsRemaining());

    }

    //Set up the item drop skins.
    public void SkinDelivery(int index)
    {
        ActiveItemDrop drop = mailLists.activeItemDrops[index];

        i_header.text = drop.header;
        i_subheader.text = drop.subheader;

        i_image.sprite = drop.icon;
        i_location.text = drop.ctaText;

        if (drop.location == "Gallery")
        {
            //Set description to painting unlocked.
            i_description.text = "Enjoy this painting in your gallery!";
        }
        else
        {
            i_description.text = "You’ve received " + drop.itemName + " from " + drop.sender + ".  Would you like to try it on Elizabeth and Jane now?";
        }

        i_loadScene.SetActive(true);
        i_loadScene.GetComponent<SceneLoader>().SceneToLoad = drop.location;

        garmentCategory = drop.category;

        initialLetters.ItemDropRead(drop);

        i_next.SetActive(initialLetters.DeliveriesRemaining());
    }


    //Checks to see if we are initiating the next set of documents.
    //This will always start with letters, so we don't need to check that first.  Just need to start at notifications.
    public void CheckDocuments()
    {
        if (initialLetters.NotifsRemaining())
        {
            //We still have more to do.
            ToggleInitialLetter(initialLetters.currentIndex, "Notification", true);
            return;
        }
        if (initialLetters.NovelsRemaining())
        {
            ToggleInitialLetter(initialLetters.currentIndex, "Novelogue", true);
            return;
        }
        if (initialLetters.DeliveriesRemaining())
        {
            ToggleInitialLetter(initialLetters.currentIndex, "Item Drop", true);
            return;
        }
    }

    //SCENE CHANGES BASED ON BUTTON CLICKS-----------------------------------

    //Called when the player selects a button on an item drop specifically.
    //The current item drop will have the category name attached to it.
    public void MoveToCloset()
    {
        playerData.categoryToOpen = garmentCategory;
    }

    //Called when the player selects the reader from a novelogue.
    public void MoveToReader()
    {
        playerData.novelogueTransfer = true;
        for (int i = 0; i < FindObjectOfType<PnPDatabase>().chapters.Count; i++)
        {
            if (FindObjectOfType<PnPDatabase>().chapters[i].section == sectionToLoad && FindObjectOfType<PnPDatabase>().chapters[i].chapterNumber == chapterToLoad + 1)
            {
                //This is the right index, use this.
                playerData.listIndex = i;
            }
        }
    }

    //BASE Skins, no data change-----------------------------------
    //Set up the letter display.
    public void SkinLetterBase(int index)
    {
        //We use the letter index to set the UI elements to the right letter values.
        //First, let's get this letter's data as a reference.
        ActiveLetter letter = mailLists.readMail[index];

        if (letter.letterType == "CHAR")
        {
            //Character letter, skin appropriately.
            //Turn things on first.
            readHeader.enabled = false;
            readRegencyBody.enabled = false;

            readSender.enabled = true;
            readDate.enabled = true;

            readBody.enabled = true;

            readSender.text = letter.sender;
            readDate.text = letter.sentDate;
            readBody.text = letter.content;
        }
        else
        {
            //Regency letter, skin appropriately
            readHeader.enabled = true;
            readRegencyBody.enabled = true;

            readSender.enabled = false;
            readDate.enabled = false;

            readBody.enabled = false;

            readHeader.text = letter.header;
            readRegencyBody.text = letter.content;
        }
        readLetter.SetActive(true);
    }

    //Turn off the letter from the buffer.
    public void LetterBaseOff()
    {
        readLetter.SetActive(false);
    }
}
