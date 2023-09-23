using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScrollWindows : MonoBehaviour
{
    public GameObject dressScrollWindow;
    public GameObject hatScrollWindow;

    public GameObject blocker;


    public void ToggleWindows(string category)
    {
        blocker.SetActive(true);
        switch (category)
        {
            case "Hats":
                hatScrollWindow.SetActive(true);
                dressScrollWindow.SetActive(false);
                break;
            case "Dresses":
                dressScrollWindow.SetActive(true);
                hatScrollWindow.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void TurnOffWindows()
    {
        hatScrollWindow.SetActive(false);
        dressScrollWindow.SetActive(false);
        blocker.SetActive(false);
    }
}
