using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Sets up the clothing for the main dolls, positioned when the final tableau is open.
public class TableauClothingPopulator : MonoBehaviour
{
    //--------------------------------------------------------------
    //SCRIPT REFS
    private DataManager playerData;

    //--------------------------------------------------------------
    //DOLL REFS
    public MiniDoll finalElizabeth;
    public MiniDoll finalJane;

    //--------------------------------------------------------------
    //INDIVIDUAL TABLEAU CONTROLS
    public List<TableauControl> tableaus;


    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();

        CheckAllDolls();
        SetUpClothing();

    }

    public void CheckAllDolls()
    {
        if (!finalElizabeth.init)
        {
            finalElizabeth.SetUpDatabase();
            finalElizabeth.init = true;
        }

        if (!finalJane.init)
        {
            finalJane.SetUpDatabase();
            finalJane.init = true;
        }
    }

    public void SetUpClothing()
    {
        //SET UP THE CLOTHING GARMENTS
        //If we have a piece of garment, we set it up!
        if (playerData.elizabeth.clothing != null)
        {
            foreach (KeyValuePair<string, Sprite> pair in playerData.elizabeth.clothing)
            {
                if (pair.Value != null)
                {
                    finalElizabeth.clothingBank[pair.Key].sprite = pair.Value;
                }
            }
        }

        if (playerData.jane.clothing != null)
        {
            //If we have a piece of garment, we set it up!
            foreach (KeyValuePair<string, Sprite> pair in playerData.jane.clothing)
            {
                if (pair.Value != null)
                {
                    if (finalJane.clothingBank.ContainsKey(pair.Key))
                    {

                        finalJane.clothingBank[pair.Key].sprite = pair.Value;

                    }
                }
            }
        }

        if (playerData.elizabeth.jewelry != null)
        {
            //SET UP THE JEWELRY
            foreach (KeyValuePair<string, Sprite> pair in playerData.elizabeth.jewelry)
            {
                if (pair.Value != null)
                {
                    Debug.Log("E " + pair.Key + " is not null");
                    if (finalElizabeth.clothingBank.ContainsKey(pair.Key)) {
                        {
                            Debug.Log("Established " + pair.Key + " with a sprite");
                            finalElizabeth.clothingBank[pair.Key].sprite = pair.Value;
                        }
                    }
                }
            }
        }

        if (playerData.jane.jewelry != null)
        {
            foreach (KeyValuePair<string, Sprite> pair in playerData.jane.jewelry)
            {
                if (pair.Value != null)
                {
                    finalJane.clothingBank[pair.Key].sprite = pair.Value;
                }
            }
        }

        PopulateImages();

    }

    //Sets up all of the clothing and jewelry images on the two respective main dolls.
    //Turns them on if they have a sprite.
    public void PopulateImages()
    {
        if (finalElizabeth.clothingBank != null)
        {
            foreach (KeyValuePair<string, Image> pair in finalElizabeth.clothingBank)
            {
                if (pair.Value.sprite != null)
                {
                    pair.Value.enabled = true;
                }
            }
        }

        if (finalJane.clothingBank != null)
        {
            foreach (KeyValuePair<string, Image> pair2 in finalJane.clothingBank)
            {
                if (pair2.Value.sprite != null)
                {
                    pair2.Value.enabled = true;
                }
            }
        }
    }
}
