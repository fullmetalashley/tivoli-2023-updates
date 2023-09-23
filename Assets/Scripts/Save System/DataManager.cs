using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/*Script purpose: track  information that needs to move between the scenes.  
 * Information in this script is accessed by multiple elements, including TimeCheck,
 * the SaveSystem, and other elements.  
 * Some of these elements are also pushed into the saved data for the game itself.
 * Please note that this script only contains data, and does not manipulate data from other elements.
 * This helps keep the script accessible from other elements regardless of what scene is currently active.*/
public class DataManager : MonoBehaviour
{
    //----------------------------------------------------
    //Data manager information
    //This tracks the current instance of the DataManager and replaces it as necessary.
    public static DataManager instance;
    //This checks to see if there is any content loaded, i.e. the player has played the game before.
    public bool dataExists;

    //----------------------------------------------------
    //DOLL INFORMATION
    //A running list of what the dolls are currently wearing so they persist during a session.
    [Header("Doll data")]
    //Tracks if the dolls have been accessed.
    public bool dollsSaved;
    public bool jewelrySaved;
    //Tracks which doll was dressed last to load them first.
    public string lastAccessedDoll;

    public Doll elizabeth;
    public Doll jane;
    public string categoryToOpen;   //Category to open if the player is entering a dressing area from the delivery drops.


    //SAVE DATA FOR CLOTHING
    [Header("Save data for clothing")]
    public string[] eClothing;
    public string[] jClothing;
    public string[] eJewels;
    public string[] jJewels;


    //----------------------------------------------------
    //DATE INFORMATION
    //Tracks the date the player last played.
    [Header("Date information")]
    public int[] inGameSplit;
    //Tracks the date the player last played.
    public int[] realTimeSplit;

    //These two items track the previous dates from the last time the player quit the game, separate from this instance.
    public DateTime lastPlayedInGame;   //This is stored when the player saves the game.
    public DateTime lastPlayedRealTime;   //The real time last played.

    //These items track the current dates for this play session, which will be stored when the player quits.
    public DateTime currentPlayedRealTime;   //The current system date.
    public DateTime currentPlayedInGame;   //The current game date.


    //----------------------------------------------------
    //TABLEAUS
    [Header("Tableau save data")]
    //The directory where the tableaus are saved.  Defaults to the project folder.
    public string tableauDirectory;
    //A list of all sprites for the tableaus that have been created.
    public List<Sprite> savedTableaus;
    public List<DateTime> tableauDates;
    public string[] tableauDatesString;
    public Texture2D[] tableausToSave;
    public byte[][] tableauBytes;


    //----------------------------------------------------
    //PNP / eReader info
    //The indexes for the PnP reading.
    [Header("PnP data")]
    public bool bookRead;
    public int pnpChapter;
    public int pnpSection;
    public int listIndex;
    public bool novelogueTransfer;
    public float bookmarkPosition;  //The position of the bookmark as previously saved
    public float scrollValue;   //The position of the scroll window as previously saved

    //----------------------------------------------------
    //ADVENT TRACKING
    [Header("Advent data")]
    public List<int> adventDaysUnlocked;
    public int[] adventTransferValues;

    //----------------------------------------------------
    //LETTERS READ
    [Header("Deliverable data")]
    public int unreadLettersProcessed;
    public bool mailAccessed;

    public int notifProcessed;
    public int itemDropProcessed;
    public int novelogueProcessed;

    //----------------------------------------------------
    //SETTINGS
    [Header("Settings")]
    public bool musicOn;
    public bool particlesOn;
    public bool dayTimeOn;  //This is a debugging setting only.
    public bool SFXOn;

    //----------------------------------------------------
    //ACHIEVEMENTS
    [Header("Achievement data")]
    public int achievementsUnlocked;
    public int currentAchievement;
    public List<Achievement> unlocked;
    public List<int> unlockedIndexes;
    public int[] indexesToBeSaved;
    public int catClicks;


