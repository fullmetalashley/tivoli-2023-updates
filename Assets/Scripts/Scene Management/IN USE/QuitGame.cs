using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Exit application.
    //Currently called from the start screen.
    //Currently called from the main menu's quit button.
public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }
}
