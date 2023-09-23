using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyoutManager : MonoBehaviour
{

    public List<ItemFlyout> clothingItems;
    public List<ClothingPanel> clothingPanels;

    public string category;

    public bool active; //A bool to track whether or not this category is active.  If it is, the items are on.  If not, the entire group is turned off.

    public int transitionComplete;

    public void TransitionItems()
    {
        
        foreach(ItemFlyout item in clothingItems)
        {
            item.Init();
            item.StartTransition();
        }
    }

    public void SignalTransitionComplete()
    {
        transitionComplete++;

        if (transitionComplete == clothingItems.Count)
        {
            //The entire list has transitioned, and we can mark ourselves as complete.
            transitionComplete = 0;

            if (FindObjectOfType<ClosetControls>() != null)
            {
                FindObjectOfType<ClosetControls>().CheckButtons();
            }

            if (FindObjectOfType<JewelryClosetController>() != null)
            {
                FindObjectOfType<JewelryClosetController>().CheckButtons();
            }
        }
    }
}
