using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosetManager : MonoBehaviour
{
    //Closet manager should manage the images and the slots, not the lists.
    //It will control when categories are changed and swapped and will control the flyouts.
    //It also controls the current image on the doll, but this should probably be moved to the doll controller itself.

    private ClothingPopulator clothingChanger;

    public ClothingSlot currentSlot;
    public ClothingSlot otherGloveSlot;
    public ClothingSlot otherCapeSlot;
    public ClothingSlot otherShawlSlot;

    private DollManager dollManager;

    private Color hitboxActive;

    public string currentCategory;

    public List<string> categories;
    public List<FlyoutManager> flyouts;

    public Dictionary<string, FlyoutManager> flyoutControllers;

    // Start is called before the first frame update
    void Start()
    {
        clothingChanger = FindObjectOfType<ClothingPopulator>();
        dollManager = FindObjectOfType<DollManager>();

 //       NewSlotActive();

        Color hitboxActive = new Color(1f, .92f, .016f, .4f);

        flyoutControllers = new Dictionary<string, FlyoutManager>();

        for (int i = 0; i < categories.Count; i++)
        {
            flyoutControllers.Add(categories[i], flyouts[i]);
        }
        Debug.Log("flyout controllers lenght: " + flyoutControllers.Count);

    }

    public void ChangeCategory(string category)
    {
        if (currentCategory != category)
        {
            //Whatever the current category is has to be turned off.
            if (currentCategory != "")
            {
                if (flyoutControllers[currentCategory].active)
                {
                    flyoutControllers[currentCategory].active = false;
                    flyoutControllers[currentCategory].TransitionItems();
                }
            }
        }

        currentCategory = category;
        flyoutControllers[currentCategory].active = !flyoutControllers[category].active;
        flyoutControllers[currentCategory].TransitionItems();
        clothingChanger.SwitchLists(currentCategory, flyoutControllers[currentCategory].clothingPanels);

    }

    //Images are turned on, and rendered transparent until a sprite is applied to the image with the ImageColorOn method.
    public void ActivateImage()
    {
        if (clothingChanger.currentCategory == "Gloves")
        {
            //Left Glove.
            Color transparentColor = dollManager.currentDoll.dollClothing["Left Glove"].color;
            transparentColor.a = 0f;
            dollManager.currentDoll.dollClothing["Left Glove"].color = transparentColor;
            dollManager.currentDoll.dollClothing["Left Glove"].enabled = true;

            //Right Glove.
            Color transparentColor2 = dollManager.currentDoll.dollClothing["Right Glove"].color;
            transparentColor2.a = 0f;
            dollManager.currentDoll.dollClothing["Right Glove"].color = transparentColor2;
            dollManager.currentDoll.dollClothing["Right Glove"].enabled = true;
        }else if (clothingChanger.currentCategory == "Capes")
        {
            //Back Cape.
            Color transparentColor = dollManager.currentDoll.dollClothing["Cape Back"].color;
            transparentColor.a = 0f;
            dollManager.currentDoll.dollClothing["Cape Back"].color = transparentColor;
            dollManager.currentDoll.dollClothing["Cape Back"].enabled = true;

            //Front Cape.
            Color transparentColor2 = dollManager.currentDoll.dollClothing["Cape Front"].color;
            transparentColor2.a = 0f;
            dollManager.currentDoll.dollClothing["Cape Front"].color = transparentColor2;
            dollManager.currentDoll.dollClothing["Cape Front"].enabled = true;
        }
        else if (clothingChanger.currentCategory == "Shawls" && dollManager.currentDoll.pose == "3Q")
        {
            //Shawl Front.
            Color transparentColor = dollManager.currentDoll.dollClothing["Shawl Front"].color;
            transparentColor.a = 0f;
            dollManager.currentDoll.dollClothing["Shawl Front"].color = transparentColor;
            dollManager.currentDoll.dollClothing["Shawl Front"].enabled = true;

            //Shawl Back.
            Color transparentColor2 = dollManager.currentDoll.dollClothing["Shawl Back"].color;
            transparentColor2.a = 0f;
            dollManager.currentDoll.dollClothing["Shawl Back"].color = transparentColor2;
            dollManager.currentDoll.dollClothing["Shawl Back"].enabled = true;
        }
        else
        {
            Color transparentColor = dollManager.currentDoll.dollClothing[clothingChanger.currentCategory].color;
            transparentColor.a = 0f;
            dollManager.currentDoll.dollClothing[clothingChanger.currentCategory].color = transparentColor;
            dollManager.currentDoll.dollClothing[clothingChanger.currentCategory].enabled = true;
        }
    }

    //If an image has a sprite applied to it, the sprite is given full color and can be fully seen now.
    public void ImageColorOn()
    {
        if (clothingChanger.currentCategory == "Gloves")
        {
            //Left Glove.
            Color fullColorL = dollManager.currentDoll.dollClothing["Left Glove"].color;
            fullColorL.a = 1f;
            dollManager.currentDoll.dollClothing["Left Glove"].color = fullColorL;

            //Right Glove.
            Color fullColorR = dollManager.currentDoll.dollClothing["Right Glove"].color;
            fullColorR.a = 1f;
            dollManager.currentDoll.dollClothing["Right Glove"].color = fullColorR;
        }
        else if (clothingChanger.currentCategory == "Capes")
        {
            //Back Cape.
            Color fullColorL = dollManager.currentDoll.dollClothing["Cape Back"].color;
            fullColorL.a = 1f;
            dollManager.currentDoll.dollClothing["Cape Back"].color = fullColorL;

            //Front Cape.
            Color fullColorR = dollManager.currentDoll.dollClothing["Cape Front"].color;
            fullColorR.a = 1f;
            dollManager.currentDoll.dollClothing["Cape Front"].color = fullColorR;
        }
        else if (clothingChanger.currentCategory == "Shawls" && dollManager.currentDoll.pose == "3Q")
        {
            //Back Shawl.
            Color fullColorL = dollManager.currentDoll.dollClothing["Shawl Back"].color;
            fullColorL.a = 1f;
            dollManager.currentDoll.dollClothing["Shawl Back"].color = fullColorL;

            //Front Shawl.
            Color fullColorR = dollManager.currentDoll.dollClothing["Shawl Front"].color;
            fullColorR.a = 1f;
            dollManager.currentDoll.dollClothing["Shawl Front"].color = fullColorR;
        }
        else
        {
            Color fullColor = dollManager.currentDoll.dollClothing[clothingChanger.currentCategory].color;
            fullColor.a = 1f;
            dollManager.currentDoll.dollClothing[clothingChanger.currentCategory].color = fullColor;
        }
    }

    //If an image needs to be turned off, the image is deactivated and the sprite color is reversed back to transparent.
    //Currently, not sure if we need to set the color back to transparent, because this is done in the activate image.
    public void DeactivateImage()
    {
        if (clothingChanger.currentCategory == "Gloves")
        {
            //Left Glove.
            dollManager.currentDoll.dollClothing["Left Glove"].enabled = false;
            Color activeColorL = dollManager.currentDoll.dollClothing["Left Glove"].color;
            activeColorL.a = 0f;
            dollManager.currentDoll.dollClothing["Left Glove"].color = activeColorL;

            //Right Glove.
            dollManager.currentDoll.dollClothing["Right Glove"].enabled = false;
            Color activeColorR = dollManager.currentDoll.dollClothing["Right Glove"].color;
            activeColorR.a = 0f;
            dollManager.currentDoll.dollClothing["Right Glove"].color = activeColorR;
        }
        else if (clothingChanger.currentCategory == "Capes")
        {
            //Back Cape.
            dollManager.currentDoll.dollClothing["Cape Back"].enabled = false;
            Color activeColorL = dollManager.currentDoll.dollClothing["Cape Back"].color;
            activeColorL.a = 0f;
            dollManager.currentDoll.dollClothing["Cape Back"].color = activeColorL;

            //Front Cape.
            dollManager.currentDoll.dollClothing["Cape Front"].enabled = false;
            Color activeColorR = dollManager.currentDoll.dollClothing["Cape Front"].color;
            activeColorR.a = 0f;
            dollManager.currentDoll.dollClothing["Cape Front"].color = activeColorR;
        }
        else if (clothingChanger.currentCategory == "Shawls" && dollManager.currentDoll.pose == "3Q")
        {
            //Back Shawl.
            dollManager.currentDoll.dollClothing["Shawl Back"].enabled = false;
            Color activeColorL = dollManager.currentDoll.dollClothing["Shawl Back"].color;
            activeColorL.a = 0f;
            dollManager.currentDoll.dollClothing["Shawl Back"].color = activeColorL;

            //Front Shawl.
            dollManager.currentDoll.dollClothing["Shawl Front"].enabled = false;
            Color activeColorR = dollManager.currentDoll.dollClothing["Shawl Front"].color;
            activeColorR.a = 0f;
            dollManager.currentDoll.dollClothing["Shawl Front"].color = activeColorR;
        }
        else
        {
            dollManager.currentDoll.dollClothing[clothingChanger.currentCategory].enabled = false;
            Color activeColor = dollManager.currentDoll.dollClothing[clothingChanger.currentCategory].color;
            activeColor.a = 0f;
            dollManager.currentDoll.dollClothing[clothingChanger.currentCategory].color = activeColor;
        }
    }

    public void ResetSprite()
    {
        if (clothingChanger.currentCategory == "Gloves")
        {
            //Left Glove.
            dollManager.currentDoll.dollClothing["Left Glove"].enabled = false;
            dollManager.currentDoll.dollAttire["Left Glove"] = null;

            //Right Glove.
            dollManager.currentDoll.dollClothing["Right Glove"].enabled = false;
            dollManager.currentDoll.dollAttire["Right Glove"] = null;
        }
        else if (clothingChanger.currentCategory == "Capes")
        {
            //Back Cape.
            dollManager.currentDoll.dollClothing["Cape Back"].enabled = false;
            dollManager.currentDoll.dollAttire["Cape Back"] = null;

            //Front Cape.
            dollManager.currentDoll.dollClothing["Cape Front"].enabled = false;
            dollManager.currentDoll.dollAttire["Cape Front"] = null;
        }
        else if (clothingChanger.currentCategory == "Shawls" && dollManager.currentDoll.pose == "3Q")
        {
            //Back Shawl.
            dollManager.currentDoll.dollClothing["Shawl Back"].enabled = false;
            dollManager.currentDoll.dollAttire["Shawl Back"] = null;

            //Front Shawl.
            dollManager.currentDoll.dollClothing["Shawl Front"].enabled = false;
            dollManager.currentDoll.dollAttire["Shawl Front"] = null;
        }else if (clothingChanger.currentCategory == "Jackets")
        {
            if (CheckForOuterwear(dollManager.currentDoll))
            {
                //Some outerwaer is on turn it all off
                //Back Shawl
                Debug.Log("Doll has outerwaer on.");
                if (dollManager.currentDoll.pose == "3Q")
                {
                    dollManager.currentDoll.dollClothing["Shawl Back"].enabled = false;
                    dollManager.currentDoll.dollAttire["Shawl Front"] = null;

                }
                else
                {
                    dollManager.currentDoll.dollClothing["Shawls"].enabled = false;
                    dollManager.currentDoll.dollAttire["Shawls"] = null;
                }


                //CAPES
                //Back Cape.
                dollManager.currentDoll.dollClothing["Cape Back"].enabled = false;
                dollManager.currentDoll.dollAttire["Cape Back"] = null;

                //Front Cape.
                dollManager.currentDoll.dollClothing["Cape Front"].enabled = false;
                dollManager.currentDoll.dollAttire["Cape Front"] = null;

                dollManager.currentDoll.dollClothing["Jackets"].enabled = false;
                dollManager.currentDoll.dollAttire["Jackets"] = null;
            }
        }
        else
        {
            dollManager.currentDoll.dollClothing[clothingChanger.currentCategory].enabled = false;
            dollManager.currentDoll.dollAttire[clothingChanger.currentCategory] = null;
        }
    }

    //A slot reset takes the current clothing in the slot and disables it, and sets it back to null.
    public void SlotReset(IndividualDoll currentDoll)
    {
        if (clothingChanger.currentCategory == "Gloves")
        {
            //Left Glove
            if (dollManager.currentDoll.dollAttire["Left Glove"] != null)
            {
                dollManager.currentDoll.dollSlots["Left Glove"].hasClothing = false;
                dollManager.currentDoll.dollSlots["Right Glove"].hasClothing = false;
            }
            else
            {
                dollManager.currentDoll.dollSlots["Left Glove"].hasClothing = true;
                dollManager.currentDoll.dollSlots["Right Glove"].hasClothing = true;
            }

        }
        else if (clothingChanger.currentCategory == "Capes")
        {
            //Back Cape
            if (dollManager.currentDoll.dollAttire["Cape Back"] != null)
            {
                dollManager.currentDoll.dollSlots["Cape Back"].hasClothing = false;
                dollManager.currentDoll.dollSlots["Cape Front"].hasClothing = false;
            }
            else
            {
                dollManager.currentDoll.dollSlots["Cape Back"].hasClothing = true;
                dollManager.currentDoll.dollSlots["Cape Front"].hasClothing = true;
            }
        }
        else if (clothingChanger.currentCategory == "Shawls" && dollManager.currentDoll.pose == "3Q")
        {
            //Back Shawl
            if (dollManager.currentDoll.dollAttire["Shawl Back"] != null)
            {
                dollManager.currentDoll.dollSlots["Shawl Back"].hasClothing = false;
                dollManager.currentDoll.dollSlots["Shawl Front"].hasClothing = false;
            }
            else
            {
                dollManager.currentDoll.dollSlots["Shawl Back"].hasClothing = true;
                dollManager.currentDoll.dollSlots["Shawl Front"].hasClothing = true;
            }
        }else if (clothingChanger.currentCategory == "Jackets")
        {
            if (CheckForOuterwear(currentDoll)) {
                //Some outerwaer is on turn it all off
                //Back Shawl
                Debug.Log("Doll has outerwaer on.");
                if (currentDoll.pose == "3Q")
                {
                    if (dollManager.currentDoll.dollAttire["Shawl Back"] != null)
                    {
                        dollManager.currentDoll.dollSlots["Shawl Back"].hasClothing = false;
                        dollManager.currentDoll.dollSlots["Shawl Front"].hasClothing = false;
                    }
                    else
                    {
                        dollManager.currentDoll.dollSlots["Shawl Back"].hasClothing = true;
                        dollManager.currentDoll.dollSlots["Shawl Front"].hasClothing = true;
                    }
                }
                else
                {
                    if (dollManager.currentDoll.dollAttire["Shawls"] != null)
                    {
                        dollManager.currentDoll.dollSlots["Shawls"].hasClothing = true;

                    }
                    else
                    {
                        dollManager.currentDoll.dollSlots["Shawls"].hasClothing = false;

                    }
                }
               

                //CAPES
                if (dollManager.currentDoll.dollAttire["Cape Back"] != null)
                {
                    dollManager.currentDoll.dollSlots["Cape Back"].hasClothing = false;
                    dollManager.currentDoll.dollSlots["Cape Front"].hasClothing = false;
                }
                else
                {
                    dollManager.currentDoll.dollSlots["Cape Back"].hasClothing = true;
                    dollManager.currentDoll.dollSlots["Cape Front"].hasClothing = true;
                }
                

                //JACKETS
                if (dollManager.currentDoll.dollAttire["Jackets"] != null)
                {
                    dollManager.currentDoll.dollSlots["Jackets"].hasClothing = false;
                }
            }
        }
        else
        {
            currentSlot.hasClothing = false;
            foreach (KeyValuePair<string, Sprite> article in dollManager.currentDoll.dollAttire)
            {
                if (article.Value != null)
                {
                    dollManager.currentDoll.dollSlots[article.Key].hasClothing = false;
                }
                else
                {
                    dollManager.currentDoll.dollSlots[article.Key].hasClothing = true;
                }
            }
        }
    }

    //This will specifically be used to toggle off outerwear.
    public void SlotReset(string specificSlot)
    {
        Debug.Log("Slot reset!");
        //Get the items at that slot and reset them.
        if (specificSlot == "Shawls" && dollManager.currentDoll.pose == "3Q")
        {
            Debug.Log("Resetting shawls");
            dollManager.currentDoll.dollSlots["Shawl Front"].hasClothing = false;
            dollManager.currentDoll.dollSlots["Shawl Back"].hasClothing = false;

        }
        else if (specificSlot == "Capes")
        {
            Debug.Log("Resetting cpaes");
            dollManager.currentDoll.dollSlots["Cape Front"].hasClothing = false;
            dollManager.currentDoll.dollSlots["Cape Back"].hasClothing = false;
        }
        else
        {
            Debug.Log("Resetting whatever");
            dollManager.currentDoll.dollSlots[specificSlot].hasClothing = false;
        }
                
    }

    public void ResetSprite(string specificSprite)
    {
        if (specificSprite == "Shawls" && dollManager.currentDoll.pose == "3Q")
        {
            dollManager.currentDoll.dollClothing["Shawl Back"].enabled = false;
            dollManager.currentDoll.dollAttire["Shawl Back"] = null;

            dollManager.currentDoll.dollClothing["Shawl Front"].enabled = false;
            dollManager.currentDoll.dollAttire["Shawl Front"] = null;
        }
        else if (specificSprite == "Capes")
        {
            dollManager.currentDoll.dollClothing["Cape Front"].enabled = false;
            dollManager.currentDoll.dollAttire["Cape Front"] = null;

            dollManager.currentDoll.dollClothing["Cape Back"].enabled = false;
            dollManager.currentDoll.dollAttire["Cape Back"] = null;
        }
        else
        {
            dollManager.currentDoll.dollClothing[specificSprite].enabled = false;
            dollManager.currentDoll.dollAttire[specificSprite] = null;
        }
            

    }



    public void NewSlotActive()
    {
        if (clothingChanger.currentCategory == "Gloves")
        {
            currentSlot = dollManager.currentDoll.dollSlots["Left Glove"];
            otherGloveSlot = dollManager.currentDoll.dollSlots["Right Glove"];

            currentSlot.enabled = true;
            otherGloveSlot.enabled = true;

            foreach(KeyValuePair<string, ClothingSlot> slot2 in dollManager.currentDoll.dollSlots)
            {
                if (slot2.Key != "Left Glove" && slot2.Key != "Right Glove")
                {
                    dollManager.currentDoll.dollSlots[slot2.Key].enabled = false;

                }
            }
        }
        else if (clothingChanger.currentCategory == "Capes")
        {
            currentSlot = dollManager.currentDoll.dollSlots["Cape Front"];

            currentSlot.enabled = true;

            foreach (KeyValuePair<string, ClothingSlot> slot2 in dollManager.currentDoll.dollSlots)
            {
                if (slot2.Key != "Cape Front" && slot2.Key != "Cape Back")
                {
                    dollManager.currentDoll.dollSlots[slot2.Key].enabled = false;

                }
            }
        }
        else if (clothingChanger.currentCategory == "Shawls" && dollManager.currentDoll.pose == "3Q")
        {
            currentSlot = dollManager.currentDoll.dollSlots["Shawl Front"];
            currentSlot.enabled = true;

            foreach (KeyValuePair<string, ClothingSlot> slot2 in dollManager.currentDoll.dollSlots)
            {
                if (slot2.Key != "Shawl Front" && slot2.Key != "Shawl Back")
                {
                    dollManager.currentDoll.dollSlots[slot2.Key].enabled = false;

                }
            }
        }
        else
        {
            if (otherGloveSlot != null)
            {
                otherGloveSlot.enabled = false;
            }
            foreach (KeyValuePair<string, ClothingSlot> slot in dollManager.currentDoll.dollSlots)
            {
                if (slot.Key == clothingChanger.currentCategory)
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


    public bool CheckForAllClothes(IndividualDoll currentDoll)
    {
        foreach (KeyValuePair<string, Sprite> article in currentDoll.dollAttire)
        {
            //At least one article of clothing exists on the doll.
            if (currentDoll.dollAttire[article.Key] != null)
            {
                return true;
            }
        }
        return false;
    }
      

    public bool CheckForClothes(IndividualDoll currentDoll)
    {
        if (clothingChanger.currentCategory == "Gloves")
        {
            if (currentDoll.dollAttire["Left Glove"] != null && currentDoll.dollAttire["Right Glove"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (currentDoll.pose == "3Q" && clothingChanger.currentCategory == "Shawls")
        {
            if (currentDoll.dollAttire["Shawl Front"] != null && currentDoll.dollAttire["Shawl Back"] != null)
            {
                return true;

            }
            else
            {
                return false;
            }
                

        }
        else if (clothingChanger.currentCategory == "Capes")
        {
            if (currentDoll.dollAttire["Cape Front"] != null && currentDoll.dollAttire["Cape Back"] != null)
            {
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
                if (article.Key == clothingChanger.currentCategory)
                {
                    if (currentDoll.dollAttire[article.Key] != null)
                    {
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

    public bool CheckForOuterwear(IndividualDoll currentDoll)
    {
        if (currentDoll.pose == "3Q")
        {
            if (currentDoll.dollAttire["Jackets"] != null || currentDoll.dollAttire["Cape Front"] != null || currentDoll.dollAttire["Shawl Front"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (currentDoll.dollAttire["Jackets"] != null || currentDoll.dollAttire["Cape Front"] != null || currentDoll.dollAttire["Shawls"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
