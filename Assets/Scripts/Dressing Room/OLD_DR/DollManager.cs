using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the dolls as they appear in the dressing room.
    //This functionality is SPECIFIC to the dressing room, and does not carry over to the jewelry room (except for the dolls themselves).
public class DollManager : MonoBehaviour
{
    //INITIALIZATION TOOLS
    //These lists are used for set up to create the doll image dictionaries.
    public List<string> clothingCategoriesElizabeth;
    public List<string> clothingCategoriesJane;

    //This is an initialization tool that helps load the body sprites.
    public Sprite[] dollBases;

    public List<Image> elizabethImages;
    public List<Image> janeImages;

    //------
    //PERSISTENT OBJECTS IN THE SCENE
    //The game objects that show the dolls and their individual images.
    public GameObject elizabethObject;
    public GameObject janeObject;

    //Dolls are added into this list to track.  The index makes note of which doll is active at any given time.
    public int dollIndex;



    //SCRIPT REFS
    private ClosetManager theCloset;
    private ClothingPopulator clothesDatabase;

    public int clothingIndex;

    


    public Dictionary<string, Image> activeDollImages;
    public Dictionary<string, Image> inactiveDollImages;

    //Things from this line down have been edited as of changing the new system.
    public IndividualDoll Elizabeth;
    public IndividualDoll Jane;

    List<IndividualDoll> dollList;
    List<GameObject> dollObjects;

    public GameObject inactiveDollObject;
    public GameObject activeDollObject;

    public IndividualDoll currentDoll;
    public IndividualDoll inactiveDoll;

    public List<IndividualDoll> availableDolls;

    public Transform activePosition;
    public Transform inactivePosition;

    public float inactiveScale;
    public float activeScale;

    private Vector3 activeSize;
    private Vector3 inactiveSize;

    public CanvasGroup janeGroup;
    public CanvasGroup elizabethGroup;

    private DataManager playerData;

    public List<Image> elizabethJewelryImages;
    public List<Image> janeJewelryImages;

    public List<string> jewelryCategories;

    public Dictionary<string, Image> eJewelryDatabase;
    public Dictionary<string, Image> jJewelryDatabase;

    public GameObject reset1Button;
    public GameObject resetAllButton;

    
    void Start()
    {
        //Script initialization.
        playerData = FindObjectOfType<DataManager>();
        theCloset = FindObjectOfType<ClosetManager>();
        clothesDatabase = FindObjectOfType<ClothingPopulator>();

        //We start the list of dolls at 0.
        dollIndex = 0;



        Dictionary<string, Image> elizabethImageDictionary = new Dictionary<string, Image>();
        Dictionary<string, Sprite> elizabethSpriteDictionary = new Dictionary<string, Sprite>();
        Dictionary<string, ClothingSlot> elizabethSlotDictionary = new Dictionary<string, ClothingSlot>();
        for (int i = 0; i < clothingCategoriesElizabeth.Count; i++)
        {
            elizabethImageDictionary.Add(clothingCategoriesElizabeth[i], elizabethImages[i]);
            elizabethSpriteDictionary.Add(clothingCategoriesElizabeth[i], elizabethImages[i].sprite);
            elizabethSlotDictionary.Add(clothingCategoriesElizabeth[i], elizabethImages[i].GetComponent<ClothingSlot>());
        }

        Elizabeth = new IndividualDoll(elizabethImageDictionary, elizabethSpriteDictionary, elizabethSlotDictionary);

        Dictionary<string, Image> janeImageDictionary = new Dictionary<string, Image>();
        Dictionary<string, Sprite> janeSpriteDictionary = new Dictionary<string, Sprite>();
        Dictionary<string, ClothingSlot> janeSlotDictionary = new Dictionary<string, ClothingSlot>();
        for (int j = 0; j < clothingCategoriesJane.Count; j++)
        {
            janeImageDictionary.Add(clothingCategoriesJane[j], janeImages[j]);
            janeSpriteDictionary.Add(clothingCategoriesJane[j], janeImages[j].sprite);
            janeSlotDictionary.Add(clothingCategoriesJane[j], janeImages[j].GetComponent<ClothingSlot>());
        }

        Jane = new IndividualDoll(janeImageDictionary, janeSpriteDictionary, janeSlotDictionary);

        dollList = new List<IndividualDoll>
        {
            {Elizabeth },
            {Jane }
        };
        Elizabeth.pose = "3Q";
        Elizabeth.name = "Elizabeth";
        Jane.pose = "Straight";
        Jane.name = "Jane";


        foreach (KeyValuePair<string, Image> img in Elizabeth.dollClothing)
        {
            Elizabeth.dollClothing[img.Key].enabled = false;
        }
        foreach (KeyValuePair<string, Image> img2 in Jane.dollClothing)
        {
            Jane.dollClothing[img2.Key].enabled = false;
        }
        dollObjects = new List<GameObject>
        {
            elizabethObject,
            janeObject
        };

        if (playerData.dollsSaved)
        {
            Debug.Log("The dolls have been saved, so let's load those assets.");
 //           Elizabeth.dollAttire = playerData.elizabethAssets;
  //          Jane.dollAttire = playerData.janeAssets;
            LoadSavedOutfit();
        }

        if (playerData.lastAccessedDoll != "")
        {
            if (playerData.lastAccessedDoll == "Jane")
            {
                Debug.Log("Jane was saved last, load her.");
                currentDoll = Jane;
                inactiveDoll = Elizabeth;

                activeDollObject = janeObject;
                inactiveDollObject = elizabethObject;
                dollIndex = 1;

                
            }
            else
            {

                currentDoll = Elizabeth;
                inactiveDoll = Jane;

                activeDollObject = elizabethObject;
                inactiveDollObject = janeObject;
                dollIndex = 0;
            }
        }
        else
        {

            currentDoll = Elizabeth;
            inactiveDoll = Jane;

            activeDollObject = elizabethObject;
            inactiveDollObject = janeObject;
            dollIndex = 0;
        }
        
       

  //      inactiveDollObject.transform.localScale *= inactiveScale;

 //       inactiveDollObject.GetComponent<RectTransform>().localPosition = inactivePosition.GetComponent<RectTransform>().localPosition;
  //      activeDollObject.GetComponent<RectTransform>().localPosition = activePosition.GetComponent<RectTransform>().localPosition;

        if (playerData.jewelrySaved)
        {
            eJewelryDatabase = new Dictionary<string, Image>();
            jJewelryDatabase = new Dictionary<string, Image>();

            for (int z = 0; z < jewelryCategories.Count; z++)
            {
                eJewelryDatabase.Add(jewelryCategories[z], elizabethJewelryImages[z]);
                jJewelryDatabase.Add(jewelryCategories[z], janeJewelryImages[z]);

            }
            LoadJewelry();
        }
        else
        {
            for (int m = 0; m < elizabethJewelryImages.Count; m++)
            {
                elizabethJewelryImages[m].enabled = false;
                janeJewelryImages[m].enabled = false;

            }
        }
    }

