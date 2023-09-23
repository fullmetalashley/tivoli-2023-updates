using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MailDatabase : MonoBehaviour
{
    public static MailDatabase control;

    public List<ActiveLetter> unreadMail;
    public List<ActiveLetter> readMail;
    public List<InactiveLetter> inactiveMail;

    public List<ActiveNotification> activeNotifications;
    public List<InactiveNotification> inactiveNotifications;

    public List<ActiveNovelogue> activeNovelogues;
    public List<InactiveNovelogue> inactiveNovelogues;

    public List<ActiveItemDrop> activeItemDrops;
    public List<InactiveItemDrop> inactiveItemDrops;

    private DataManager playerData;


    public bool readEstablished;


    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void InitializeReadMail()
    {
        playerData = FindObjectOfType<DataManager>();

        //First, letters.
        readMail = new List<ActiveLetter>();
        List<ActiveLetter> tempList = new List<ActiveLetter>();
        if (playerData.unreadLettersProcessed <= unreadMail.Count)
        {
            for (int i = 0; i < playerData.unreadLettersProcessed; i++)
            {
                readMail.Add(unreadMail[i]);
                tempList.Add(unreadMail[i]);
            }
            for (int j = 0; j < tempList.Count; j++)
            {
                unreadMail.Remove(tempList[j]);
            }
        }

        //Next, notifications.
        List<ActiveNotification> tempNot = new List<ActiveNotification>();
        for (int i =0; i < playerData.notifProcessed; i++)
        {
            //Remove this from the active list.
            tempNot.Add(activeNotifications[i]);
        }
        for (int i =0; i < tempNot.Count; i++)
        {
            activeNotifications.Remove(tempNot[i]);
        }

        //Next, item drops.
        List<ActiveItemDrop> tempItem = new List<ActiveItemDrop>();
        for (int i = 0; i < playerData.itemDropProcessed; i++)
        {
            //Remove this from the active list.
            tempItem.Add(activeItemDrops[i]);
        }
        for (int i = 0; i < tempItem.Count; i++)
        {
            activeItemDrops.Remove(tempItem[i]);
        }

        //Next, novelogues.
        List<ActiveNovelogue> tempNov = new List<ActiveNovelogue>();
        for (int i = 0; i < playerData.novelogueProcessed; i++)
        {
            //Remove this from the active list.
            tempNov.Add(activeNovelogues[i]);
        }
        for (int i = 0; i < tempNov.Count; i++)
        {
            activeNovelogues.Remove(tempNov[i]);
        }
    }
    
    //Show each letter in the list, starting at index 0.  Once the button is pressed (i.e. letter is read), mark that letter as read and
    //move it to the read mail list.  Might not even need the bool eventually.
    public void MoveUnreadLetters()
    {
        Debug.Log("Moving mail!");
        for (int i = 0; i < readMail.Count; i++)
        {
            unreadMail.Remove(readMail[i]);
        }
    }

    //Runs an update on the story map.  
    public void UpdateLists(DateTime currentTime)
    {
        //Runs an update for the letters.
        List<InactiveLetter> toBeRemoved = new List<InactiveLetter>();
        foreach (InactiveLetter theLetter in inactiveMail)
        {
            if ((theLetter.dateActive - currentTime).TotalDays < 0 || (theLetter.dateActive - currentTime).TotalDays == 0)
            {

                toBeRemoved.Add(theLetter);
                if (theLetter.hasItem)
                {
                    unreadMail.Add(new ActiveLetter(theLetter.sender, theLetter.sentDate, theLetter.content, true, theLetter.deliveryNames, theLetter.dateActive, theLetter.deliveryIcons, theLetter.letterType));

                }
                else
                {
                    unreadMail.Add(new ActiveLetter(theLetter.sender, theLetter.sentDate, theLetter.content, theLetter.header, theLetter.letterType));
                }
            }
        }
        foreach (InactiveLetter inactiveLets in toBeRemoved)
        {
            inactiveMail.Remove(inactiveLets);
        }

        //Run an update for notifications.
        List<InactiveNotification> removeNotif = new List<InactiveNotification>();
        foreach (InactiveNotification notif in inactiveNotifications)
        {
            if ((notif.activeDate - currentTime).TotalDays < 0 || (notif.activeDate - currentTime).TotalDays == 0)
            {
                removeNotif.Add(notif);
                activeNotifications.Add(new ActiveNotification(notif.header, notif.description, notif.imageRef, notif.linkRef));
            }
        }
        foreach(InactiveNotification inactives in removeNotif)
        {
            inactiveNotifications.Remove(inactives);
        }


        //Run an update for novelogues;
        List<InactiveNovelogue> removeNovel = new List<InactiveNovelogue>();
        foreach (InactiveNovelogue novel in inactiveNovelogues)
        {
            if ((novel.availableDate - currentTime).TotalDays < 0 || (novel.availableDate - currentTime).TotalDays == 0)
            {
                removeNovel.Add(novel);
                activeNovelogues.Add(new ActiveNovelogue(novel));
            }
        }
        foreach (InactiveNovelogue inactives in removeNovel)
        {
            inactiveNovelogues.Remove(inactives);
        }

        //Run an update for item drops.
        List<InactiveItemDrop> removeDrops = new List<InactiveItemDrop>();
        foreach (InactiveItemDrop drop in inactiveItemDrops)
        {
            if ((drop.availableDate - currentTime).TotalDays < 0 || (drop.availableDate - currentTime).TotalDays == 0)
            {
                removeDrops.Add(drop);
                //string signifier, string name, string room, string category, string header, string subheader, string sender, string cta, Sprite icon
                activeItemDrops.Add(new ActiveItemDrop(drop.signifier, drop.itemName, drop.location, drop.category, drop.header, drop.subheader, drop.sender, drop.ctaText, drop.icon));
            }
        }
        foreach(InactiveItemDrop inactives in removeDrops)
        {
            inactiveItemDrops.Remove(inactives);
        }
    }

    //Returns what is currently new to read.
    public int MailToRead()
    {
        return unreadMail.Count + activeNovelogues.Count + activeNotifications.Count + activeItemDrops.Count + readMail.Count;
    }
}
