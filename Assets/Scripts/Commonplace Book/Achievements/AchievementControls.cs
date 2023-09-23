using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script controls: Control the achievements within the story map.
public class AchievementControls : MonoBehaviour
{
    //SCRIPT REFS
    private AchievementDatabase achievements;
    private DataManager playerData;

    //UI elements
    public GameObject achievementCoin;
    public Text header;
    public Text description;
    public Image icon;
    public GameObject achievementBlocker;

    public int currentIndex = 0;

    public List<int> toRemove;

    // Start is called before the first frame update
    void Start()
    {
        achievements = FindObjectOfType<AchievementDatabase>();
        playerData = FindObjectOfType<DataManager>();


        AchievementCheck();
    }

    //Check to see if we need to toggle the achievement on.
    public void AchievementCheck()
    {
        if (playerData.currentAchievement < playerData.achievementsUnlocked)
        {
            //We have an achievement pending.
            achievementCoin.SetActive(true);
            achievementBlocker.SetActive(true);
            achievementCoin.GetComponent<Animator>().SetBool("active", true);
            SkinAchievement();
        }
        else
        {
            achievementBlocker.SetActive(false);
            achievementCoin.SetActive(false);
            playerData.unlockedIndexes.Clear();

        }


    }

    //Put the appropriate settings on the coin object.
    public void SkinAchievement()
    {
        header.text = achievements.achievements[playerData.unlockedIndexes[currentIndex]].header;
        description.text = achievements.achievements[playerData.unlockedIndexes[currentIndex]].description;
        icon.sprite = achievements.achievements[playerData.unlockedIndexes[currentIndex]].icon;

        if (!playerData.unlocked.Contains(achievements.achievements[playerData.unlockedIndexes[currentIndex]]))
        {
            playerData.unlocked.Add(achievements.achievements[playerData.unlockedIndexes[currentIndex]]);
        }
    }

    //Moves to the next achievement in the queue.
    //If no more, the coin will turn off.
    public void NextAchievement()
    {
        playerData.currentAchievement++;
        currentIndex++;

        achievementCoin.GetComponent<Animator>().SetBool("active", false);

        StartCoroutine(CoinDelay(1f));
    }

    IEnumerator CoinDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        AchievementCheck();
    }

}
