using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryMapUII : MonoBehaviour
{
    //These are the larger modal containers that will be toggled on and off accordingly.
    public GameObject writingDeskModal;
    public GameObject bookshelfModal;

    //The main menu button is disabled if a modal is on.
    public GameObject mainMenuButton;

    //References to necessary scripts.
    private MailController theMail;
    private MailDatabase mailLists;
    private DataManager gameData;


    // Start is called before the first frame update
    void Start()
    {
        theMail = FindObjectOfType<MailController>();
        mailLists = FindObjectOfType<MailDatabase>();
        gameData = FindObjectOfType<DataManager>();
    }

    public void ToggleWritingDesk()
    {
        writingDeskModal.SetActive(!writingDeskModal.activeSelf);
        if (writingDeskModal.activeSelf)
        {
            //If the writing desk is turned on...
            if (mailLists.unreadMail.Count > 0)
            {
                Debug.Log("We have letters to read!");
                //We still have unread mail to show, so the letter should be closed.
            }
            else
            {
                Debug.Log("We have no letters to read.");
                theMail.letter.SetActive(false);
            }
        }
        else
        {
            //Desk is off.
            mainMenuButton.SetActive(true);
            mailLists.MoveUnreadLetters();
        }
    }
    

    public void ToggleBookshelf()
    {
        bookshelfModal.SetActive(!bookshelfModal.activeSelf);
        if (bookshelfModal.activeSelf)
        {

            mainMenuButton.SetActive(false);
        }
        else
        {
            mainMenuButton.SetActive(true);
        }
    }
}
