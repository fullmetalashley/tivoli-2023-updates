using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Track all draggable elements and prevent them from being clicked by both buttons.
public class GarmentDrag : MonoBehaviour
{
    //Bool to track if something is being dragged.
    public bool dragActive;

    //Scene code
    public string scene;

    //Script ref
    public DisplayPopulator display;
    public JewelryDisplayPopulator jewelryDisplay;

    private void Start()
    {
        display = FindObjectOfType<DisplayPopulator>();
        jewelryDisplay = FindObjectOfType<JewelryDisplayPopulator>();
    }

    public void OnDrag()
    {
        dragActive = true;

        if (display != null)    //We are in the dressing room.
        {
            List<ClothingPanel> clothingDrags = display.currentPanelList;

            //Okay, we want to do this, but only for the ACTIVE items.
            for (int i = 0; i < clothingDrags.Count; i++)
            {
                if (clothingDrags[i].clothingActive)
                {
                    clothingDrags[i].GetComponent<Image>().raycastTarget = false;
                }
            }
        }

        if (jewelryDisplay != null) //We are in the mirror.
        {
            List<ClothingPanel> jewelryDrags = jewelryDisplay.currentPanelList;

            //Okay, we want to do this, but only for the ACTIVE items.
            for (int i = 0; i < jewelryDrags.Count; i++)
            {
                if (jewelryDrags[i].clothingActive)
                {
                    jewelryDrags[i].GetComponent<Image>().raycastTarget = false;
                }
            }
        }
    }

    //ONLY for the active items!  The display populator has all of our lists, and the closet has our active list.
    public void EndDrag()
    {
        dragActive = false;

        if (display != null)
        {
            List<ClothingPanel> clothingDrags = display.currentPanelList;

            for (int i = 0; i < clothingDrags.Count; i++)
            {
                //Is this allowed to be draggable?
                if (clothingDrags[i].GetComponent<ClothingPanel>().clothingActive)
                {
                    clothingDrags[i].GetComponent<Image>().raycastTarget = true;
                }
            }
        }

        if (jewelryDisplay != null)
        {
            List<ClothingPanel> jewelryDrags = jewelryDisplay.currentPanelList;

            //Okay, we want to do this, but only for the ACTIVE items.
            for (int i = 0; i < jewelryDrags.Count; i++)
            {
                if (jewelryDrags[i].clothingActive)
                {
                    jewelryDrags[i].GetComponent<Image>().raycastTarget = true;
                }
            }
        }
    }
}
