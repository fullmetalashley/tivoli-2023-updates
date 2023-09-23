using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Controls the loading of the initial mail stack when the player first lands on the writing desk.
public class InitialMailDisplay : MonoBehaviour
{
    //SCRIPT REFS-----------------------------------
    private MailDatabase mailDatabase;
    private DataManager playerData;
    private MailController mailUI;

    //Letter / deliverable counts to track where they've left off
    public int totalCount = 0;

    public int letterIndex = 0;
    public int notifIndex = 0;
    public int novelIndex = 0;
    public int itemIndex = 0;

    public int lettersToRead;
    public int notifsToRead;
    public int novelsToRead;
    public int itemsToRead;

    //Where we're currently at in the list.
    public int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        mailDatabase = FindObjectOfType<MailDatabase>();
        playerData = FindObjectOfType<DataManager>();
        mailUI = GetComponent<MailController>();

        //How many unread items exist to be read?
        //Notifications + item drops + novelogues + mail
        totalCount += mailDatabase.activeNotifications.Count + mailDatabase.activeItemDrops.Count + mailDatabase.activeNovelogues.Count + mailDatabase.unreadMail.Count;

        lettersToRead = mailDatabase.unreadMail.Count;
        notifsToRead = mailDatabase.activeNotifications.Count;
        novelsToRead = mailDatabase.activeNovelogues.Count;
        itemsToRead = mailDatabase.activeItemDrops.Count;

        //Now that save values are loaded, begin processing the stack.

        if (ContentToBeRead())
        {
            //We need to figure out what needs to be read first.

            if (LettersRemaining()){
                mailUI.ToggleInitialLetter(currentIndex, "Letter", true);
            }
            else if (NotifsRemaining())
            {
                mailUI.ToggleInitialLetter(currentIndex, "Notification", true);
            }
            else if (NovelsRemaining())
            {
                mailUI.ToggleInitialLetter(currentIndex, "Novelogue", true);
            }
            else if (DeliveriesRemaining())
            {
                mailUI.ToggleInitialLetter(currentIndex, "Item Drop", true);
            }
        }
    }

    //PROCESSING ORDER:
    //Letters, Notifs, Novelogues, Item Drops.


    //LETTER CHECKS------------------------------------------------------------
    //The letter has been read, so we can update that this has happened.
    public void LetterRead(ActiveLetter letter)
    {
        //The letter has been skinned, so now we can report to the letter display that this letter has been updated.
        mailDatabase.readMail.Add(letter);
        mailDatabase.unreadMail.Remove(letter);
        playerData.unreadLettersProcessed++;

        letterIndex++;
        currentIndex++;  //Move to the next letter.
    }

    //Called when we hit the NEXT button.
    public void NextLetter()
    {
        //If our tracking index is less than the letters to read, we need to move to the next category.
        if (currentIndex < lettersToRead)
        {
            //We have more letters to read.
            mailUI.SkinLetter(0);
        }
    }

    //NOTIF CHECKS------------------------------------------------------------
    //The notification has been read, so we update.
    public void NotifRead(ActiveNotification notif)
    {
        mailDatabase.activeNotifications.Remove(notif);
        playerData.notifProcessed++;

        notifIndex++;
        currentIndex++; //Move to the next notification.
    }

    //Called when we hit the NEXT button.
    public void NextNotif()
    {
        if (currentIndex < notifsToRead + lettersToRead)
        {
            //We have more letters to read.
            mailUI.SkinNotification(0);
        }
    }

    //NOVELOGUE CHECKS------------------------------------------------------------
    //We read an item drop, so we process it.
    public void NovelogueRead(ActiveNovelogue novel)
    {
        mailDatabase.activeNovelogues.Remove(novel);
        playerData.novelogueProcessed++;

        novelIndex++;
        currentIndex++;
    }

    //Move to the next novelogue.
    public void NextNovelogue()
    {
        if (currentIndex < novelsToRead + notifsToRead + lettersToRead)
        {
            mailUI.SkinNovelogue(0);
        }
    }

    //ITEM DROP CHECKS------------------------------------------------------------
    //We read an item drop, so we process it.
    public void ItemDropRead(ActiveItemDrop drop)
    {
        mailDatabase.activeItemDrops.Remove(drop);
        playerData.itemDropProcessed++;

        itemIndex++;
        currentIndex++;
    }

    //Move to the next item drop.
    public void NextItemDrop()
    {
        if (currentIndex < itemsToRead + novelsToRead + notifsToRead + lettersToRead)
        {
            mailUI.SkinDelivery(0);
        }
    }





    //BOOLEAN CHECKS TO SEE IF WE HAVE CONTENT REMAINING

    //Are there any LETTERS specifically?
    public bool LettersRemaining()
    {
        if (letterIndex < lettersToRead)
        {
            //Still more letters to go.
            return true;
        }
        return false;
    }

    //Do we have any notifications in the list?
    public bool NotifsRemaining()
    {
        if (notifIndex < notifsToRead)
        {
            return true;
        }
        return false;
    }

    //Do we have any item drops in the list?
    public bool DeliveriesRemaining()
    {
        if (itemIndex < itemsToRead)
        {
            return true;
        }
        return false;
    }

    //Do we have any novelogues in the list?
    public bool NovelsRemaining()
    {
        if (novelIndex < novelsToRead)
        {
            return true;
        }
        return false;
    }

    //Are there any things left to read?
    public bool ContentToBeRead()
    {
        if (currentIndex < totalCount)
        {
            return true;
        }
        return false;
    }

}
