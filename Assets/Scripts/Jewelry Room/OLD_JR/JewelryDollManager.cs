using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JewelryDollManager : MonoBehaviour
{
    public JewelryDoll currentDoll;
    public JewelryDoll inactiveDoll;
    private List<JewelryDoll> dollList;

    public GameObject JaneObject;
    public GameObject ElizabethObject;

    public int dollIndex;

    public JewelryDoll Elizabeth;
    public JewelryDoll Jane;

    public Image JaneLeftEar;
    public Image JaneRightEar;
    public Image JaneComb;
    public Image JaneNecklace;

    public Image ElizabethLeftEar;
    public Image ElizabethRightEar;
    public Image ElizabethComb;
    public Image ElizabethNecklace;

    private JewelryCloset theCloset;
    private JewelryLoader jewelryPopulator;
    private JewelryDatabase jewelsDatabase;
    private DataManager playerData;

    public ClothingSlot ElizabethTestSlot;

    public Image mirrorDoll;
    public Sprite janeMirror;
    public Sprite elizabethMirror;

    private JewelryRemovalSystem removalJewels;

    // Start is called before the first frame update
    void Start()
    {
        theCloset = FindObjectOfType<JewelryCloset>();
        jewelryPopulator = FindObjectOfType<JewelryLoader>();
        jewelsDatabase = FindObjectOfType<JewelryDatabase>();
        playerData = FindObjectOfType<DataManager>();

        removalJewels = FindObjectOfType<JewelryRemovalSystem>();

        currentDoll = new JewelryDoll();
        inactiveDoll = new JewelryDoll();
        dollList = new List<JewelryDoll>();

        dollIndex = 0;

        Elizabeth = new JewelryDoll
        {
            name = "Elizabeth",
            pose = "3Q"
        };

        Dictionary<string, Image> ElizabethImages = new Dictionary<string, Image>
        {
            { "Left Ear", ElizabethLeftEar },
            { "Right Ear", ElizabethRightEar },
            { "Combs", ElizabethComb },
            { "Necklaces", ElizabethNecklace }
        };
        Elizabeth.dollClothing = ElizabethImages;
        Elizabeth.dollAttire = new Dictionary<string, Sprite>();
        Elizabeth.dollSlots = new Dictionary<string, ClothingSlot>();
        foreach (KeyValuePair<string, Image> jewels in Elizabeth.dollClothing)
        {
            Elizabeth.dollAttire.Add(jewels.Key, Elizabeth.dollClothing[jewels.Key].sprite);
            Elizabeth.dollSlots.Add(jewels.Key, Elizabeth.dollClothing[jewels.Key].GetComponent<ClothingSlot>());

        }

        //Initialize Jane
        Jane = new JewelryDoll
        {
            name = "Jane",
            pose = "Straight"
        };

        Dictionary<string, Image> JaneImages = new Dictionary<string, Image>
        {
            { "Left Ear", JaneLeftEar },
            { "Right Ear", JaneRightEar },
            { "Combs", JaneComb },
            { "Necklaces", JaneNecklace }
        };
        Jane.dollClothing = JaneImages;
        Jane.dollAttire = new Dictionary<string, Sprite>();
        Jane.dollSlots = new Dictionary<string, ClothingSlot>();

        foreach (KeyValuePair<string, Image> jewelsJane in Jane.dollClothing)
        {
            Jane.dollAttire.Add(jewelsJane.Key, Jane.dollClothing[jewelsJane.Key].sprite);
            Jane.dollSlots.Add(jewelsJane.Key, Jane.dollClothing[jewelsJane.Key].GetComponent<ClothingSlot>());
        }


        if (playerData.jewelrySaved)
        {
            if (playerData.elizabeth.jewelry != null)
            {
                Elizabeth.dollAttire = playerData.elizabeth.jewelry;
            }
            else
            {
            }
            if (playerData.jane.jewelry != null)
            {
                Jane.dollAttire = playerData.jane.jewelry;
            }
            else
            {
            }
        }

        if (playerData.lastAccessedDoll != "")
        {
            if (playerData.lastAccessedDoll == "Jane")
            {
                currentDoll = Jane;
                inactiveDoll = Elizabeth;

                ElizabethObject.SetActive(false);
                JaneObject.SetActive(true);
                mirrorDoll.sprite = elizabethMirror;
            }
            else
            {

                currentDoll = Elizabeth;
                inactiveDoll = Jane;
                dollIndex = 0;

                ElizabethObject.SetActive(true);
                JaneObject.SetActive(false);
                mirrorDoll.sprite = janeMirror;

            }
        }
        else
        {

            currentDoll = Elizabeth;
            inactiveDoll = Jane;
            dollIndex = 0;

            ElizabethObject.SetActive(true);
            JaneObject.SetActive(false);
        }



        dollList.Add(currentDoll);
        dollList.Add(inactiveDoll);

        if (playerData.jewelrySaved)
        {
            InitialOutfitDisplay();
        }
    
        
    }

    public void InitialOutfitDisplay()
    {
        foreach(KeyValuePair<string, Image> img in currentDoll.dollClothing){
            currentDoll.dollClothing[img.Key].sprite = currentDoll.dollAttire[img.Key];
            if (currentDoll.dollAttire[img.Key] != null)
            {
                currentDoll.dollClothing[img.Key].enabled = true;
                currentDoll.dollSlots[img.Key].hasClothing = true;
            }
        }

        foreach (KeyValuePair<string, Image> img2 in inactiveDoll.dollClothing)
        {
            inactiveDoll.dollClothing[img2.Key].sprite = inactiveDoll.dollAttire[img2.Key];
            if (inactiveDoll.dollAttire[img2.Key] != null)
            {
                inactiveDoll.dollClothing[img2.Key].enabled = true;
                inactiveDoll.dollSlots[img2.Key].hasClothing = true;

            }
        }

        CheckRemoval();
    }


    public void CheckRemoval()
    {
        JewelryRemovalSystem jewelRemoval = FindObjectOfType<JewelryRemovalSystem>();
        if (!jewelRemoval.CheckForClothing())
        {
            jewelRemoval.removalButton.interactable = false;
            jewelRemoval.RemovalOff();

        }
        else
        {
            jewelRemoval.removalButton.interactable = true;
        }
    }

    public void ResetOutfit()
    {
        foreach (KeyValuePair<string, Image> img in currentDoll.dollClothing)
        {
            currentDoll.dollClothing[img.Key].enabled = false;
            currentDoll.dollAttire[img.Key] = null;
        }
        theCloset.SlotReset(currentDoll);
    }

    //The active doll is swapped with the inactive doll.  Used when one button toggles both dolls.
    public void SwitchDolls()
    {
        removalJewels.RemovalOff();

        inactiveDoll = dollList[dollIndex];

        dollIndex++;
        if (dollIndex == dollList.Count)
        {
            dollIndex = 0;
        }
        currentDoll = dollList[dollIndex];
        if (currentDoll == Elizabeth)
        {
            ElizabethObject.SetActive(true);
            JaneObject.SetActive(false);
            mirrorDoll.sprite = janeMirror;
        }
        else
        {
            ElizabethObject.SetActive(false);
            JaneObject.SetActive(true);
            mirrorDoll.sprite = elizabethMirror;

        }

            if (jewelryPopulator.currentCategory == "Earrings")
        {
            theCloset.currentSlot = currentDoll.dollSlots["Left Ear"];
            theCloset.otherEarSlot = currentDoll.dollSlots["Right Ear"];
        }
        else
        {
            theCloset.currentSlot = currentDoll.dollSlots[jewelryPopulator.currentCategory];
        }
        jewelryPopulator.SwitchLists(jewelryPopulator.currentCategory);
        CheckRemoval();

    }

    //The active doll is swapped with the inactive doll.  Used when specific buttons for specific dolls are in play.
    public void SwitchDolls(string newDoll)
    {
        

        for (int m = 0; m < dollList.Count; m++)
        {
            if (dollList[m].name == newDoll)
            {
                currentDoll = dollList[m];
            }
            else
            {
                inactiveDoll = dollList[m];
            }
        }

       

        if (currentDoll == Elizabeth)
        {
            ElizabethObject.SetActive(true);
            JaneObject.SetActive(false);
            mirrorDoll.sprite = elizabethMirror;
        }
        else
        {
            ElizabethObject.SetActive(false);
            JaneObject.SetActive(true);
            mirrorDoll.sprite = janeMirror;
        }

        if (jewelryPopulator.currentCategory == "Earrings")
        {
            theCloset.currentSlot = currentDoll.dollSlots["Left Ear"];
            theCloset.otherEarSlot = currentDoll.dollSlots["Right Ear"];
        }
        else
        {
            theCloset.currentSlot = currentDoll.dollSlots[jewelryPopulator.currentCategory];
        }
        jewelryPopulator.SwitchLists(jewelryPopulator.currentCategory);

        CheckRemoval();

    }
    //The selected attire is changed dependent on what item is pulled from the clothing populator.
    //We need to reset this based on the display items?  Indexes are being used right now.  We can check
    //How we did it in clothes.
    public void ChangeClothes(int index)
    {
        if (jewelryPopulator.currentCategory == "Earrings")
        {
            currentDoll.dollClothing["Right Ear"].enabled = true;
            currentDoll.dollClothing["Right Ear"].sprite = jewelryPopulator.currentList.assetSprites[index];
            currentDoll.dollAttire["Right Ear"] = jewelryPopulator.currentList.assetSprites[index];

            currentDoll.dollClothing["Left Ear"].enabled = true;
            currentDoll.dollClothing["Left Ear"].sprite = jewelryPopulator.currentList.assetSprites[index];
            currentDoll.dollAttire["Left Ear"] = jewelryPopulator.currentList.assetSprites[index];
        }
        else
        {
            if (jewelryPopulator.currentCategory == "Combs")
            {
                RemoveHat();
            }
            foreach (KeyValuePair<string, Image> img in currentDoll.dollClothing)
            {
                if (img.Key == jewelryPopulator.currentCategory)
                {
                    img.Value.enabled = true;
                    img.Value.sprite = jewelryPopulator.currentList.assetSprites[index];
                    currentDoll.dollAttire[img.Key] = jewelryPopulator.currentList.assetSprites[index];
                }
            }
        }
        CheckRemoval();
    }

    public void RemoveCurrentArticle()
    {
        theCloset.SlotReset(currentDoll);
        theCloset.ResetSprite();

        CheckRemoval();
    }

    public void RemoveSpecificArticle(string category)
    {
        jewelryPopulator.currentCategory = category;

        //We should check to see if that spot even has a piece of clothing.  Only do this if there's clothing here.
        if (theCloset.CheckForClothes(currentDoll))
        {
            theCloset.ResetSprite();
            theCloset.SlotReset(currentDoll);
            CheckRemoval();
        }
    }

    public void SaveDolls()
    {
        playerData.jewelrySaved = true;
        playerData.elizabeth.jewelry = Elizabeth.dollAttire;
        playerData.jane.jewelry = Jane.dollAttire;
        playerData.lastAccessedDoll = currentDoll.name;

    }

    public void RemoveHat()
    {
  /*      if (currentDoll.name == "Elizabeth")
        {
            if (playerData.elizabethAssets != null)
            {
                if (playerData.elizabethAssets["Hats"] != null)
                {
                    playerData.elizabethAssets["Hats"] = null;
                }
            }
        }
        if (currentDoll.name == "Jane")
        {
            if (playerData.janeAssets != null)
            {
                if (playerData.janeAssets["Hats"] != null)
                {
                    playerData.janeAssets["Hats"] = null;
                }
            }
        }
        */
    }
}