    public void LoadJewelry()
    {
   /*     foreach(KeyValuePair<string, Sprite> spritesE in playerData.elizabethJewelry)
        {
            if (playerData.elizabethJewelry[spritesE.Key] != null)
            {
                eJewelryDatabase[spritesE.Key].enabled = true;
                eJewelryDatabase[spritesE.Key].sprite = playerData.elizabethJewelry[spritesE.Key];
            }
            else
            {
                eJewelryDatabase[spritesE.Key].enabled = false;

            }
        }
        foreach (KeyValuePair<string, Sprite> spritesJ in playerData.janeJewelry)
        {
            if (playerData.janeJewelry[spritesJ.Key] != null)
            {
                jJewelryDatabase[spritesJ.Key].enabled = true;
                jJewelryDatabase[spritesJ.Key].sprite = playerData.janeJewelry[spritesJ.Key];
            }
            else
            {
                jJewelryDatabase[spritesJ.Key].enabled = false;

            }
        }

        //If a hat was on previously, a jewelry item was the last thing placed, the hat is removed.
        if (eJewelryDatabase["Combs"].enabled)
        {
            Debug.Log("Elizabeth has a comb on.");
            if (Elizabeth.dollSlots["Hats"].hasClothing)
            {
                Debug.Log("She had a hat, so we replaced it.");
                Elizabeth.dollClothing["Hats"].enabled = false;
                Elizabeth.dollClothing["Hats"].sprite = null;
                Elizabeth.dollSlots["Hats"].hasClothing = false;
                playerData.elizabethAssets["Hats"] = null;
            }
            
        }
        if (jJewelryDatabase["Combs"].enabled)
        {
            if (Jane.dollSlots["Hats"].hasClothing)
            {
                Jane.dollClothing["Hats"].enabled = false;
                Jane.dollClothing["Hats"].sprite = null;
                Jane.dollSlots["Hats"].hasClothing = false;
                playerData.janeAssets["Hats"] = null;


            }

        }
        */
    }

