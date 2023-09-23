using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewDollControls : MonoBehaviour
{
    public List<MiniDoll> eDolls;
    public List<MiniDoll> jDolls;

    private DollManager dollControls;

    public List<Transform> activeDollPositions;
    public List<Transform> inactiveDollPositions;


    public Sprite eBody;
    public Sprite jBody;

    // Start is called before the first frame update
    void Start()
    {
        dollControls = FindObjectOfType<DollManager>();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateMinis()
    {
       //So first we need to match the mini dolls to their positions.
       if (dollControls.currentDoll.name == "Elizabeth")
        {
            for (int i = 0; i < eDolls.Count; i++)
            {
                eDolls[i].GetComponent<RectTransform>().localPosition = activeDollPositions[i].GetComponent<RectTransform>().localPosition;
                jDolls[i].GetComponent<RectTransform>().localPosition = inactiveDollPositions[i].GetComponent<RectTransform>().localPosition;
            }
        }
        else
        {
            for (int i = 0; i < eDolls.Count; i++)
            {
                eDolls[i].GetComponent<RectTransform>().localPosition = inactiveDollPositions[i].GetComponent<RectTransform>().localPosition;
                jDolls[i].GetComponent<RectTransform>().localPosition = activeDollPositions[i].GetComponent<RectTransform>().localPosition;
            }
        }

        //Now, take each of the images from the current doll and put the same in the mini doll.
        //This needs to be handled different for each doll because they aren't always the same.
        if (dollControls.currentDoll.name == "Elizabeth") {
            for (int i = 0; i < eDolls.Count; i++)
            {
                foreach (KeyValuePair<string, Image> clothes in eDolls[i].clothingBank)
                {
                    eDolls[i].clothingBank[clothes.Key].sprite = dollControls.currentDoll.dollClothing[clothes.Key].sprite;
                    if (eDolls[i].clothingBank[clothes.Key].sprite != null)
                    {
                        eDolls[i].clothingBank[clothes.Key].enabled = true;
                    }
                    else
                    {
                        eDolls[i].clothingBank[clothes.Key].enabled = false;
                    }
                }
            }
            for (int j = 0; j < eDolls.Count; j++)
            {
                foreach (KeyValuePair<string, Image> clothes2 in jDolls[j].clothingBank)
                {
                    jDolls[j].clothingBank[clothes2.Key].sprite = dollControls.inactiveDoll.dollClothing[clothes2.Key].sprite;
                    if (jDolls[j].clothingBank[clothes2.Key].sprite != null)
                    {
                        jDolls[j].clothingBank[clothes2.Key].enabled = true;
                    }
                    else
                    {
                        jDolls[j].clothingBank[clothes2.Key].enabled = false;
                    }
                }

            }
        }
        else
        {
            for (int i = 0; i < eDolls.Count; i++)
            {
                foreach (KeyValuePair<string, Image> clothes in eDolls[i].clothingBank)
                {
                    eDolls[i].clothingBank[clothes.Key].sprite = dollControls.inactiveDoll.dollClothing[clothes.Key].sprite;
                    if (eDolls[i].clothingBank[clothes.Key].sprite != null)
                    {
                        eDolls[i].clothingBank[clothes.Key].enabled = true;
                    }
                    else
                    {
                        eDolls[i].clothingBank[clothes.Key].enabled = false;
                    }
                }
            }
            for (int j = 0; j < eDolls.Count; j++)
            {
                foreach (KeyValuePair<string, Image> clothes2 in jDolls[j].clothingBank)
                {
                    jDolls[j].clothingBank[clothes2.Key].sprite = dollControls.currentDoll.dollClothing[clothes2.Key].sprite;
                    if (jDolls[j].clothingBank[clothes2.Key].sprite != null)
                    {
                        jDolls[j].clothingBank[clothes2.Key].enabled = true;
                    }
                    else
                    {
                        jDolls[j].clothingBank[clothes2.Key].enabled = false;
                    }
                }
            }
        }

        //Clear out any not functional sprites for Elizabeth dolls.
        for (int m = 0; m < eDolls.Count; m++)
        {
            foreach (KeyValuePair<string, Image> sprites in eDolls[m].clothingBank)
            {
                if (eDolls[m].clothingBank[sprites.Key].sprite == null)
                {
                    eDolls[m].clothingBank[sprites.Key].enabled = false;
                }
                else
                {
                    eDolls[m].clothingBank[sprites.Key].enabled = true;
                }
            }
        }

        //Clear out any not functional sprites for Jane dolls.
        for (int n = 0; n < jDolls.Count; n++)
        {
            foreach (KeyValuePair<string, Image> sprites in jDolls[n].clothingBank)
            {
                if (jDolls[n].clothingBank[sprites.Key].sprite == null)
                {
                    jDolls[n].clothingBank[sprites.Key].enabled = false;
                }
                else
                {
                    jDolls[n].clothingBank[sprites.Key].enabled = true;
                }
            }
        }
    }
}
