using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Control the OOBE and tutorial settings with a persistent manager that stays open across all scenes.
public class OOBEManager : MonoBehaviour
{
    public static OOBEManager instance;

    //LIST REFERENCES: for initialization only
    public List<string> rooms;
    public List<string> toolTipCodes;
    public List<string> discoveryStrings;

    //DICTIONARY REFS
    public Dictionary<string, bool> roomCodes;  //Whether or not a room has been accessed for the first time.
    public Dictionary<string, bool> tipCodes;   //What tips have specifically been accessed in that room.
    public Dictionary<string, bool> discoveryCodes;     //What features have / haven't been discovered.



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

    private void Start()
    {
        //Set up the room's 1st use dictionary.
        roomCodes = new Dictionary<string, bool>();
        tipCodes = new Dictionary<string, bool>();
        discoveryCodes = new Dictionary<string, bool>();

        for (int i =0; i < rooms.Count; i++)
        {
            roomCodes.Add(rooms[i], true); //Sets the baseline at true, as in "We are entering the room for the first time".
        }

        for (int i = 0; i < toolTipCodes.Count; i++)
        {
            tipCodes.Add(toolTipCodes[i], false); //Sets the baseline at false, as in "We have not accessed this tip yet."
        }

        for (int i = 0; i < discoveryStrings.Count; i++)
        {
            discoveryCodes.Add(discoveryStrings[i], false); //Sets the baseline at false, as in "We have not accessed this feature yet".
        }
    }

    //When we have encountered this for the first time, we set the code to false.
    public void FirstTimeEncounter(string code)
    {
        roomCodes[code] = false;
    }

    //Return whether or not this is a first time encounter.
    public bool ReturnEncounterStatus(string code)
    {
        return roomCodes[code];
    }

    //We have viewed this tip and can now record it as true.
    public void MiniTipRead(string code)
    {
        tipCodes[code] = true;
    }

    //When the game is reset, set all tutorial options to accessed.
    public void ResetOOBE()
    {
        //Set all room codes to true.
        for (int i = 0; i < rooms.Count; i++)
        {
            roomCodes[rooms[i]] = true;
        }

        //Set all tool tips to false.
        for (int i = 0; i < toolTipCodes.Count; i++)
        {
            tipCodes[toolTipCodes[i]] = false;
        }

        //Set all discovery codes to false.
        for (int i = 0; i < discoveryStrings.Count; i++)
        {
            discoveryCodes[discoveryStrings[i]] = false;
        }
    }

    //When OOBE is disabled from the tutorial, set everything so that it will not run anymore.
    public void DisableOOBE()
    {
        //Set all room codes to false.
        for (int i =0; i < rooms.Count; i++)
        {
            roomCodes[rooms[i]] = false;
        }

        //Set all tool tips to true.
        for (int i = 0; i < toolTipCodes.Count; i++)
        {
            tipCodes[toolTipCodes[i]] = true;
        }

        //Set all discovery codes to true.
        for (int i = 0; i < discoveryStrings.Count; i++)
        {
            discoveryCodes[discoveryStrings[i]] = true;
        }

        //Also, call the current Scene OOBE to recheck all tips so they don't accidentally trigger.
        FindObjectOfType<SceneOOBE>().ProcessTipsInDictionary();
    }
}