    public void LoadSavedOutfit()
    {
        //We also need to make sure that the dolls have the slots set to "Have clothing"
        foreach(KeyValuePair<string, Image> img in Elizabeth.dollClothing)
        {
            if (Elizabeth.dollAttire[img.Key] != null)
            {
                Elizabeth.dollClothing[img.Key].enabled = true;
                Elizabeth.dollClothing[img.Key].sprite = Elizabeth.dollAttire[img.Key];
                Elizabeth.dollSlots[img.Key].hasClothing = true;
            }
        }

        foreach (KeyValuePair<string, Image> img2 in Jane.dollClothing)
        {
            if (Jane.dollAttire[img2.Key] != null)
            {
                Jane.dollClothing[img2.Key].enabled = true;
                Jane.dollClothing[img2.Key].sprite = Jane.dollAttire[img2.Key];
                Jane.dollSlots[img2.Key].hasClothing = true;
            }
        }

        
    }

    //All items for the currently placed doll are reset.
    public void ResetOutfit()
    {
        foreach (KeyValuePair<string, Image> img in currentDoll.dollClothing)
        {
            currentDoll.dollClothing[img.Key].enabled = false;
            currentDoll.dollAttire[img.Key] = null;           
        }
        theCloset.SlotReset(currentDoll);

        CheckRemovalButtons();
    }

    //The active doll is swapped with the inactive doll.
    public void SwitchDolls()
    {
        inactiveDoll = dollList[dollIndex];
        inactiveDollObject = dollObjects[dollIndex];

        dollIndex++;
        if (dollIndex == dollList.Count)
        {
            dollIndex = 0;
        }
        currentDoll = dollList[dollIndex];
        activeDollObject = dollObjects[dollIndex];

        activeDollObject.SetActive(true);
        inactiveDollObject.SetActive(false);

        //Adjust position.
 /*      inactiveDollObject.GetComponent<RectTransform>().localPosition = inactivePosition.GetComponent<RectTransform>().localPosition;
        activeDollObject.GetComponent<RectTransform>().localPosition = activePosition.GetComponent<RectTransform>().localPosition;
        */
 //       //Adjust scale.
 //       activeDollObject.transform.localScale *= 1f / inactiveScale;
 //       inactiveDollObject.transform.localScale *= inactiveScale;
        theCloset.NewSlotActive();
//        clothesDatabase.SwitchLists(clothesDatabase.currentCategory);
    }


