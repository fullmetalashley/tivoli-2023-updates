using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SCRIPT PURPOSE: Powers on the debug menu.  Functionality is controlled in Debug Clock.
public class DebugMenu : MonoBehaviour
{

    public GameObject debugMenu;


    public void ToggleOnOff()
    {
        debugMenu.SetActive(!debugMenu.activeSelf);

        if (debugMenu.activeSelf)
        {
            GetComponent<DebugClock>().TimeCheck();
        }
    }
}
