using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCheck : MonoBehaviour
{
    private DataManager playerData;

    public string condition;

    public int index;

    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();

        //When we land in this scene, check to see if we have completed this condition.
        if (!playerData.CheckCondition(condition)){
            //We haven't done this yet.
            playerData.SetCondition(condition);
            playerData.achievementsUnlocked++;
            playerData.unlockedIndexes.Add(index);
        }
    }
}
