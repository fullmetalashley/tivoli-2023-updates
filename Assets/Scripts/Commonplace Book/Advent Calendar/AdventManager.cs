using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Track the advent calendar and mark when they've been unlocked.
public class AdventManager : MonoBehaviour
{
    //SCRIPT REFS
    private DataManager playerData;

    //Tracking refs
    public int decDay;
    public List<Advent> adventDrops;

    //Persistence refs
    public static AdventManager control;

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

    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();
    }

    //Returns what day in December we are in.
    //Happens at the story map after all time checks have been run.
    public void CheckDecemberDay()
    {
        //We are in December.
        if (playerData.currentPlayedInGame.Month == 12)
        {
            decDay = playerData.currentPlayedInGame.Day;
        }
        else
        {
            decDay = 0;
        }
    }
}
