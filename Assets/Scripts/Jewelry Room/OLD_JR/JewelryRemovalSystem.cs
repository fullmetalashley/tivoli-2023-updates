using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Controls the very specific removal system in the jewelry room.
public class JewelryRemovalSystem : MonoBehaviour
{
    public GameObject janeRemoval;
    public GameObject elizabethRemoval;

    private JewelryDollSkin jewelDolls;
    private JewelryClosetController theCloset;

    public Button removalButton;

    // Start is called before the first frame update
    void Start()
    {
        jewelDolls = FindObjectOfType<JewelryDollSkin>();
        theCloset = FindObjectOfType<JewelryClosetController>();

    }

    //Determine whether or not clothing exists at this image.
    public bool CheckForClothing()
    {
        if (jewelDolls.elizabeth.activeSelf)
        {
            foreach (KeyValuePair<string, Image> slots in jewelDolls.elizabethJewels)
            {
                if (slots.Value.GetComponent<ClothingSlot>().hasClothing)
                {
                    //There is at least one piece of clothing in here, so we can return true.
                    return true;
                }
            }
            return false;
        }
        else
        {
            foreach (KeyValuePair<string, Image> slots2 in jewelDolls.janeJewels)
            {
                if (slots2.Value.GetComponent<ClothingSlot>().hasClothing)
                {
                    //There is at least one piece of clothing in here, so we can return true.
                    return true;
                }
            }
            return false;
        }
        
    }

    //Remove a specific piece of clothing from the doll.
    public void RemovePiece(string pieceToRemove)
    {
        jewelDolls.RemoveGarment(jewelDolls.ActiveDoll(), pieceToRemove);
        //If all pieces of jewelry are gone, automatically turn off removal system.
    }

    //Toggle the removal categories on and off.
    public void ToggleRemoval()
    {
        Debug.Log("Toggling removal...");
        if (jewelDolls.ActiveDoll() == "Jane")
        {
            janeRemoval.SetActive(!janeRemoval.activeSelf);
        }
        else
        {
            elizabethRemoval.SetActive(!elizabethRemoval.activeSelf);
        }

        if (CheckForClothing())
        {
            Debug.Log("Leave button ON");
            removalButton.interactable = true;
        }
        else
        {
            Debug.Log("Turn OFF button");
            removalButton.interactable = false;
            RemovalOff();
        }
    }

    //Turn off the removal categories.
    public void RemovalOff()
    {
        janeRemoval.SetActive(false);
        elizabethRemoval.SetActive(false);
    }
}
