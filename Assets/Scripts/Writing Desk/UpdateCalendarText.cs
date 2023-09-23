using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Update the text in the calendar based on the current in-game date.
public class UpdateCalendarText : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        DataManager playerData = FindObjectOfType<DataManager>();

        this.GetComponent<Text>().text = "1811" + "\n" + playerData.currentPlayedInGame.Day + "\n" +  ReturnMonth(playerData.currentPlayedInGame.Month);
    }

    public string ReturnMonth(int value)
    {
        switch (value)
        {
            case 1:
                return "JAN";
            case 2:
                return "FEB";
            case 3:
                return "MAR";
            case 4:
                return "APR";
            case 5:
                return "MAY";
            case 6:
                return "JUN";
            case 7:
                return "JUL";
            case 8:
                return "AUG";
            case 9:
                return "SEP";
            case 10:
                return "OCT";
            case 11:
                return "NOV";
            case 12:
                return "DEC";
            default:
                return "";
        }
    }
}
