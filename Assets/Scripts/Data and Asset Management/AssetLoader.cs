using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

//Script purpose: Load all of the assets for the game based on what act we are in.
public class AssetLoader : MonoBehaviour
{

    //A string will determine what Act we are in.
    //From there, we will have references built to each folder location.
    //Only populate a folder location in the list if that folder has content.
    //NOTE: empty folders should not be in the resources folder, so any folder that exists should work.

        [Header("Loading locations")]
        //The primary loading folder.
    public string actToLoad;

    //References to the clothing locations.
    public string activeClothingLocation;
    public string inactiveClothingLocation;

    //Reference to the saved player tableaus.
    public string savedTableausLocation;

    //References to the tableau location.
    public string activeTableauLocation;
    public string inactiveTableauLocation;

    //References to the jewelry location.
    public string activeJewelryLocation;
    public string inactiveJewelryLocation;

    //References to the letter location.
    public string activeRegencyLocation;
    public string inactiveRegencyLocation;

    //References to the notification location.
    public string activeNotificationLocation;
    public string inactiveNotificationLocation;

    //References to the item drop location.
    public string activeItemDropLocation;
    public string inactiveItemDropLocation;

    //References to the novelogue location.
    public string activeNovelogueLocation;
    public string inactiveNovelogueLocation;

    //References to PnP content.
    public string chapterLocation;
    public string sectionHeaderLocation;

    [Header("Text asset names")]

    //Stored lists of the specific letter names.
    public string[] activeRegencyNames;
    public string[] inactiveRegencyNames;

    //Stored lists of the specific novelogue names.
    public string[] activeNovelogueNames;
    public string[] inactiveNovelogueNames;

    //Stored lists of the specific notification names.
    public string[] activeNotificationNames;
    public string[] inactiveNotificationNames;

    //Stored lists of the specific item drop names.
    public string[] activeItemDropNames;
    public string[] inactiveItemDropNames;

    [Header("Garment locations")]

    //THE FOLLOWING LISTS ARE USED TO ESTABLISH DATABASES.
    //Clothing elements that need to be loaded.
    public List<string> activeClothingElementsToLoad;
    public List<string> inactiveClothingElementsToLoad;

    public List<string> activeJewelryElementsToLoad;
    public List<string> inactiveJewelryElementsToLoad;

    [Header("Advent locations")]
    public List<TextAsset> adventDropNames;
    public List<Sprite> adventImages;

    [Header("Act / date values")]

    public List<string> act1Dates;
    public List<string> act2Dates;
    public List<string> act3Dates;
    public List<string> act4Dates;
    public List<string> act5Dates;

    public List<DateTime> act1Times;
    public List<DateTime> act2Times;
    public List<DateTime> act3Times;
    public List<DateTime> act4Times;
    public List<DateTime> act5Times;
        
    public List<string> inactiveDropDates;

    [Header("Database lists")]

    public List<ActiveDatabaseList> activeClothing;
    public List<InactiveDatabaseList> inactiveClothing;

    public List<ActiveTableauList> activeTableaus;
    public List<InactiveTableauList> inactiveTableaus;

    public List<ActiveLetter> activeRegency;
    public List<InactiveLetter> inactiveRegency;

    public List<ActiveNotification> activeNotifications;
    public List<InactiveNotification> inactiveNotifications;

    public List<ActiveItemDrop> activeItemDrops;
    public List<InactiveItemDrop> inactiveItemDrops;

    public List<ActiveNovelogue> activeNovelogues;
    public List<InactiveNovelogue> inactiveNovelogues;

    public List<ActiveDatabaseList> activeJewelry;
    public List<InactiveDatabaseList> inactiveJewelry;

    public List<Chapter> chapters;

    public List<Advent> advents;

    [Header("Display asset locations")]

    //These references establish the display icon database.
    public string displayLocation;
    public DisplayItemList displayItems;

    //This is a temporary list used to help establish the display icon list.
    public List<Sprite> tempSprites;

    [Header("PnP Information")]
    public List<TextAsset> volume1TextAssets;

    //References to the text assets for volume 1 of PNP.
    private Dictionary<int, List<List<string>>> pnpText;


    //Called by the game initializer.
    //This creates each of the lists and eventually sends them to their respective databases.
    public void InitializeLoad()
    {
        //Lists are all initialized.
        activeClothing = new List<ActiveDatabaseList>();
        inactiveClothing = new List<InactiveDatabaseList>();

        activeTableaus = new List<ActiveTableauList>();
        inactiveTableaus = new List<InactiveTableauList>();

        activeRegency = new List<ActiveLetter>();
        inactiveRegency = new List<InactiveLetter>();

        activeNovelogues = new List<ActiveNovelogue>();
        inactiveNovelogues = new List<InactiveNovelogue>();

        activeNotifications = new List<ActiveNotification>();
        inactiveNotifications = new List<InactiveNotification>();

        activeItemDrops = new List<ActiveItemDrop>();
        inactiveItemDrops = new List<InactiveItemDrop>();


        activeJewelry = new List<ActiveDatabaseList>();
        inactiveJewelry = new List<InactiveDatabaseList>();

        chapters = new List<Chapter>();

        advents = new List<Advent>();

        pnpText = new Dictionary<int, List<List<string>>>();

        act1Times = new List<DateTime>();
        act2Times = new List<DateTime>();
        act3Times = new List<DateTime>();
        act4Times = new List<DateTime>();
        act5Times = new List<DateTime>();

        LoadActiveClothing();
        LoadInactiveClothing();

        LoadActiveJewelry();
        LoadInactiveJewelry();

        LoadActiveTableaus();
        LoadInactiveTableaus();

        LoadDisplayIcons();

        LoadPnPText();

        LoadRegency();
        LoadNotifications();
        LoadNovelogues();
        LoadItemDelivery();

        LoadAdvents();

        savedTableausLocation = Application.persistentDataPath;

        tempSprites = new List<Sprite>();
        LoadSavedTableaus();

        EstablishDates(act1Dates, act1Times);
        EstablishDates(act2Dates, act2Times);
        EstablishDates(act3Dates, act3Times);
        EstablishDates(act4Dates, act4Times);
        EstablishDates(act5Dates, act5Times);

        EstablishDeliveryIcons();


        //All established lists are sent to their respective database scripts.
        TransferData();
    }

