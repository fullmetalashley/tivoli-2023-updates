using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Track and hold all achievements.
public class AchievementDatabase : MonoBehaviour
{
    public static AchievementDatabase control;

    public List<Achievement> achievements;

    //Initialize this instance of the achievement control so it can persist across scenes.
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

    //Set up the player's achievement list.
    public void SetAchievements()
    {
        DataManager playerData = FindObjectOfType<DataManager>();

        for (int i =0; i < playerData.indexesToBeSaved.Length; i++)
        {
            playerData.achievementsUnlocked++;
            playerData.currentAchievement++;

            playerData.unlocked.Add(achievements[playerData.indexesToBeSaved[i]]);
        }
    }
}
