using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the slots that are toggled on and off by the doll controls.

public class SlotManager : MonoBehaviour
{

    //These are the slots controlled by the manager.
    public ClothingSlot currentSlot;

    public Dictionary<string, ClothingSlot> eSlots;
    public Dictionary<string, ClothingSlot> jSlots;

    //Slots that must be toggled because they are multiple sprite slots.
    public ClothingSlot otherGloveSlot;
    public ClothingSlot shawlBackSlot;
    public ClothingSlot capeBackSlot;
    public ClothingSlot hatBackSlot;

    //The main clothing slots.
    public Dictionary<string, ClothingSlot> currentDollSlots;

    //SCRIPT REFS
    private DollSkin dollControls;
    private ClosetControls theCloset;

    public void InitializeSlotLists()
    {
        dollControls = FindObjectOfType<DollSkin>();
        theCloset = FindObjectOfType<ClosetControls>();

        //Initialize all dictionaries
        eSlots = new Dictionary<string, ClothingSlot>();
        jSlots = new Dictionary<string, ClothingSlot>();

        currentDollSlots = new Dictionary<string, ClothingSlot>();

        //Initialize elizabeth slots
        foreach (KeyValuePair<string, Image> pair in dollControls.elizabethGarments)
        {
            eSlots.Add(pair.Key, pair.Value.GetComponent<ClothingSlot>());
        }
        //Initialize elizabeth slots
        foreach (KeyValuePair<string, Image> pair2 in dollControls.janeGarments)
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
        if (theCloset.subcategory == "Gloves" || theCloset.subcategory == "Capes" || theCloset.subcategory == "Shawls" || theCloset.subcategory == "Hats")
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
            case "Hats":
                ActivateSlot("Hat Front");
                ActivateSlot("Hat Back");

                //Set the slots in this script.
                currentSlot = currentDollSlots["Hat Front"];
                hatBackSlot = currentDollSlots["Hat Back"];
                break;
            case "Gloves":
                ActivateSlot("Right Glove");
                ActivateSlot("Left Glove");

                //Set the slots in this script.
                currentSlot = currentDollSlots["Right Glove"];
                otherGloveSlot = currentDollSlots["Left Glove"];
                break;

            case "Capes":
                ActivateSlot("Cape Front");
                ActivateSlot("Cape Back");

                currentSlot = currentDollSlots["Cape Front"];
                capeBackSlot = currentDollSlots["Cape Back"];
                break;
            case "Shawls":
                //Elizabeth has double slots for shawls.  Jane does not.
                if (dollControls.elizabeth.activeSelf)
                {
                    ActivateSlot("Shawl Back");
                    ActivateSlot("Shawl Front");

                    currentSlot = currentDollSlots["Shawl Front"];
                    shawlBackSlot = currentDollSlots["Shawl Back"];
                }
                else
                {
                    ActivateSlot("Shawls");
                    currentSlot = currentDollSlots["Shawls"];
                }

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
        ShutDownSlots();
    }

    //Resets the current slot.
    public void ResetSlot(string slot)
    {
        currentSlot = null;

        if (slot == "Gloves")
        {
            otherGloveSlot = null;
            otherGloveSlot.hasClothing = false;
        }else if (slot == "Capes")
        {
            capeBackSlot = null;
        }else if (slot == "Shawls")
        {
            shawlBackSlot = null;
        }else if (slot == "Hats")
        {
            hatBackSlot = null;
        }        
    }

    //Turn off all slots.
    public void ShutDownSlots()
    {
        //Reset the current slot.
        currentSlot = null;

        foreach(KeyValuePair<string, ClothingSlot> pair in currentDollSlots)
        {
            //There is no sprite here, so we turn off the image.
            if (pair.Value.GetComponent<Image>().sprite == null)
            {
                pair.Value.GetComponent<Image>().enabled = false;
            }
        }
    }
}
