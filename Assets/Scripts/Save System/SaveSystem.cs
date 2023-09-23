using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//Script purpose: Control save operations.
    //Attached to the Player Data object.
public class SaveSystem : MonoBehaviour
{
    private DataManager inGameData;


    // Start is called before the first frame update
    void Start()
    {
        inGameData = GetComponent<DataManager>();
    }

    //This will save the game when the player has closed the app.
    public void OnApplicationQuit()
    {
        inGameData.SaveGame();
    }

    //This saves the player data to a .tiv file.  
    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.tiv";
        FileStream stream = new FileStream(path, FileMode.Create);

        //Writing to file
        //File order:
        /* In game date, real time date,
         * Unlocked achievements list,
         * Dressing bool, CB bool, gallery bool,
         * Tableau directory,
         * Read letters, read notifs, read novelogues, read item drops,
         * chapter read, bookmarkPos, scroll value, book read bool
         * advent days list
         * Dressing first time bool, sfx bool, particles bool, music on bool,
         * tableau bytes,
         * e clothing, j clothing, e jewels, j jewels
         * int[] date, int[] realTimeDate,   //Date values
         */
        PlayerData data = new PlayerData(inGameData.inGameSplit, inGameData.realTimeSplit,
            inGameData.indexesToBeSaved,
            inGameData.dressingVisited, inGameData.cbVisited, inGameData.galleryVisit,
            inGameData.tableauDirectory,
            inGameData.unreadLettersProcessed, inGameData.notifProcessed, inGameData.novelogueProcessed, inGameData.itemDropProcessed,
            inGameData.pnpChapter, inGameData.pnpSection, inGameData.bookmarkPosition, inGameData.scrollValue, inGameData.bookRead,
            inGameData.adventTransferValues,
            inGameData.SFXOn, inGameData.particlesOn, inGameData.musicOn,
            inGameData.tableauBytes,
            inGameData.eClothing, inGameData.jClothing, inGameData.eJewels, inGameData.jJewels) ;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    //This loads the existing player data.
    public static PlayerData LoadData()
    {
        string path = Application.persistentDataPath + "/player.tiv";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
    
    //A method checking to see if player data exists or not.
    public bool PlayerDataExists()
    {
        string path = Application.persistentDataPath + "/player.tiv";
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //A player's file can be deleted from within the game.
    //If we keep this, and it is used, the game should also perform a reset to the start with wiped content.
    public void ClearData()
    {
        string path = Application.persistentDataPath + "/player.tiv";
        if (File.Exists(path))
        {
            File.Delete(Application.persistentDataPath + "/player.tiv");
            Debug.Log("Deleted file: " + path);
            Debug.Log("File deleted.");
        }
        else
        {
            Debug.Log("No file found in " + path);
        }
        string[] fileNames = Directory.GetFiles(Application.persistentDataPath);
        for (int i = 0; i < fileNames.Length; i++)
        {
            if (fileNames[i].Contains("Tableau"))
            {
                File.Delete(fileNames[i]);
            }
        }
    }
}
