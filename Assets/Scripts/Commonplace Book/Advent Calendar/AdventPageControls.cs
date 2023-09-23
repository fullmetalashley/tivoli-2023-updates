using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the UI functionality of the advent calendar.
//Stays in the CB.
public class AdventPageControls : MonoBehaviour
{
    //SCRIPT REFS
    private AdventManager adventManager;
    private DataManager playerData;

    //Advent buttons
    public List<GameObject> adventButtons;
    public List<Image> adventIcons;
    public List<Text> adventNumbers;

    [Header("Card links")]
    public Text description;
    public Text adventName;
    public Image icon;
    public GameObject cardModal;


    // Start is called before the first frame update
    void Start()
    {
        adventManager = FindObjectOfType<AdventManager>();
        playerData = FindObjectOfType<DataManager>();

        UpdateButtons();
    }

    //Go through each button up to our current day and track what we can unlock.
    public void UpdateButtons()
    {
        //We have a list of days that the player has unlocked.  They might not be sequential.
        //Go through our list of unlocked indexes, and set those first.
        for (int i = 0; i < playerData.adventDaysUnlocked.Count; i++) {
            adventIcons[playerData.adventDaysUnlocked[i]].enabled = true;
            adventIcons[playerData.adventDaysUnlocked[i]].sprite = adventManager.adventDrops[playerData.adventDaysUnlocked[i]].icon;
            adventNumbers[playerData.adventDaysUnlocked[i]].enabled = false;
        }

        //We've set every possible value that has been unlocked, even if it's happened out of order.  So now let's check the rest.
        //First, scroll up through the available days, and make sure they're all interactable if they haven't been turned on.
        for (int i = 0; i < adventManager.decDay; i++)
        {
            adventButtons[i].GetComponentInChildren<Button>().interactable = true;
        }

        //Now, start at the end of the list and turn these off of interactable.
        for (int i = adventManager.decDay; i < adventButtons.Count; i++)
        {
            adventButtons[i].GetComponentInChildren<Button>().interactable = false;
        }
    }

    //Open the card modal and skin it to the appropriate index.
    public void OpenCard(int index)
    {
        cardModal.SetActive(true);

        //Skin the card.
        description.text = adventManager.adventDrops[index].description;
        adventName.text = adventManager.adventDrops[index].name;
        icon.sprite = adventManager.adventDrops[index].icon;

        //If this is a new card, we need to set this image to the appropriate icon.
        adventIcons[index].enabled = true;
        adventIcons[index].sprite = adventManager.adventDrops[index].icon;
        adventNumbers[index].enabled = false;   //Turn off the number.

        //If we haven't unlocked this day, add it to the list.
        if (!playerData.adventDaysUnlocked.Contains(index))
        {
            playerData.adventDaysUnlocked.Add(index);
        }

    }

    //Close the card modal.
    public void CloseCard()
    {
        cardModal.SetActive(false);
    }
}
