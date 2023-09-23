using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the UI elements of the charm page in the CB.
public class CharmController : MonoBehaviour
{
    //SCRIPT REFS
    private DataManager playerData;
    private AchievementDatabase achievements;

    //Charm list of UI elements
    public List<CharmUI> charms;

    [Header("Detail modal refs")]
    public Text header;
    public Text description;
    public Image icon;
    public GameObject detailModal;

    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();
        achievements = FindObjectOfType<AchievementDatabase>();

        SkinCharms();
    }

    //Set up the  individual charms.
    public void SkinCharms()
    {
        //We should change this.  Let's skin every charm based on the ACHIEVEMENTS, then we'll adjust for what the player
        //has unlocked.  That way they appear in the same order every time.

        for (int i = 0; i < achievements.achievements.Count; i++)
        {
            if (playerData.unlocked.Contains(achievements.achievements[i]))
            {
                //Regular skin, because we have this one.
                charms[i].UpdateUI(achievements.achievements[i]);
            }
            else
            {
                //Not unlocked yet, so let's adjust the skin.
                charms[i].DisableButton();
                charms[i].hint.text = achievements.achievements[i].hint;
            }
        }
    }

    //Set up the detail modal.
    public void OpenDetail(int index)
    {
        detailModal.SetActive(true);
        header.text = playerData.unlocked[index].header;
        description.text = playerData.unlocked[index].description;
        icon.sprite = playerData.unlocked[index].icon; 
    }

    //CLose the detail view.
    public void CloseDetail()
    {
        detailModal.SetActive(false);
    }

}
