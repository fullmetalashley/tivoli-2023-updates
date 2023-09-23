using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the slots that are toggled on and off by the doll controls.
public class JewelrySlotManager : MonoBehaviour
{
    //These are the slots controlled by the manager.
    public ClothingSlot currentSlot;

    public Dictionary<string, ClothingSlot> eSlots;
    public Dictionary<string, ClothingSlot> jSlots;

    //Slots that must be toggled because they are multiple sprite slots.
    public ClothingSlot otherEarSlot;

    //The main clothing slots.
    public Dictionary<string, ClothingSlot> currentDollSlots;

    //SCRIPT REFS
    private JewelryDollSkin dollControls;
    private JewelryClosetController theCloset;

//Set up the lists
    public void InitializeSlotLists()
    {
        dollControls = FindObjectOfType<JewelryDollSkin>();
        theCloset = FindObjectOfType<JewelryClosetController>();

        //Initialize all dictionaries
        eSlots = new Dictionary<string, ClothingSlot>();
        jSlots = new Dictionary<string, ClothingSlot>();

        currentDollSlots = new Dictionary<string, ClothingSlot>();

        //Initialize elizabeth slots
        foreach (KeyValuePair<string, Image> pair in dollControls.elizabethJewels)
        {
            eSlots.Add(pair.Key, pair.Value.GetComponent<ClothingSlot>());
        }
        //Initialize elizabeth slots
        foreach (KeyValuePair<string, Image> pair2 in dollControls.janeJewels)
        {
            jSlots.Add(pair2.Key, pair2.Value.GetComponent<ClothingSlot>());
        }

        ChangeSlotLists();
    }

    //Change the current list of slots based on which doll is active.
    public void ChangeSlotLists()
    {
        if (dollControls.elizabeth.activeSelf)
        {
            currentDollSlots = eSlots;
        }
        else
        {
            currentDollSlots = jSlots;
        }
    }

    //Turn on the corresponding spot based on the subcategory.
    public void ActivateSlot()
    {
        if (theCloset.subcategory == "Earrings")
        {
            DoubleSlotActivation();
        }
        else
        {
            ActivateSlot(theCloset.subcategory);
            currentSlot = currentDollSlots[theCloset.subcategory];
        }
    }

    public void ActivateSlot(string slot)
    {
        currentDollSlots[slot].GetComponent<Image>().enabled = true;
        currentDollSlots[slot].GetComponent<Image>().raycastTarget = true;
        if (currentDollSlots[slot].GetComponent<Image>().sprite == null)
        {
            SlotTransparency(slot);
        }
    }

    public void DoubleSlotActivation()
    {
        switch (theCloset.subcategory)
        {
            case "Earrings":
                ActivateSlot("Right Earring");
                ActivateSlot("Left Earring");

                //Set the slots in this script.

                currentSlot = currentDollSlots["Right Earring"];
                otherEarSlot = currentDollSlots["Left Earring"];
                break;
        }
    }

    //Set the slot to transparent if there is no sprite in it.
    public void SlotTransparency(string slot)
    {
        Color transparentColor = currentDollSlots[slot].GetComponent<Image>().color;
        transparentColor.a = 0f;
        currentDollSlots[slot].GetComponent<Image>().color = transparentColor;
    }

    //Set the slot to full color if it now has a sprite.
    public void SlotFullColor(string slot)
    {
        Color fullColor = currentDollSlots[slot].GetComponent<Image>().color;
        fullColor.a = 1f;
        currentDollSlots[slot].GetComponent<Image>().color = fullColor;
        currentDollSlots[slot].GetComponent<Image>().raycastTarget = false;
    }

    //Set clothing details for this slot.
    public void AddClothing(string slot)
    {
        SlotFullColor(slot);

        //Not sure we actually need this.
        currentDollSlots[slot].hasClothing = true;
    }

    //Resets the current slot.
    public void ResetSlot(string slot)
    {
        if (slot == "Earrings")
        {
            otherEarSlot.hasClothing = false;
            otherEarSlot = null;

            if (theCloset.currentCategory == "Earrings")
            {
                currentSlot.hasClothing = false;
                currentSlot = null;
            }
            currentDollSlots["Left Earring"].hasClothing = false;
            currentDollSlots["Right Earring"].hasClothing = false;

        }
        else
        {

            //Reset the slot for this specified garment.
            if (currentDollSlots[slot] != null)
            {
                currentDollSlots[slot].hasClothing = false;
            }

            //Is the current slot this garment?
            if (slot == theCloset.currentCategory)
            {
                if (currentSlot != null)
                {
                    currentSlot.hasClothing = false;
                    currentSlot = null;
                }
            }
        }
    }

    //Turn off all slots.
    public void ShutDownSlots()
    {
        //Reset the current slot.
        currentSlot = null;

        foreach (KeyValuePair<string, ClothingSlot> pair in currentDollSlots)
        {
            //There is no sprite here, so we turn off the image.
            if (pair.Value.GetComponent<Image>().sprite == null)
            {
                pair.Value.GetComponent<Image>().enabled = false;
            }
        }
    }
}