    //The selected attire is changed dependent on what item is pulled from the clothing populator.
    public void ChangeClothes(int index)
    {

        //First, we amend the index based on what the outerwear is doing.
        if (clothesDatabase.outerwearOn)
        {

            int actualIndex = 0;
            if (clothesDatabase.currentCategory == "Jackets")
            {
                for (int i = 0; i < clothesDatabase.jacketIndexes.Count; i++)
                {
                    if (clothesDatabase.jacketIndexes[i] == index)
                    {
                        actualIndex += i;
                    }
                }
            }
            else if (clothesDatabase.currentCategory == "Shawls")
            {
                for (int i = 0; i < clothesDatabase.shawlIndexes.Count; i++)
                {
                    if (clothesDatabase.shawlIndexes[i] == index)
                    {
                        actualIndex += i;
                    }
                }
            }
            else if (clothesDatabase.currentCategory == "Capes")
            {
                for (int i = 0; i < clothesDatabase.capeIndexes.Count; i++)
                {
                    if (clothesDatabase.capeIndexes[i] == index)
                    {
                        actualIndex += i;
                    }
                }
            }

            index = actualIndex;
        }


        if (clothesDatabase.currentCategory == "Gloves")
        {

            //Left Glove.
            currentDoll.dollClothing["Left Glove"].enabled = true;
                currentDoll.dollClothing["Left Glove"].sprite = clothesDatabase.currentList.assetSprites[index * 2];
                currentDoll.dollAttire["Left Glove"] = clothesDatabase.currentList.assetSprites[index * 2];


                //Right Glove.
                currentDoll.dollClothing["Right Glove"].enabled = true;
                currentDoll.dollClothing["Right Glove"].sprite = clothesDatabase.currentList.assetSprites[(index *2) + 1];
                currentDoll.dollAttire["Right Glove"] = clothesDatabase.currentList.assetSprites[(index * 2) + 1];
            
            

        }
        //Both dolls need secondary sprites activated for capes.
        else if (clothesDatabase.currentCategory == "Capes")
        {

            //Front Cape.
            currentDoll.dollClothing["Cape Front"].enabled = true;
            currentDoll.dollClothing["Cape Front"].sprite = clothesDatabase.currentList.assetSprites[index * 2];
            currentDoll.dollAttire["Cape Front"] = clothesDatabase.currentList.assetSprites[index * 2];


            //Back Cape.
            currentDoll.dollClothing["Cape Back"].enabled = true;
            currentDoll.dollClothing["Cape Back"].sprite = clothesDatabase.currentList.assetSprites[(index * 2) + 1];
            currentDoll.dollAttire["Cape Back"] = clothesDatabase.currentList.assetSprites[(index * 2) + 1];

            RemoveSpecificArticle("Shawls");
            RemoveSpecificArticle("Jackets");
            RemoveSpecificArticle("Coats");
        }
        //Only Elizabeth needs to have a second sprite activated for a shawl
        else if (clothesDatabase.currentCategory == "Shawls" && currentDoll.pose == "3Q")
        {

            //Front Cape.
            currentDoll.dollClothing["Shawl Front"].enabled = true;
            currentDoll.dollClothing["Shawl Front"].sprite = clothesDatabase.currentList.assetSprites[index * 2];
            currentDoll.dollAttire["Shawl Front"] = clothesDatabase.currentList.assetSprites[index * 2];


            //Back Cape.
            currentDoll.dollClothing["Shawl Back"].enabled = true;
            currentDoll.dollClothing["Shawl Back"].sprite = clothesDatabase.currentList.assetSprites[(index * 2) + 1];
            currentDoll.dollAttire["Shawl Back"] = clothesDatabase.currentList.assetSprites[(index * 2) + 1];

            RemoveSpecificArticle("Capes");
            RemoveSpecificArticle("Jackets");
        }
        else
        {
            if (clothesDatabase.currentCategory == "Jackets")
            {
                RemoveSpecificArticle("Shawls");
                RemoveSpecificArticle("Capes");
                RemoveSpecificArticle("Coats");

            }
            if (clothesDatabase.currentCategory == "Shawls")
            {
                RemoveSpecificArticle("Jackets");
                RemoveSpecificArticle("Capes");
                RemoveSpecificArticle("Coats");

            }
            if (clothesDatabase.currentCategory == "Capes")
            {
                RemoveSpecificArticle("Shawls");
                RemoveSpecificArticle("Jackets");
                RemoveSpecificArticle("Coats");
            }
            if (clothesDatabase.currentCategory == "Coats")
            {
                RemoveSpecificArticle("Shawls");
                RemoveSpecificArticle("Jackets");
                RemoveSpecificArticle("Capes");
            }
            if (clothesDatabase.currentCategory == "Hats")
            {
                //We need to take off the hairpiece from both the current doll and the specific doll.
                if (currentDoll.name == "Jane")
                {
                    if (jJewelryDatabase != null)
                    {
                        jJewelryDatabase["Combs"].enabled = false;
                        jJewelryDatabase["Combs"].sprite = null;


//                        playerData.janeJewelry["Combs"] = null;
                    }

                }
                else
                {
                    
                    if (eJewelryDatabase != null)
                    {
                        Debug.Log("Elizabeth was wearing a comb, we replaced it.");
                        eJewelryDatabase["Combs"].enabled = false;
                        eJewelryDatabase["Combs"].sprite = null;

     //                   playerData.elizabethJewelry["Combs"] = null;
                    }
                }
            }
            foreach (KeyValuePair<string, Image> img in currentDoll.dollClothing)
            {
                if (img.Key == clothesDatabase.currentCategory)
                {
                    img.Value.enabled = true;
                    img.Value.sprite = clothesDatabase.currentList.assetSprites[index];
                    currentDoll.dollAttire[img.Key] = clothesDatabase.currentList.assetSprites[index];
                }
            }
        }

        CheckRemovalButtons();
    }


    public void RemoveSpecificArticle(string category)
    {
        theCloset.SlotReset(category);
        theCloset.ResetSprite(category);

        CheckRemovalButtons();

    }

    public void CheckRemovalButtons()
    {
        //If a piece of clothing in general is on, reset all is available.
        if (theCloset.CheckForAllClothes(currentDoll))
        {
            resetAllButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            resetAllButton.GetComponent<Button>().interactable = false;
        }

        //If a piece of clothing in the current category is on, the reset button is on.
        if (theCloset.CheckForClothes(currentDoll))
        {
            reset1Button.GetComponent<Button>().interactable = true;
        }
        else
        {
            reset1Button.GetComponent<Button>().interactable = false;
        }

        if (clothesDatabase.currentCategory == "Jackets" || clothesDatabase.currentCategory == "Shawls" || clothesDatabase.currentCategory == "Capes")
        {
            if (theCloset.CheckForOuterwear(currentDoll))
            {
                reset1Button.GetComponent<Button>().interactable = true ;
            }
            else
            {
                reset1Button.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void RemoveCurrentArticle()
    {
        Debug.Log("Removing current article");
        theCloset.SlotReset(currentDoll);
        theCloset.ResetSprite();

        CheckRemovalButtons();
    }

    public void SaveDolls()
    {
        playerData.dollsSaved = true;
//        playerData.elizabethAssets = Elizabeth.dollAttire;
 //       playerData.janeAssets = Jane.dollAttire;
        playerData.lastAccessedDoll = currentDoll.name;


    }
}
