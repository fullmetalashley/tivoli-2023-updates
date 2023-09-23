using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script purpose: Controls the flyout objects for the jewelry items.

public class JewelryFlyoutManager : MonoBehaviour
{
    public List<JewelryMovement> items;

    //Spread the items out to their designated locations.
    public void SpreadItems()
    {
        foreach(JewelryMovement spread in items){
            spread.StartTransition();
        }
    }
}
