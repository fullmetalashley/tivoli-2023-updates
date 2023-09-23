using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelryCloset : MonoBehaviour
{
    //The closet manages the item chooser and the doll slots.

    public GameObject itemChooser;

    private JewelryLoader jewelryPopulator;

    private JewelryDatabase jewelryLists;
    private JewelryDollManager dollManager;

    public List<GameObject> categoryButtons;

    public ClothingSlot currentSlot;
    public ClothingSlot otherEarSlot;

    // Start is called before the first frame update
    void Start()
    {
        jewelryPopulator = FindObjectOfType<JewelryLoader>();
        jewelryLists = FindObjectOfType<JewelryDatabase>();
        dollManager = FindObjectOfType<JewelryDollManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateImage()
    {
        if (jewelryPopulator.currentCategory == "Earrings"){
            //Left Earring.
            Color transparentColor = dollManager.currentDoll.dollClothing["Left Ear"].color;
            transparentColor.a = 0f;
            dollManager.currentDoll.dollClothing["Left Ear"].color = transparentColor;
            dollManager.currentDoll.dollClothing["Left Ear"].enabled = true;

            //Right Earring.
            Color transparentColor2 = dollManager.currentDoll.dollClothing["Right Ear"].color;
            transparentColor2.a = 0f;
            dollManager.currentDoll.dollClothing["Right Ear"].color = transparentColor2;
            dollManager.currentDoll.dollClothing["Right Ear"].enabled = true;
        }
        else{
            Color transparentColor = dollManager.currentDoll.dollClothing[jewelryPopulator.currentCategory].color;
            transparentColor.a = 0f;
            dollManager.currentDoll.dollClothing[jewelryPopulator.currentCategory].color = transparentColor;
            dollManager.currentDoll.dollClothing[jewelryPopulator.currentCategory].enabled = true;
        }
    }

    public void ImageColorOn()
    {
        if (jewelryPopulator.currentCategory == "Earrings")
        {
            //Left Ear.
            Color fullColor = dollManager.currentDoll.dollClothing["Left Ear"].color;
            fullColor.a = 1f;
            dollManager.currentDoll.dollClothing["Left Ear"].color = fullColor;

            //Right Ear.
            Color fullColor2 = dollManager.currentDoll.dollClothing["Right Ear"].color;
            fullColor2.a = 1f;
            dollManager.currentDoll.dollClothing["Right Ear"].color = fullColor2;
        }
        else
        {
            Color fullColor = dollManager.currentDoll.dollClothing[jewelryPopulator.currentCategory].color;
            fullColor.a = 1f;
            dollManager.currentDoll.dollClothing[jewelryPopulator.currentCategory].color = fullColor;
        }
    }

    public void DeactivateImage()
    {
        if (jewelryPopulator.currentCategory == "Earrings")
        {
            //Left Ear.
            dollManager.currentDoll.dollClothing["Left Ear"].enabled = false;
            Color activeColor = dollManager.currentDoll.dollClothing["Left Ear"].color;
            activeColor.a = 0f;
            dollManager.currentDoll.dollClothing["Left Ear"].color = activeColor;

            //Right Ear.
            dollManager.currentDoll.dollClothing["Right Ear"].enabled = false;
            Color activeColor2 = dollManager.currentDoll.dollClothing["Right Ear"].color;
            activeColor2.a = 0f;
            dollManager.currentDoll.dollClothing["Right Ear"].color = activeColor2;
        }
        else
        {
            dollManager.currentDoll.dollClothing[jewelryPopulator.currentCategory].enabled = false;
            Color activeColor = dollManager.currentDoll.dollClothing[jewelryPopulator.currentCategory].color;
            activeColor.a = 0f;
            dollManager.currentDoll.dollClothing[jewelryPopulator.currentCategory].color = activeColor;
        }
    }

    public void ResetSprite()
    {
        if (jewelryPopulator.currentCategory == "Earrings")
        {
            
            //Left Ear.
            dollManager.currentDoll.dollClothing["Left Ear"].enabled = false;
            dollManager.currentDoll.dollAttire["Left Ear"] = null;

            //Right Ear.
            dollManager.currentDoll.dollClothing["Right Ear"].enabled = false;
            dollManager.currentDoll.dollAttire["Right Ear"] = null;
        }
        else
        {
            dollManager.currentDoll.dollClothing[jewelryPopulator.currentCategory].enabled = false;
            dollManager.currentDoll.dollAttire[jewelryPopulator.currentCategory] = null;
        }
    }

    //A slot reset takes the current clothing in the slot and disables it, and sets it back to null.
    public void SlotReset(JewelryDoll currentDoll)
    {
        if (currentSlot != null)
        {
            if (jewelryPopulator.currentCategory == "Earrings")
            {
                currentSlot.hasClothing = false;
                if (otherEarSlot != null)
                {
                    otherEarSlot.hasClothing = false;
                }

                if (dollManager.currentDoll.dollAttire["Left Ear"] != null)
                {
                    dollManager.currentDoll.dollSlots["Left Ear"].hasClothing = false;
                }
 /*               else
                {
                    dollManager.currentDoll.dollSlots["Left Ear"].hasClothing = true;

                }
                */
                if (dollManager.currentDoll.dollAttire["Right Ear"] != null)
                {
                    dollManager.currentDoll.dollSlots["Right Ear"].hasClothing = false;
                }
                /*
                else
                {
                    dollManager.currentDoll.dollSlots["Right Ear"].hasClothing = true;
                }
                */
            }
            else
            {
                if (currentSlot != null)
                {
                    currentSlot.hasClothing = false;
                }

                foreach (KeyValuePair<string, Sprite> article in dollManager.currentDoll.dollAttire)
                {
                    if (article.Value != null)
                    {
                        dollManager.currentDoll.dollSlots[article.Key].hasClothing = true;
                    }
                    else
                    {
                        dollManager.currentDoll.dollSlots[article.Key].hasClothing = false;

                    }
                }
            }
        }
    }

    public void NewSlotActive()
    {
        if (jewelryPopulator.currentCategory == "Earrings")
        {
            dollManager.currentDoll.dollSlots["Left Ear"].enabled = true;
            dollManager.currentDoll.dollSlots["Right Ear"].enabled = true;


            currentSlot = dollManager.currentDoll.dollSlots["Left Ear"];
            otherEarSlot = dollManager.currentDoll.dollSlots["Right Ear"];
            currentSlot.enabled = true;
            otherEarSlot.enabled = true;


            foreach(KeyValuePair<string, ClothingSlot> slots in dollManager.currentDoll.dollSlots)
            {
                if (slots.Key != "Left Ear" && slots.Key != "Right Ear")
                {
                    dollManager.currentDoll.dollSlots[slots.Key].enabled = false;
                }
            }
        }
        else
        {
            foreach (KeyValuePair<string, ClothingSlot> slot in dollManager.currentDoll.dollSlots)
            {
                if (slot.Key == jewelryPopulator.currentCategory)
                {
                    currentSlot = dollManager.currentDoll.dollSlots[slot.Key];
                    currentSlot.enabled = true;
                }
                else
                {
                    dollManager.currentDoll.dollSlots[slot.Key].enabled = false;
                }
            }
        }
    }


    public bool CheckForClothes(JewelryDoll currentDoll)
    {
        Debug.Log("Current category: " + jewelryPopulator.currentCategory);
        if (jewelryPopulator.currentCategory == "Earrings")
        {
            if (currentDoll.dollAttire["Left Ear"] != null && currentDoll.dollAttire["Right Ear"] != null)
            {
                Debug.Log("Earrings are here");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            //Loop through the dictionary to see if that category of clothing currently has a sprite.
            foreach (KeyValuePair<string, Sprite> article in currentDoll.dollAttire)
            {
                if (article.Key == jewelryPopulator.currentCategory)
                {
                    if (currentDoll.dollAttire[article.Key] != null)
                    {
                        Debug.Log("Article is here: " + article.Key);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        return false;
    }

    public void ToggleChooser()
    {
        itemChooser.SetActive(true);
        jewelryPopulator.Population();
        if (jewelryPopulator.currentCategory == "Earrings")
        {
            currentSlot = dollManager.currentDoll.dollSlots["Left Ear"];
            otherEarSlot = dollManager.currentDoll.dollSlots["Right Ear"];
        }
        else
        {
            currentSlot = dollManager.currentDoll.dollSlots[jewelryPopulator.currentCategory];
        }
    }

    public void CloseChooser()
    {
        itemChooser.SetActive(false);

    }
}