    //Conditions for the current three achievements.  NOT PERMANENT!
    public bool cbVisited;
    public bool dressingVisited;
    public bool galleryVisit;

    

    //----------------------------------------------------
    //DEBUGGING
    [Header("Debug data")]
    public bool achievementsManual = true; //Can the achievements be manually uncovered?  Defaults to true.
    public bool timeOverride = false;   //Can we manually set the clock for passive updates?  Defaults to false.



    //This does a check to avoid duplicating objects across scene loads.
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    //Constantly be checking the time.
    private void Update()
    {
        currentPlayedRealTime = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);
    }

    //Accesses the save system to save the current data.
    public void SaveGame()
    {
        //Set the saved inGameDate to the values of the current in-game date.
        inGameSplit = new int[3];
        inGameSplit[0] = currentPlayedInGame.Month;
        inGameSplit[1] = currentPlayedInGame.Day;
        inGameSplit[2] = currentPlayedInGame.Year;

        //Get the system date and save that.
        realTimeSplit = new int[3];
        realTimeSplit[0] = currentPlayedRealTime.Month;
        realTimeSplit[1] = currentPlayedRealTime.Day;
        realTimeSplit[2] = currentPlayedRealTime.Year;

        //Sets the achievements to a list of indexes that can be read when the game is reloaded.
        indexesToBeSaved = new int[unlocked.Count];
        for (int i =0; i < unlocked.Count; i++)
        {
            indexesToBeSaved[i] = unlocked[i].index;
        }

        //Set the advent transfer values.
        adventTransferValues = new int[adventDaysUnlocked.Count];
        for (int i =0; i < adventDaysUnlocked.Count; i++)
        {
            adventTransferValues[i] = adventDaysUnlocked[i];
        }

        //Convert the tableaus to 2D textures.
        tableausToSave = new Texture2D[savedTableaus.Count];
        tableauBytes = new byte[savedTableaus.Count][];
        for (int i =0; i < savedTableaus.Count; i++)
        {
            //Convert it!
            // assume "sprite" is your Sprite object
            var croppedTexture = new Texture2D((int)savedTableaus[i].rect.width, (int)savedTableaus[i].rect.height);
            var pixels = savedTableaus[i].texture.GetPixels((int)savedTableaus[i].textureRect.x,
                                                    (int)savedTableaus[i].textureRect.y,
                                                    (int)savedTableaus[i].textureRect.width,
                                                    (int)savedTableaus[i].textureRect.height);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            tableausToSave[i] = croppedTexture; //Add it to the list.

            byte[] itemBGBytes = croppedTexture.EncodeToPNG();  //Create a PNG of this.
            tableauBytes[i] = itemBGBytes;  //Save this to the array of arrays.
            Debug.Log("One tableau saved with appropriate bytes");
        }

        //Convert the clothing lists to saveable data.
        if (elizabeth.clothing != null)
        {
            eClothing = new string[elizabeth.clothing.Count];

            List<string> tempClothes = new List<string>();
            //We need to get the names of the sprites.
            foreach(KeyValuePair<string, Sprite> pair in elizabeth.clothing)
            {
                if (pair.Value != null)
                {
                    tempClothes.Add(pair.Value.name);
                }
            }

            for (int i =0; i < tempClothes.Count; i++)
            {
                if (eClothing.Length != 0)
                {
                    eClothing[i] = tempClothes[i];
                }
            }
        }
        else
        {
            eClothing = new string[0];
        }

        //Convert the clothing lists to saveable data.
        if (jane.clothing != null)
        {
            jClothing = new string[jane.clothing.Count];

            List<string> tempClothes = new List<string>();
            //We need to get the names of the sprites.
            foreach (KeyValuePair<string, Sprite> pair in jane.clothing)
            {
                if (pair.Value != null)
                {
                    tempClothes.Add(pair.Value.name);
                }
            }

            for (int i = 0; i < tempClothes.Count; i++)
            {
                if (jClothing.Length != 0)
                {
                    jClothing[i] = tempClothes[i];
                }
            }
        }
        else
        {
            jClothing = new string[0];
        }

        //Convert the clothing lists to saveable data.
        if (elizabeth.jewelry != null)
        {
            eJewels = new string[elizabeth.jewelry.Count];

            List<string> tempClothes = new List<string>();
            //We need to get the names of the sprites.
            foreach (KeyValuePair<string, Sprite> pair in elizabeth.jewelry)
            {
                if (pair.Value != null)
                {
                    tempClothes.Add(pair.Value.name);
                }
            }

            for (int i = 0; i < tempClothes.Count; i++)
            {
                if (eJewels.Length != 0)
                {
                    eJewels[i] = tempClothes[i];
                }
            }
        }
        else
        {
            eJewels = new string[0];
        }

        //Convert the clothing lists to saveable data.
        if (jane.jewelry != null)
        {
            jJewels = new string[jane.jewelry.Count];

            List<string> tempClothes = new List<string>();
            //We need to get the names of the sprites.
            foreach (KeyValuePair<string, Sprite> pair in jane.clothing)
            {
                if (pair.Value != null)
                {
                    tempClothes.Add(pair.Value.name);
                }
            }

            for (int i = 0; i < tempClothes.Count; i++)
            {
                if (jJewels.Length != 0)
                {
                    jJewels[i] = tempClothes[i];
                }
            }
        }
        else
        {
            jJewels = new string[0];
        }




        //Save the game.
        if (FindObjectOfType<DataManager>() != null)
        {
            FindObjectOfType<DataManager>().GetComponent<SaveSystem>().SaveData();
        }
    }

    //This is called to set up basic player data in case they have never played the game.
    //Will only be used the first time the player opens the game, if they reset, or for debugging purposes.
    public void DefaultLoad()
    {
        //Set the dates for the game.
        //Should be 09/29/1811 as the standard.
        currentPlayedInGame = new DateTime(1811, 09, 29);
        lastPlayedInGame = new DateTime(1811, 09, 29);

        inGameSplit = null;
        realTimeSplit = null;

        //Establish the real time dates as today, right now.
        currentPlayedRealTime = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);
        lastPlayedRealTime = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);

        //There are no unread letters to process.
        unreadLettersProcessed = 0;
        notifProcessed = 0;
        novelogueProcessed = 0;
        itemDropProcessed = 0;

        //This clears P&P content.
        pnpChapter = 0;
        scrollValue = 1f;
        bookRead = false;
  

        //Elizabeth is set as the base doll, and no garments or jewelry have been saved before.
        dollsSaved = false;
        jewelrySaved = false;

        jane = new Doll("Jane");
        elizabeth = new Doll("Elizabeth");

        lastAccessedDoll = "Elizabeth";

        //Adjust settings.
        musicOn = true;
        SFXOn = true;
        particlesOn = true;

        adventDaysUnlocked.Clear(); //Erase the advent calendar days.

        //Data exists is false, because we have no previous data.
        dataExists = false;

        //Reset achievements
        dressingVisited = false;
        cbVisited = false;
        galleryVisit = false;
        currentAchievement = 0;
        achievementsUnlocked = 0;
        unlocked.Clear();
        indexesToBeSaved = new int[0];

        //If we are default saving, we are not loading any tableaus, so we do not need to access that.
        SetTableauDirectory();
    }

    //Accesses the save system to load existing player data.
    public void LoadData()
    {
        //First, we get the saved data.
        PlayerData currentData = SaveSystem.LoadData();

        //Establish the last time we played this game.
        lastPlayedInGame = SetDate(currentData.gameDatePlayed);
        lastPlayedRealTime = SetDate(currentData.realTimeDatePlayed);

        currentPlayedInGame = SetDate(currentData.gameDatePlayed);
        currentPlayedRealTime = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);

        //Set up the deliverable read values.
        unreadLettersProcessed = currentData.readLetters;
        notifProcessed = currentData.readNotifs;
        novelogueProcessed = currentData.readNovelogues;
        itemDropProcessed = currentData.readItemDrops;

        //Set up PNP values.
        pnpChapter = currentData.chapterRead;
        bookRead = currentData.bookRead;
        scrollValue = currentData.pnpScrollValue;
        bookmarkPosition = currentData.bookmarkPos;

        //To set up the list index:
        for (int i =0; i < FindObjectOfType<PnPDatabase>().chapters.Count; i++)
        {
            if (FindObjectOfType<PnPDatabase>().chapters[i].chapterNumber == pnpChapter &&
                FindObjectOfType<PnPDatabase>().chapters[i].section == pnpSection)
            {
                listIndex = i;
                break;
            }
        }

        //Load achievements
        indexesToBeSaved = currentData.unlockedAchievements;

        //Load advent days
        adventDaysUnlocked = new List<int>();

        if (currentData.adventValues != null)
        {
            for (int i = 0; i < currentData.adventValues.Length; i++)
            {
                adventDaysUnlocked.Add(currentData.adventValues[i]);
            }
        }

        //Load the saved tableaus.
        savedTableaus = new List<Sprite>();
        if (currentData.tableauBytes != null)
        {
            for (int i =0; i < currentData.tableauBytes.Length; i++)
            {
                //First: We need to load a new texture from those bytes.
                // LoadImage will replace with with incoming image size.  Texture size doesn't matter, the byte data will replace this.
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(currentData.tableauBytes[i]);

                Sprite NewSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100f);
                savedTableaus.Add(NewSprite);
            }
        }

        dressingVisited = currentData.dressing;
        cbVisited = currentData.cb;
        galleryVisit = currentData.gallery;


        //Load settings
        musicOn = currentData.musicOn;
        SFXOn = currentData.SFXOn;
        particlesOn = currentData.particlesOn;

        jane = new Doll("Jane");
        elizabeth = new Doll("Elizabeth");
        lastAccessedDoll = "Elizabeth";

        dataExists = true;

        SetTableauDirectory();

        //Load the doll clothing if a day has passed.
        int timeGap = Mathf.RoundToInt((float)((currentPlayedInGame - lastPlayedInGame).TotalDays));
        if (timeGap == 0)
        {
            //The clothing is the same, so load it.
            elizabeth.clothing = new Dictionary<string, Sprite>();
            jane.clothing = new Dictionary<string, Sprite>();

            jane.clothing = new Dictionary<string, Sprite>();
            jane.jewelry = new Dictionary<string, Sprite>();


            for (int i = 0; i < currentData.eClothing.Length; i++)
            {
                if (FindObjectOfType<AssetLoader>().ReturnDollKey(currentData.eClothing[i], "3Q") != null)
                {
                   elizabeth.clothing.Add(FindObjectOfType<AssetLoader>().ReturnDollKey(currentData.eClothing[i], "3Q"),
                        FindObjectOfType<AssetLoader>().LoadDollClothing(currentData.eClothing[i], "3Q"));
                }
            }

            for (int i = 0; i < currentData.jClothing.Length; i++)
            {
                if (FindObjectOfType<AssetLoader>().ReturnDollKey(currentData.jClothing[i], "Straight") != null)
                {
                    jane.clothing.Add(FindObjectOfType<AssetLoader>().ReturnDollKey(currentData.jClothing[i], "Straight"),
                                        FindObjectOfType<AssetLoader>().LoadDollClothing(currentData.jClothing[i], "Straight"));
                }
            }

            for (int i = 0; i < currentData.eJewels.Length; i++)
            {
                //FOR EARRINGS: The same sprite will be added, but the value needs to be different each time.
                if (FindObjectOfType<AssetLoader>().ReturnDollJewelryKey(currentData.eJewels[i], "3Q") == "Earrings")
                {
                    //Okay, we will start with the left earring.
                    Sprite earring = FindObjectOfType<AssetLoader>().LoadDollJewelry(currentData.eJewels[i], "3Q");

                    if (!elizabeth.jewelry.ContainsKey("Left Earring"))
                    {
                        elizabeth.jewelry.Add("Left Earring", earring);
                    }
                    else
                    {
                        elizabeth.jewelry.Add("Right Earring", earring);
                    }
                }
                else
                {
                    if (FindObjectOfType<AssetLoader>().ReturnDollJewelryKey(currentData.eJewels[i], "3Q") != null)
                    {
                        elizabeth.jewelry.Add(FindObjectOfType<AssetLoader>().ReturnDollJewelryKey(currentData.eJewels[i], "3Q"),
                                            FindObjectOfType<AssetLoader>().LoadDollJewelry(currentData.eJewels[i], "3Q"));
                    }
                }
            }

            for (int i = 0; i < currentData.jJewels.Length; i++)
            {
                //FOR EARRINGS: The same sprite will be added, but the value needs to be different each time.
                if (FindObjectOfType<AssetLoader>().ReturnDollJewelryKey(currentData.jJewels[i], "Straight") == "Earrings")
                {
                    //Okay, we will start with the left earring.
                    Sprite earring = FindObjectOfType<AssetLoader>().LoadDollJewelry(currentData.jJewels[i], "Straight");

                    if (!jane.jewelry.ContainsKey("Left Earring"))
                    {
                        jane.jewelry.Add("Left Earring", earring);
                    }
                    else
                    {
                        jane.jewelry.Add("Right Earring", earring);
                    }
                }
                else
                {
                    if (FindObjectOfType<AssetLoader>().ReturnDollJewelryKey(currentData.jJewels[i], "Straight") != null)
                    {
                        jane.jewelry.Add(FindObjectOfType<AssetLoader>().ReturnDollJewelryKey(currentData.jJewels[i], "Straight"),
                                            FindObjectOfType<AssetLoader>().LoadDollJewelry(currentData.jJewels[i], "Straight"));
                    }
                }
            }
        }
    }

    //DEBUGGING ONLY
    public void DebugList(Dictionary<string, Sprite> list)
    {
        foreach(KeyValuePair<string, Sprite> pair in list)
        {
            Debug.Log("KEY: " + pair.Key);
            if (pair.Value != null)
            {
                Debug.Log("Sprite name: " + pair.Value.name);
            }
            else
            {
                Debug.Log("No value.");
            }
        }
    }


    //----------------------------------------------
    //Non-save data sensitive functions.
    //Creates a new DateTime using YYYY/MM/DD format from the int list.
    public DateTime SetDate(int[] theDate)
    {
        return new DateTime(theDate[2], theDate[0], theDate[1]);
    }

    //If the player real time values need to be reset, like when the game resets at the beginning, this method is called.
    //Currently called by the Clock Tracker.
    public void ResetRealTime()
    {
        currentPlayedRealTime = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);
        lastPlayedRealTime = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);
    }

    //Return the current in game date as a string.  For debugging.
    public string ReturnGameDate()
    {
        return currentPlayedInGame.Month + "/" + currentPlayedInGame.Day + "/" + currentPlayedInGame.Year;
    }

    //Return the current real time date as a string.  For debugging.
    public string ReturnRealTimeDate()
    {
        return currentPlayedRealTime.Month + "/" + currentPlayedRealTime.Day + "/" + currentPlayedRealTime.Year;
    }


    //ACHIEVEMENT FUNCTIONS
    public bool CheckCondition(string condition)
    {
        switch (condition)
        {
            case "CB Check":
                return cbVisited;
            case "DR Visit":
                return dressingVisited;
            case "Gallery Visit":
                return galleryVisit;
            default:
                return false;
        }
    }

    public void SetCondition(string condition)
    {
        switch (condition)
        {
            case "CB Check":
                cbVisited = true;
                break;
            case "DR Visit":
                dressingVisited = true;
                break;
            case "Gallery Visit":
                galleryVisit = true;
                break;
            default:
                break;
        }
    }

    //ACHIEVEMENT CONTROLS----------------------------------
    //Why is this being done in here, and not in the achievement database?

    //DEBUGGING
    //Resets all achievements so none have been unlocked.
    public void ResetAllAchievements()
    {
        achievementsManual = true;  //Set achievements back to manual mode.

        achievementsUnlocked = 0;   
        currentAchievement = 0;
        unlocked.Clear();

        for (int i =0; i < indexesToBeSaved.Length; i++)
        {
            indexesToBeSaved[i] = 0;
        }

        //Clear all conditions.
        cbVisited = false;
        dressingVisited = false;
        galleryVisit = false;

    }

    //DEBUGGING
    //Sets the achievements to either be manually unlockable through discovery, or automatically unlocked.
    public void AchievementUnlockToggle()
    {
        achievementsManual = !achievementsManual;
        if (!achievementsManual)
        {
            //Set all achievements to be unlocked.
            //Find out how many achievements we don't have, and add them to the list.
            AchievementDatabase achievements = FindObjectOfType<AchievementDatabase>();

            if (unlocked.Count <= achievements.achievements.Count)
            {
                for (int i = 0; i < achievements.achievements.Count; i++)
                {
                    if (unlocked.Contains(achievements.achievements[i]))
                    {
                        //We have this achievement in the list already, so leave it alone.
                    }
                    else
                    {
                        achievementsUnlocked++;
                        SetCondition(achievements.achievements[i].condition);
                        unlockedIndexes.Add(achievements.achievements[i].index);
                        currentAchievement++;
                        unlocked.Add(achievements.achievements[i]);
                    }
                }
            }
        }
    }

    //Reset doll clothing if the debug clock has changed days.
    public void ClothingReset()
    {
        elizabeth.clothing = new Dictionary<string, Sprite>();
        elizabeth.jewelry = new Dictionary<string, Sprite>();

        jane.clothing = new Dictionary<string, Sprite>();
        jane.jewelry = new Dictionary<string, Sprite>();
    }

    //TABLEAU LOADING
    //This does NOT load tableaus from this directory; it just establishes a place on the player's computer to save these files locally if they want them.
    public void SetTableauDirectory()
    {
        //First, we need to determine if our tableau directory is functional.
        string ScreenCapDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        if (!ScreenCapDirectory.Contains("Documents"))
        {
            ScreenCapDirectory += "/Documents";
        }

        if (!ScreenCapDirectory.Contains("Tivoli Photos"))
        {
            Directory.CreateDirectory(ScreenCapDirectory += "/Tivoli Photos");
        }
        else
        {
            ScreenCapDirectory += "/Tivoli Photos";
        }

        tableauDirectory = ScreenCapDirectory;
    }

    //Checks to see if that file is already in the directory.
    //Adjust it so it takes a file name in, and returns true if that file already exists.
    public List<string> CheckToLoad(string fileName)
    {
        List<string> filePaths = new List<string>();
        for (int i = 0; i < Directory.GetFiles(tableauDirectory).Length; i++)
        {
            if (Directory.GetFiles(tableauDirectory)[i].Contains(fileName))
            {
                //That means that this is a tableau.
                filePaths.Add(Directory.GetFiles(tableauDirectory)[i]);
            }
        }
        return filePaths;
    }

    // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference.
    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);
        return NewSprite;
    }

    // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference.
    public Sprite LoadNewSprite(Texture2D text, float PixelsPerUnit = 100.0f)
    {
        Sprite NewSprite = Sprite.Create(text, new Rect(0, 0, text.width, text.height), new Vector2(0, 0), PixelsPerUnit);
        return NewSprite;
    }

    //A PNG is loaded from disk into a Texture2D
    //Will return null if the load fails.
    public static Texture2D LoadTexture(string FilePath)
    {
        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        else
        {
            Debug.Log("The file didn't exist.");
        }
        return null;                     // Return null if load failed

    }
}
