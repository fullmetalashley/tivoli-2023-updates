using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the individual headers and button groups based on the tableaus.
public class TableauCaptions : MonoBehaviour
{
    public List<GameObject> tableauButtons; //The individual buttons that control this tableau.
    public List<Text> tableauHeaders;   //All individual heads for this specific tableau.
    public Text quote;

    //Sets all buttons to the passed bool.
    public void ToggleButtons(bool value)
    {
        for (int i = 0; i < tableauButtons.Count; i++)
        {
            tableauButtons[i].SetActive(value);
        }
    }

    //Sets all headers to the passed bool.
    public void ToggleHeaders(bool value)
    {
        for (int i = 0; i < tableauHeaders.Count; i++)
        {
            tableauHeaders[i].enabled = value;
        }
    }
}
