using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script is the initial load process for player data.
//It determine if player data exists, and will load that when the CONTINUE button is hit.
//If the player hits NEW GAME, player data will be erased prior to starting the game.
//This script only exists on the start screen of the game.
public class StartupDataLoad : MonoBehaviour
{
    private DataManager playerData;

    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();
        //If player data exists, initiate the load.
        if (playerData == null)
        {
            Debug.Log("Data Manager not found.");
        }
    }

    //This initializes the load.
    public void Loading()
    {
        //If the player has existing data, we will load that.
        if (playerData.GetComponent<SaveSystem>().PlayerDataExists())
        {
            playerData.LoadData();
        }
        else{
            //If no existing data, we load base data instead.
            playerData.GetComponent<SaveSystem>().ClearData();
            playerData.DefaultLoad();
        }
    }
}
