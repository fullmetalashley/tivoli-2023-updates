using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control individual charm details.
public class CharmUI : MonoBehaviour
{
    [Header("Base refs")]
    public Text header;
    public Image icon;
    public Text hint;

    public bool locked;

    public void UpdateUI(Achievement achievement)
    {
        header.text = achievement.header;
        icon.sprite = achievement.icon;
        hint.text = achievement.hint;
        locked = false;
    }

    public void LockedUI()
    {
        header.text = "Locked";
        locked = true;
    }

    //If this is a locked achievement, we turn off the detail view button.
    public void DisableButton()
    {
        icon.GetComponent<Button>().interactable = false;
        LockedUI();
    }

    //Set the hint!
    public void HintOn()
    {
        if (locked)
        {
            hint.enabled = true;
        }
    }

    //Set the hint to off!
    public void HintOff()
    {
        if (locked)
        {
            hint.enabled = false;
        }
    }
}
