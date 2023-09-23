using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyFlyoutController : MonoBehaviour
{

    public List<DummyFlyout> flyingDresses;
    public List<DummyFlyout> flyingHats;

    public bool dresses;
    public bool hats;

    public void ToggleDresses()
    {
        dresses = !dresses;
        for (int i = 0; i < flyingDresses.Count; i++)
        {
            flyingDresses[i].GetComponent<DummyFlyout>().StartTransition();
        }
      
    }

    public void ToggleHats()
    {
        hats = !hats;
        for (int i = 0; i < flyingHats.Count; i++)
        {
            flyingHats[i].GetComponent<DummyFlyout>().StartTransition();
        }

    }

    public void ToggleGarments(string category)
    {
        switch (category){
            case "Dresses":
                if (hats)
                {
                    ToggleHats();
                }
                ToggleDresses();
                break;
            case "Hats":
                if (dresses)
                {
                    ToggleDresses();
                }
                ToggleHats();
                break;
            default:
                break;
        }
    }
}