    public void LoadPnPText()
    {       
        //Great!  These are all of our chapters, in text asset format.  So now let's populate them into a list of chapters.
        for (int i = 0; i < volume1TextAssets.Count; i++)
        {
            //Index 0: Chapter (int)
            //Index 1: Section (int)
            //Index 2: Read time (float)
            //Index 3: Body (string)
            //Index 4: Pull quote (string)
            //Index 5: Summary (string)

            //The chapter is broken apart by "$".
            string fullChap = volume1TextAssets[i].text;
            string[] splitChap = fullChap.Split('$');  //Splits the chapter into an array of strings

            // int chap, string syn, string pull, string body, float read, int section
            Chapter newChap = new Chapter(int.Parse(splitChap[0]), int.Parse(splitChap[1]), float.Parse(splitChap[2]), splitChap[3], splitChap[4], splitChap[5]);
            chapters.Add(newChap);
        }
    }

    //Load all of the advents into new advent containers.
    public void LoadAdvents()
    {
        //We have a list of all of the sprites and their location.

        //A list of each file to go through.  First we need to load this file.
        for (int i = 0; i < adventDropNames.Count; i++)
        {
            string advent = adventDropNames[i].text;
            string[] splitAd = advent.Split('$');

            //Index 0: corresponding image name
            //Index 1: Name
            //Index 2: Description

            Advent newAdvent = new Advent(adventImages[i], splitAd[1], splitAd[2]);
            advents.Add(newAdvent);
        }
    }

    public void LoadSavedTableaus()
    {
        string[] fileNames = Directory.GetFiles(savedTableausLocation);
        for (int i = 0; i < fileNames.Length; i++)
        {
            if (fileNames[i].Contains("Tableau"))
            {
                //We have found a tableau.
                Texture2D Tex2D; //Turn it into a texture
                byte[] FileData; //We also need to read the data to push it to the texture.

                FileData = File.ReadAllBytes(fileNames[i]);
                Tex2D = new Texture2D(2, 2); //Empty texture
                Tex2D.LoadImage(FileData); //Write the file to the texture

                Sprite NewSprite = Sprite.Create(Tex2D, new Rect(0, 0, Tex2D.width, Tex2D.height), new Vector2(0, 0), 100);
                tempSprites.Add(NewSprite);
            }
        }
    }

    //All display icons are loaded.  This has to happen after clothing / jewelry / tableaus have been established.
    public void LoadDisplayIcons()
    {
        int totalMatches3Q = 0;
        int totalMatchesStraight = 0;
        int mismatches = 0;
        int jewelryMatches3Q = 0;
        int jewelryMatchesStraight = 0;
        int jewelryMismatches = 0;

        Sprite[] tempList = Resources.LoadAll<Sprite>(displayLocation);

        List<string> displayNames = new List<string>();
        for (int i = 0; i < tempList.Length; i++)
        {
            displayNames.Add(tempList[i].name);
        }

        displayItems = new DisplayItemList(displayNames, tempList);

        //DEBUGGING FOR THE REST OF THE METHOD:
        //This is debugging stuff to make sure the matches are all lining up.
        for (int z = 0; z < displayItems.signifier.Count; z++) {
            bool foundMatch = false;
            for (int i = 0; i < activeClothing.Count; i++)
            {
                for (int j = 0; j < activeClothing[i].assetSprites.Count; j++)
                {
                    //We need to break out the signifier from the clothing name.
                    string[] splitString = activeClothing[i].assetSprites[j].name.Split("$"[0]);
                    splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                    string currentSig = splitString[splitString.Length - 1];

                    if (currentSig == displayItems.signifier[z])
                    {
                        foundMatch = true;

                        if (activeClothing[i].pose == "3Q"){
                            totalMatches3Q++;
                            
                        }
                        else if (activeClothing[i].pose == "Straight")
                        {
                            totalMatchesStraight++;
                        }                      
                    }
                }
            }
            if (z + 1 == displayItems.signifier.Count)
            {
                //We are about to move to a new signifier.
                if (!foundMatch)
                {
                    mismatches++;
                }
            }
        }

        //This is purely for debugging to make sure the matches are all lining up.
        for (int p = 0; p < displayItems.signifier.Count; p++)
        {
            bool foundMatch = false;
            for (int m = 0; m < activeJewelry.Count; m++)
            {
                for (int g = 0; g < activeJewelry[m].assetSprites.Count; g++)
                {
                    //We need to break out the signifier from the clothing name.
                    string[] splitString = activeJewelry[m].assetSprites[g].name.Split("$"[0]);
                    splitString[splitString.Length - 1] = splitString[splitString.Length - 1].Trim();
                    string currentSig = splitString[splitString.Length - 1];

                    if (currentSig == displayItems.signifier[p])
                    {
                        foundMatch = true;
                        if (activeJewelry[m].pose == "3Q")
                        {
                            jewelryMatches3Q++;

                        }
                        else if (activeJewelry[m].pose == "Straight")
                        {
                            jewelryMatchesStraight++;
                        }
                    }
                }
            }
            if (p + 1 == displayItems.signifier.Count)
            {
                //We are about to move to a new signifier.
                if (!foundMatch)
                {
                    jewelryMismatches++;
                }
            }
        }
    }

