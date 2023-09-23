using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Story the player's save data.
//Does not currently contain any PNP info or tableau info.

[System.Serializable]
public class PlayerData
{
    //DATE CONTENT
    //This is an int array of the last date played, and is translated into a DateTime.  THIS IS THE IN GAME DATE.
    public int[] gameDatePlayed;
    //This saves the last time the player accessed the app in real time.
    public int[] realTimeDatePlayed;

    //What achievemnts have been unlocked?
    //PLACEHOLDER AND WILL NEED TO BE BASED ON CHARM DESIGNS
    public int[] unlockedAchievements;

    public bool dressing;
    public bool cb;
    public bool gallery;

    public string tableauDirectory; //Saves the file location the player has their tableaus saved at.

    //READ DELIVERABLE CONTENT
    public int readLetters;
    public int readNotifs;
    public int readNovelogues;
    public int readItemDrops;

    //PNP CONTENT
    public int chapterRead;
    public int sectionRead;
    public float bookmarkPos;
    public float pnpScrollValue;
    public bool bookRead;

    //TABLEAU CONTENT
    public byte[][] tableauBytes;

    //ADVENT CONTENT
    public int[] adventValues;

    //SETTINGS
    public bool SFXOn;
    public bool particlesOn;
    public bool musicOn;

    //CLOTHING GARMENTS
    public string[] eClothing;
    public string[] jClothing;
    public string[] eJewels;
    public string[] jJewels;

    //Date data, read letters, and all achievements recorded.
    public PlayerData(int[] date, int[] realTimeDate,   //Date values
        int[] unlockedAchievements, //achievement values
        bool dressing, bool cb, bool gallery,   //TEMP achievement conditions
        string tableauDirectory,    //Where the tableaus are loaded by default
        int readLetters, int readNotifs, int readNovelogues, int readItemDrops,  //Read deliverables
        int chapterRead, int sectionRead, float bookmarkPos, float pnpScrollValue, bool bookRead,    //PnP data
        int[] adventDays,   //Advent calendar data
        bool SFXOn, bool particlesOn, bool musicOn, //Player settings
        byte[][] tableaus,  //Image data for the tableaus
        string[] eClothing, string[] jClothing, string[] eJewels, string[] jJewels //Clothing data
        )
    {
        gameDatePlayed = date;
        realTimeDatePlayed = realTimeDate;

        //Set the deliverable values.
        this.readLetters = readLetters;
        this.readNotifs = readNotifs;
        this.readNovelogues = readNovelogues;
        this.readItemDrops = readItemDrops;

        //Set eReader values.
        this.chapterRead = chapterRead;
        this.sectionRead = sectionRead;
        this.bookmarkPos = bookmarkPos;
        this.pnpScrollValue = pnpScrollValue;
        this.bookRead = bookRead;

        //Advent values
        this.adventValues = adventDays;

        //Settings values
        this.musicOn = musicOn;
        this.SFXOn = SFXOn;
        this.particlesOn = particlesOn;

        //Achievement values
        this.unlockedAchievements = unlockedAchievements;

        //Tableau values
        tableauBytes = tableaus;

        this.dressing = dressing;
        this.cb = cb;
        this.gallery = gallery;
        this.tableauDirectory = tableauDirectory;

        //Load clothing from previous day.
        this.eClothing = eClothing;
        this.jClothing = jClothing;
        this.eJewels = eJewels;
        this.jJewels = jJewels;
    }
}
