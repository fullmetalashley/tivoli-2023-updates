using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Change the resolution based on a player designated choice.
public class ResolutionControls : MonoBehaviour
{
    //A list of all available resolutions.
    public List<Resolution> resolutions;

    public Dropdown dropdown;   //The dropdown control.

    //Current resolution debug text.
    public ResolutionDebug debugText;


    private void Start()
    {
        ManualResolutionCheck();
    }

    //Sets the resolution as the start of the game based on what's closest to the player's display.
    public void ManualResolutionCheck()
    {
        int screenX = Screen.currentResolution.width;
        int screenY = Screen.currentResolution.height;

        //We need to get the next closest resolution, one size down.
        //So if our resolution is 1950, the next closest is 1920.

        int subtraction = screenX - resolutions[0].resolutionX;    //Get our starting point.
        int nextSubtraction = 0;

        int baseIndex = 0;

        for (int i =0; i < resolutions.Count; i++)
        {
            nextSubtraction = screenX - resolutions[i].resolutionX;

            if (nextSubtraction < subtraction && nextSubtraction >= 0)
            {
                subtraction = nextSubtraction;
                baseIndex = i;
            }
        }

        ChangeResolution(baseIndex);
    }


    public void ChangeResolution(int index)
    {
        Screen.SetResolution(resolutions[index].resolutionX, resolutions[index].resolutionY, true);

        debugText.RecheckText();
    }

    public void SetDropdownValue()
    {
        ChangeResolution(dropdown.value);
    }
}