    public void LoadActiveClothing()
    {
        for (int i = 0; i < activeClothingElementsToLoad.Count; i++)
        {
            string[] splitString = activeClothingElementsToLoad[i].Split("-"[0]);

            for (int j = 0; j < splitString.Length; j++)
            {
                splitString[j] = splitString[j].Trim();
            }

            Sprite[] tempList = Resources.LoadAll<Sprite>(actToLoad + "/" + activeClothingLocation + "/" + activeClothingElementsToLoad[i]);


            if (tempList.Length > 0)
            {
                //We shouldn't add a new one unless there are things to load.
                activeClothing.Add(new ActiveDatabaseList(splitString[0], splitString[1]));

                foreach (Sprite spr in tempList)
                {
                    activeClothing[activeClothing.Count - 1].assetSprites.Add(spr);
                }
            }
            else
            {
                bool found = false;
                //First, do we have this list already?
                for (int j = 0; j < activeClothing.Count; j++)
                {
                    if (activeClothing[j].itemClass == splitString[0] && activeClothing[j].pose == splitString[1])
                    {
                        //We already have this, stop.
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    activeClothing.Add(new ActiveDatabaseList(splitString[0], splitString[1]));
                    activeClothing[activeClothing.Count - 1].empty = true;
                }
            }
        }
    }

    public void LoadInactiveClothing()
    {
        for (int i = 0; i < inactiveClothingElementsToLoad.Count; i++)
        {
            Sprite[] tempList = new Sprite[0];

            string[] splitString = inactiveClothingElementsToLoad[i].Split("-"[0]);

            for (int j = 0; j < splitString.Length; j++)
            {
                splitString[j] = splitString[j].Trim();
            }

            for (int k = 0; k < inactiveDropDates.Count; k++)
            {
                string[] splitDate = inactiveDropDates[k].Split("-"[0]);
                int[] newDate = new int[3];
                for (int m = 0; m < splitDate.Length; m++)
                {
                    newDate[m] = int.Parse(splitDate[m]);
                }

                DateTime newTime = new DateTime(newDate[2], newDate[0], newDate[1]);

                tempList = Resources.LoadAll<Sprite>(actToLoad + "/" + inactiveClothingLocation + "/" + inactiveClothingElementsToLoad[i] + "/" + inactiveDropDates[k]);

                if (tempList.Length != 0)
                {
                    inactiveClothing.Add(new InactiveDatabaseList(newTime, splitString[0], splitString[1]));
                    foreach (Sprite spr in tempList)
                    {
                        inactiveClothing[inactiveClothing.Count - 1].assetSprites.Add(spr);
                    }
                }
            }
        }

    }

    public void LoadActiveJewelry()
    {
        for (int i = 0; i < activeJewelryElementsToLoad.Count; i++)
        {
            string[] splitString = activeJewelryElementsToLoad[i].Split("-"[0]);

            for (int j = 0; j < splitString.Length; j++)
            {
                splitString[j] = splitString[j].Trim();
            }
            
            Sprite[] tempList = Resources.LoadAll<Sprite>(actToLoad + "/" + activeJewelryLocation + "/" + activeJewelryElementsToLoad[i]);



            if (tempList.Length > 0)
            {
                //We shouldn't add a new one unless there are things to load.
                activeJewelry.Add(new ActiveDatabaseList(splitString[0], splitString[1]));

                foreach (Sprite spr in tempList)
                {
                    activeJewelry[activeJewelry.Count - 1].assetSprites.Add(spr);
                }
            }
        }
    }

    public void LoadInactiveJewelry()
    {
        for (int i = 0; i < inactiveJewelryElementsToLoad.Count; i++)
        {
            Sprite[] tempList = new Sprite[0];

            string[] splitString = inactiveJewelryElementsToLoad[i].Split("-"[0]);

            for (int j = 0; j < splitString.Length; j++)
            {
                splitString[j] = splitString[j].Trim();
            }

            for (int k = 0; k < inactiveDropDates.Count; k++)
            {
                string[] splitDate = inactiveDropDates[k].Split("-"[0]);
                int[] newDate = new int[3];
                for (int m = 0; m < splitDate.Length; m++)
                {
                    newDate[m] = int.Parse(splitDate[m]);
                }

                DateTime newTime = new DateTime(newDate[2], newDate[0], newDate[1]);

                tempList = Resources.LoadAll<Sprite>(actToLoad + "/" + inactiveJewelryLocation + "/" + inactiveJewelryElementsToLoad[i] + "/" + inactiveDropDates[k]);


                if (tempList.Length != 0)
                {
                    inactiveJewelry.Add(new InactiveDatabaseList(newTime, splitString[0], splitString[1]));
                    foreach (Sprite spr in tempList)
                    {
                        inactiveJewelry[inactiveJewelry.Count - 1].assetSprites.Add(spr);
                    }
                }
            }
        }
    }

    public void EstablishDates(List<string> actToList, List<DateTime> actTimes)
    {
        for (int i = 0; i < actToList.Count; i++)
        {
            String texttt = actToList[i];
            String[] textLines = texttt.Split("-"[0]);

            for (int q = 0; q < textLines.Length; q++)
            {
                textLines[q] = textLines[q].Trim();
            }
            int[] newDate = new int[3];

            for (int p = 0; p < textLines.Length; p++)
            {
                newDate[p] = int.Parse(textLines[p]);
            }

            DateTime newTime = new DateTime(newDate[2], newDate[0], newDate[1]);
            actTimes.Add(newTime);
        }
    }


    //Loads the regency messages.
    public void LoadRegency()
    {
        List<TextAsset> activeLetterList = new List<TextAsset>();
        //Start with the length of the active letters.
        for (int m = 0; m < activeRegencyNames.Length; m++)
        {
            TextAsset tempList = (TextAsset)Resources.Load(actToLoad + "/" + activeRegencyLocation + "/" + activeRegencyNames[m]);
            activeLetterList.Add(tempList);
        }

        //Lines are separated by the "$" symbol.
        //First line (index 0):  Date
        //Second line (index 1): Sender
        //Third line (index 2): Formatted date
        //Fourth line (index 3): Content
        //Fifth line (index 4): Header
        //Sixth line (index 5): Letter type
        for (int i = 0; i < activeLetterList.Count; i++)
        {
            String texttt = activeLetterList[i].text;
            String[] textLines = texttt.Split("$"[0]);

            for (int j = 0; j < textLines.Length; j++)
            {
                textLines[i] = textLines[i].Trim();
            }

            ActiveLetter newLetter = new ActiveLetter(textLines[2], textLines[1], textLines[3], textLines[4], textLines[5]);
            activeRegency.Add(newLetter);
        }

        //--------------------------------------------------------
        //LOAD INACTIVE REGENCY
        List<TextAsset> inactiveLetterList = new List<TextAsset>();
        for (int k = 0; k < inactiveRegencyNames.Length; k++)
        {
            TextAsset tempList = (TextAsset)Resources.Load(actToLoad + "/" + inactiveRegencyLocation + "/" + inactiveRegencyNames[k]);
            inactiveLetterList.Add(tempList);
        }

        //Okay, so now we need to go through that list and create the new InactiveLetters.
        for (int m = 0; m < inactiveLetterList.Count; m++)
        {
            String texttt = inactiveLetterList[m].text;
            String[] textLines = texttt.Split("$"[0]);


            for (int j = 0; j < textLines.Length; j++)
            {
                textLines[j] = textLines[j].Trim();
            }


            string[] splitDate = textLines[0].Split("-"[0]);
            for (int q = 0; q < splitDate.Length; q++)
            {
                splitDate[q] = splitDate[q].Trim();
            }
            int[] newDate = new int[3];

            for (int p = 0; p < splitDate.Length; p++)
            {
                newDate[p] = int.Parse(splitDate[p]);
            }

            DateTime tempTime = new DateTime(newDate[2], newDate[0], newDate[1]);

            //Lines are separated by the "$" symbol.
            //First line (index 0):  Date
            //Second line (index 1): Sender
            //Third line (index 2): Formatted date
            //Fourth line (index 3): Content
            //Fifth line (index 4): Header
            //Sixth line (index 5): Letter type
            InactiveLetter newLetter = new InactiveLetter(tempTime, textLines[2], textLines[1], textLines[3], textLines[4], textLines[5]);
            inactiveRegency.Add(newLetter);
        }
    }

    //Loads notifications specifically.
    public void LoadNotifications()
    {
        List<TextAsset> activeNotifList = new List<TextAsset>();
        //Start with the length of the active letters.
        for (int m = 0; m < activeNotificationNames.Length; m++)
        {
            TextAsset tempList = (TextAsset)Resources.Load(actToLoad + "/" + activeNotificationLocation + "/" + activeNotificationNames[m]);
            activeNotifList.Add(tempList);
        }

        //Lines are separated by the "$" symbol.
        //First line (index 0):  Date
        //Second line (index 1): Header
        //Third line (index 2): Written date as string
        //Fourth line (index 3): Description
        //Fifth line (index 4): Image reference
        //Sixth line (index 5): Link reference
        for (int i = 0; i < activeNotifList.Count; i++)
        {
            String texttt = activeNotifList[i].text;
            String[] textLines = texttt.Split("$"[0]);

            for (int j = 0; j < textLines.Length; j++)
            {
                textLines[i] = textLines[i].Trim();
            }

            string imgRef = textLines[4];
            string linkRef = textLines[5];
            if (imgRef.Contains("NO"))
            {
                imgRef = "";
            }
            if (linkRef.Contains("NO"))
            {
                linkRef = "";
            }

            ActiveNotification newNotif = new ActiveNotification(textLines[1], textLines[3], imgRef, linkRef);
            activeNotifications.Add(newNotif);
        }

        //--------------------------------------------------------
        //LOAD INACTIVE NOTIFICATIONS
        List<TextAsset> inactiveNotifList = new List<TextAsset>();
        for (int k = 0; k < inactiveNotificationNames.Length; k++)
        {
            TextAsset tempList = (TextAsset)Resources.Load(actToLoad + "/" + inactiveNotificationLocation + "/" + inactiveNotificationNames[k]);
            inactiveNotifList.Add(tempList);
        }

        //Okay, so now we need to go through that list and create the new InactiveLetters.
        for (int m = 0; m < inactiveNotifList.Count; m++)
        {
            String texttt = inactiveNotifList[m].text;
            String[] textLines = texttt.Split("$"[0]);


            for (int j = 0; j < textLines.Length; j++)
            {
                textLines[j] = textLines[j].Trim();
            }


            string[] splitDate = textLines[0].Split("-"[0]);
            for (int q = 0; q < splitDate.Length; q++)
            {
                splitDate[q] = splitDate[q].Trim();
            }
            int[] newDate = new int[3];

            for (int p = 0; p < splitDate.Length; p++)
            {
                newDate[p] = int.Parse(splitDate[p]);
            }

            string imgRef = textLines[4];
            string linkRef = textLines[5];
            if (imgRef.Contains("NO"))
            {
                imgRef = "";
            }
            if (linkRef.Contains("NO"))
            {
                linkRef = "";
            }

            DateTime tempTime = new DateTime(newDate[2], newDate[0], newDate[1]);

            InactiveNotification newNotif = new InactiveNotification(textLines[1], textLines[3], imgRef, linkRef, tempTime);
            inactiveNotifications.Add(newNotif);
        }
    }

    //Loads novelogues.
    public void LoadNovelogues()
    {
        List<TextAsset> activeNovelList = new List<TextAsset>();
        //Start with the length of the active letters.
        for (int m = 0; m < activeNovelogueNames.Length; m++)
        {
            TextAsset tempList = (TextAsset)Resources.Load(actToLoad + "/" + activeNovelogueLocation + "/" + activeNovelogueNames[m]);
            activeNovelList.Add(tempList);
        }

        //Lines are separated by the "$" symbol.
        //Index 0: Header (string)
        //Index 1: Summary (string)
        //Index 2: Read time (int)
        //Index 3: Novel date (string)
        //Index 4: Chapter (int)
        //Index 5: Amended chapter index (int)
        //Index 6: Chapter section (int)

        for (int i = 0; i < activeNovelList.Count; i++)
        {
            String texttt = activeNovelList[i].text;
            String[] textLines = texttt.Split("$"[0]);

            for (int j = 0; j < textLines.Length; j++)
            {
                textLines[i] = textLines[i].Trim();
            }

            ActiveNovelogue newLog = new ActiveNovelogue(textLines[0], textLines[1], textLines[2], textLines[3], int.Parse(textLines[4]), int.Parse(textLines[5]), int.Parse(textLines[6]));
            activeNovelogues.Add(newLog);
        }

        //--------------------------------------------------------
        //LOAD INACTIVE NOTIFICATIONS
        List<TextAsset> inactiveNovelList = new List<TextAsset>();
        for (int k = 0; k < inactiveNovelogueNames.Length; k++)
        {
            TextAsset tempList = (TextAsset)Resources.Load(actToLoad + "/" + inactiveNovelogueLocation + "/" + inactiveNovelogueNames[k]);
            inactiveNovelList.Add(tempList);
        }

        //Okay, so now we need to go through that list and create the new InactiveLetters.
        for (int m = 0; m < inactiveNovelList.Count; m++)
        {
            String texttt = inactiveNovelList[m].text;
            String[] textLines = texttt.Split("$"[0]);


            for (int j = 0; j < textLines.Length; j++)
            {
                textLines[j] = textLines[j].Trim();
            }

            //The date is always the last index, so index 7.
            string[] splitDate = textLines[7].Split("-"[0]);
            for (int q = 0; q < splitDate.Length; q++)
            {
                splitDate[q] = splitDate[q].Trim();
            }
            int[] newDate = new int[3];

            for (int p = 0; p < splitDate.Length; p++)
            {
                newDate[p] = int.Parse(splitDate[p]);
            }

            //Lines are separated by the "$" symbol.
            //Index 0: Header (string)
            //Index 1: Summary (string)
            //Index 2: Read time (int)
            //Index 3: Novel date (string)
            //Index 4: Chapter (int)
            //Index 5: Amended chapter index (int)
            //Index 6: Chapter section (int)
            //Index 7: Delivery date (DateTime)

            DateTime tempTime = new DateTime(newDate[2], newDate[0], newDate[1]);

            InactiveNovelogue newLog = new InactiveNovelogue(textLines[0], textLines[1], textLines[2], textLines[3], int.Parse(textLines[4]), int.Parse(textLines[5]),
                int.Parse(textLines[6]), tempTime);
            inactiveNovelogues.Add(newLog);
        }
    }

    //Loads item delivery.
    public void LoadItemDelivery()
    {
        List<TextAsset> activeItemList = new List<TextAsset>();
        //Start with the length of the active letters.
        for (int m = 0; m < activeItemDropNames.Length; m++)
        {
            TextAsset tempList = (TextAsset)Resources.Load(actToLoad + "/" + activeItemDropLocation + "/" + activeItemDropNames[m]);
            activeItemList.Add(tempList);
        }

        //ACTIVE ITEM INDEX:
        //Lines are separated by the "$" symbol.
        //First line (index 0):  Date (not used for active)
        //Second line (index 1): Signifier (string)
        //Third line (index 2): Asset name (string)
        //Fourth line (index 3): Room (string)
        //Fifth line (index 4): Category (string)
        //Sixth line (index 5):  Header (string)
        //Seventh line (index 6): Subheader (string)
        //Index 7: Sender (string)
        //Index 8: CTA Text (string)
        for (int i = 0; i < activeItemList.Count; i++)
        {
            String texttt = activeItemList[i].text;
            String[] textLines = texttt.Split("$"[0]);

            for (int j = 0; j < textLines.Length; j++)
            {
                textLines[i] = textLines[i].Trim();
            }

            ActiveItemDrop newDrop = new ActiveItemDrop(textLines[1], textLines[2], textLines[3], textLines[4], textLines[5], textLines[6], textLines[7], textLines[8]);
            activeItemDrops.Add(newDrop);
        }

        //--------------------------------------------------------
        //LOAD INACTIVE Item Drops
        List<TextAsset> inactiveItemList = new List<TextAsset>();
        for (int k = 0; k < inactiveItemDropNames.Length; k++)
        {
            TextAsset tempList = (TextAsset)Resources.Load(actToLoad + "/" + inactiveItemDropLocation + "/" + inactiveItemDropNames[k]);
            inactiveItemList.Add(tempList);
        }

        //Okay, so now we need to go through that list and create the new InactiveLetters.
        for (int m = 0; m < inactiveItemList.Count; m++)
        {
            String texttt = inactiveItemList[m].text;
            String[] textLines = texttt.Split("$"[0]);


            for (int j = 0; j < textLines.Length; j++)
            {
                textLines[j] = textLines[j].Trim();
            }


            string[] splitDate = textLines[0].Split("-"[0]);
            for (int q = 0; q < splitDate.Length; q++)
            {
                splitDate[q] = splitDate[q].Trim();
            }
            int[] newDate = new int[3];

            for (int p = 0; p < splitDate.Length; p++)
            {
                newDate[p] = int.Parse(splitDate[p]);
            }

            DateTime tempTime = new DateTime(newDate[2], newDate[0], newDate[1]);

            InactiveItemDrop newDrop = new InactiveItemDrop(tempTime, textLines[1], textLines[2], textLines[3], textLines[4], textLines[5], textLines[6], textLines[7], textLines[8]);

            inactiveItemDrops.Add(newDrop);
        }
    }

    //Delivery icons only need to be established for:
    //Item Drops
    //Notifications
    void EstablishDeliveryIcons()
    {
        //First, item drops
        for (int i = 0; i < activeItemDrops.Count; i++)
        {
            if (activeItemDrops[i].hasItem)
            {
                //This letter has a series of icons.
                //Take the list of icon names, and search each database for it.  
                //Tableau lists first.  We should only ever need to look through inactive lists.
                for (int a = 0; a < inactiveTableaus.Count; a++)
                {
                    if (activeItemDrops[a].signifier == inactiveTableaus[a].tableau.name)
                    {
                        activeItemDrops[a].icon = inactiveTableaus[a].tableau;
                    }
                }

                for (int a = 0; a < activeTableaus.Count; a++)
                {
                    if (activeItemDrops[a].signifier == activeTableaus[a].tableau.name)
                    {
                        activeItemDrops[a].icon = activeTableaus[a].tableau;
                    }
                }

                //Checks for a match in the display items.
                if (displayItems.signifier.Contains(activeItemDrops[i].signifier))
                {
                    activeItemDrops[i].icon = displayItems.displayIcons[activeItemDrops[i].signifier];
                }
            }
        }

        //First, item drops
        for (int i = 0; i < inactiveItemDrops.Count; i++)
        {
            if (inactiveItemDrops[i].hasItem)
            {
                //This letter has a series of icons.
                //Take the list of icon names, and search each database for it.  
                //Tableau lists first.  We should only ever need to look through inactive lists.
                for (int a = 0; a < inactiveTableaus.Count; a++)
                {
                    if (inactiveItemDrops[i].signifier == inactiveTableaus[a].signifier)
                    {
                        inactiveItemDrops[i].icon = inactiveTableaus[a].tableau;

                    }
                }

                //Checks for a match in the display items.
                if (displayItems.signifier.Contains(inactiveItemDrops[i].signifier))
                {
                    inactiveItemDrops[i].icon = displayItems.displayIcons[inactiveItemDrops[i].signifier];
                }
            }
        }

        //Next, Notifications

        for (int i = 0; i < activeNotifications.Count; i++)
        {
            if (activeNotifications[i].hasImage)
            {
                //This letter has a series of icons.
                //Take the list of icon names, and search each database for it.  
                //Tableau lists first.  We should only ever need to look through inactive lists.
                for (int a = 0; a < activeTableaus.Count; a++)
                {
                    if (activeNotifications[a].imageRef == activeTableaus[a].tableau.name)
                    {
                        activeNotifications[a].image = activeTableaus[a].tableau;
                    }
                }

                for (int a = 0; a < inactiveTableaus.Count; a++)
                {
                    if (activeNotifications[a].imageRef == inactiveTableaus[a].tableau.name)
                    {
                        activeNotifications[a].image = inactiveTableaus[a].tableau;

                    }
                }
                //Checks for a match in the display items.
                if (displayItems.signifier.Contains(activeNotifications[i].imageRef))
                {
                    activeNotifications[i].image = displayItems.displayIcons[activeNotifications[i].imageRef];
                }
            }
        }
        for (int i = 0; i < inactiveNotifications.Count; i++)
        {
            if (inactiveNotifications[i].hasImage)
            {
                //This letter has a series of icons.
                //Take the list of icon names, and search each database for it.  
                //Tableau lists first.  We should only ever need to look through inactive lists.
                for (int a = 0; a < inactiveTableaus.Count; a++)
                {
                    if (inactiveNotifications[a].imageRef == inactiveTableaus[a].tableau.name)
                    {
                        inactiveNotifications[a].image = inactiveTableaus[a].tableau;

                    }
                }
                //Checks for a match in the display items.
                if (displayItems.signifier.Contains(inactiveNotifications[i].imageRef))
                {
                    inactiveNotifications[i].image = displayItems.displayIcons[inactiveNotifications[i].imageRef];
                }
            }
        }
    }

    public void LoadActiveTableaus()
    {
        //So to load active tableaus, we have the location already.
        //We need to use the signifier, which is the item's name.
        Sprite[] tempList = new Sprite[0];
        tempList = Resources.LoadAll<Sprite>(actToLoad + "/" + activeTableauLocation);

        if (tempList.Length != 0)
        {
            for (int i = 0; i < tempList.Length; i++)
            {
                activeTableaus.Add(new ActiveTableauList(tempList[i], tempList[i].name));
            }
        }
    }

    public void LoadInactiveTableaus()
    {
        for (int k = 0; k < inactiveDropDates.Count; k++)
        {
            string[] splitDate = inactiveDropDates[k].Split("-"[0]);
            int[] newDate = new int[3];
            for (int m = 0; m < splitDate.Length; m++)
            {
                newDate[m] = int.Parse(splitDate[m]);
            }

            DateTime newTime = new DateTime(newDate[2], newDate[0], newDate[1]);

            Sprite[] tempList = new Sprite[0];
            //For GGC, we are loading from a different folder.
            tempList = Resources.LoadAll<Sprite>(actToLoad + "/" + inactiveTableauLocation + "/" + inactiveDropDates[k]);
            if (tempList.Length != 0)
            {
                for (int i = 0; i < tempList.Length; i++)
                {
                    inactiveTableaus.Add(new InactiveTableauList(newTime, tempList[i], tempList[i].name));
                }
            }
        }
    }

    //Load the clothing based on the string names of each doll to add into save data.
    //We have the names of each garment, so we will loop through the strings.
    public Sprite LoadDollClothing(string key, string pose)
    {
        //This is being done after the assets are loaded, so our active databases should already exist.
        for (int i =0; i < activeClothing.Count; i++)
        {
            if (activeClothing[i].pose == pose)
            {
                //We've hit the right pose.  Now search the list for the key.
                for (int j =0; j < activeClothing[i].assetSprites.Count; j++)
                {
                    if (activeClothing[i].assetSprites[j].name == key)
                    {
                        //This is it, return it.
                        return activeClothing[i].assetSprites[j];
                    }
                }
            }
        }
        return null;
    }

    //When we have to deal with a double category, we need to be a little better about what we return.
    public string ReturnDoubleDollKey(string primaryKey, string spriteName, string pose)
    {
        switch (primaryKey)
        {
            case "Hats":
                if (spriteName.Contains("BACK"))
                {
                    //This is the hat back, so return that.
                    return "Hat Back";
                }
                else
                {
                    return "Hat Front";
                }
            case "Capes":
                if (spriteName.Contains("BACK"))
                {
                    return "Cape Back";
                }
                else
                {
                    return "Cape Front";
                }
            case "Shawls":
                //This is pose dependent.
                if (pose == "3Q")
                {
                    if (spriteName.Contains("BACK"))
                    {
                        return "Shawl Back";
                    }
                    else
                    {
                        return "Shawl Front";
                    }
                }
                else
                {
                    return primaryKey;
                }
            case "Gloves":
                if (spriteName.Contains("LEFT"))
                {
                    return "Left Glove";
                }
                else
                {
                    return "Right Glove";
                }
            default:
                break;
        }
        return null;
    }

    //Load the clothing based on the string names of each doll to add into save data.
    //We have the names of each garment, so we will loop through the strings.
    public string ReturnDollKey(string clothingItem, string pose)
    {
        //This is being done after the assets are loaded, so our active databases should already exist.
        for (int i = 0; i < activeClothing.Count; i++)
        {
            if (activeClothing[i].pose == pose)
            {
                //We've hit the right pose.  Now search the list for the key.
                for (int j = 0; j < activeClothing[i].assetSprites.Count; j++)
                {
                    if (activeClothing[i].assetSprites[j].name == clothingItem)
                    {
                        //This is it, return it.
                        if (activeClothing[i].itemClass == "Hats" || activeClothing[i].itemClass == "Capes" || activeClothing[i].itemClass == "Shawls" || activeClothing[i].itemClass == "Gloves")
                        {
                            return ReturnDoubleDollKey(activeClothing[i].itemClass, activeClothing[i].assetSprites[j].name, pose);
                        }
                        return activeClothing[i].itemClass;
                    }
                }
            }
        }
        return null;
    }

    //Load the clothing based on the string names of each doll to add into save data.
    //We have the names of each garment, so we will loop through the strings.
    public string ReturnDollJewelryKey(string clothingItem, string pose)
    {
        //This is being done after the assets are loaded, so our active databases should already exist.
        for (int i = 0; i < activeJewelry.Count; i++)
        {
            if (activeJewelry[i].pose == pose)
            {
                //We've hit the right pose.  Now search the list for the key.
                for (int j = 0; j < activeJewelry[i].assetSprites.Count; j++)
                {
                    if (activeJewelry[i].assetSprites[j].name == clothingItem)
                    {
                        return activeJewelry[i].itemClass;
                    }
                }
            }
        }
        return null;
    }

    //Load the jewelry based on the string names of each doll to add into save data.
    //We have the names of each item, so we will loop through the strings.
    public Sprite LoadDollJewelry(string key, string pose)
    {
        //This is being done after the assets are loaded, so our active databases should already exist.
        for (int i = 0; i < activeJewelry.Count; i++)
        {
            if (activeJewelry[i].pose == pose)
            {
                //We've hit the right pose.  Now search the list for the key.
                for (int j = 0; j < activeJewelry[i].assetSprites.Count; j++)
                {
                    if (activeJewelry[i].assetSprites[j].name == key)
                    {
                        //This is it, return it.
                        return activeJewelry[i].assetSprites[j];
                    }
                }
            }
        }
        return null;
    }

    public void TransferData()
    {
        DataManager playerData = FindObjectOfType<DataManager>();
        playerData.savedTableaus = this.tempSprites;

        ClothingDatabase theclothingManager = FindObjectOfType<ClothingDatabase>();
        theclothingManager.activeClothing = this.activeClothing;
        theclothingManager.inactiveClothing = this.inactiveClothing;
        theclothingManager.displayItems = this.displayItems;

        TableauDatabase theTableauManager = FindObjectOfType<TableauDatabase>();
        theTableauManager.activeTableaus = this.activeTableaus;
        theTableauManager.inactiveTableaus = this.inactiveTableaus;

        MailDatabase theMailManager = FindObjectOfType<MailDatabase>();
        theMailManager.unreadMail = this.activeRegency;
        theMailManager.inactiveMail = this.inactiveRegency;
        theMailManager.activeNovelogues = this.activeNovelogues;
        theMailManager.inactiveNovelogues = this.inactiveNovelogues;
        theMailManager.activeItemDrops = this.activeItemDrops;
        theMailManager.inactiveItemDrops = this.inactiveItemDrops;
        theMailManager.activeNotifications = this.activeNotifications;
        theMailManager.inactiveNotifications = this.inactiveNotifications;

        ClockTracker theClock = FindObjectOfType<ClockTracker>();
        theClock.act1Times = this.act1Times;
        theClock.act2Times = this.act2Times;
        theClock.act3Times = this.act3Times;
        theClock.act4Times = this.act4Times;
        theClock.act5Times = this.act5Times;

        JewelryDatabase theJewelry = FindObjectOfType<JewelryDatabase>();
        theJewelry.activeJewelry = this.activeJewelry;
        theJewelry.inactiveJewelry = this.inactiveJewelry;     

        PnPDatabase pnpNovel = FindObjectOfType<PnPDatabase>();
        pnpNovel.chapters = this.chapters;

        AdventManager adventManager = FindObjectOfType<AdventManager>();
        adventManager.adventDrops = advents;
    }


    //THESE ARE ALL DEBUG METHODS.
    void DebugLetters()
    {
        for (int i = 0; i < inactiveRegency.Count; i++)
        {
            Debug.Log(inactiveRegency[i].sentDate);
            Debug.Log(inactiveRegency[i].content);
            Debug.Log(inactiveRegency[i].dateActive.ToString());
        }
    }

    public void DebugClothingAssets()
    {
        for (int z = 0; z < inactiveClothing.Count; z++)
        {
            Debug.Log("INACTIVE ITEM: " + inactiveClothing[z].itemClass + " Pose: " + inactiveClothing[z].pose + "Drop Date: " + inactiveClothing[z].stringDate);
            foreach (Sprite sprrr in inactiveClothing[z].assetSprites)
            {
                Debug.Log("\nSprite name: " + sprrr.name + " which drops on " + inactiveClothing[z].stringDate);
            }
        }
        for (int z = 0; z < activeClothing.Count; z++)
        {
            Debug.Log("ACTIVE ITEM: " + activeClothing[z].itemClass + " Pose: " + activeClothing[z].pose);
            foreach (Sprite sprrr in activeClothing[z].assetSprites)
            {
                Debug.Log("\nSprite name: " + sprrr.name);
            }
        }
    }

    public void DebugJewelry()
    {
        Debug.Log("Debugging jewelry...");
        for (int z = 0; z < inactiveJewelry.Count; z++)
        {
            Debug.Log("INACTIVE ITEM: " + inactiveJewelry[z].itemClass + " Pose: " + inactiveJewelry[z].pose + "Drop Date: " + inactiveJewelry[z].stringDate);
            foreach (Sprite sprrr in inactiveJewelry[z].assetSprites)
            {
                Debug.Log("\nSprite name: " + sprrr.name + " which drops on " + inactiveJewelry[z].stringDate);
            }
        }
        for (int q = 0; q < activeJewelry.Count; q++)
        {
            Debug.Log("ACTIVE ITEM: " + activeJewelry[q].itemClass + " Pose: " + activeJewelry[q].pose);
            foreach (Sprite sprrr in activeJewelry[q].assetSprites)
            {
                Debug.Log("\nSprite name: " + sprrr.name);
            }
        }
    }

    public void DebugTableaus()
    {
        for (int i = 0; i < activeTableaus.Count; i++)
        {
            Debug.Log("ACTIVE TABLEAU: " + activeTableaus[i].tableau.name);
        }
        for (int m = 0; m < inactiveTableaus.Count; m++)
        {
            Debug.Log("INACTIVE TABLEAU: " + inactiveTableaus[m].tableau.name);
        }
    }
}
