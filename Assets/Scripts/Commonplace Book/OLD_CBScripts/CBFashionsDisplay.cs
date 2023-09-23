using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CBFashionsDisplay : MonoBehaviour
{
    public List<Image> fashionIcons;
    private ClothingDatabase clothingLists;

    public Sprite unknownItem;

    // Start is called before the first frame update
    void Start()
    {
        clothingLists = FindObjectOfType<ClothingDatabase>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParseList()
    {
        int internalIndex = 0;
        for (int j = 0; j < fashionIcons.Count; j++)
        {
            if (internalIndex < clothingLists.displayItems.signifier.Count)
            {
                if (clothingLists.displayItems.activeStatus[clothingLists.displayItems.signifier[internalIndex]])
                {
                    fashionIcons[j].sprite = clothingLists.displayItems.displayIcons[clothingLists.displayItems.signifier[internalIndex]];
                }
                else
                {
                    fashionIcons[j].sprite = unknownItem;

                }
                internalIndex++;
            }
            else
            {
                fashionIcons[j].sprite = unknownItem;
            }
        }       
    }
}
