using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelryHitboxController : MonoBehaviour
{
    public List<string> clothingCategories;
    public List<GameObject> hitboxes;
    public Dictionary<string, GameObject> hitboxKeys;

    private JewelryClosetController theCloset;

    // Start is called before the first frame update
    void Start()
    {
        theCloset = FindObjectOfType<JewelryClosetController>();

        hitboxKeys = new Dictionary<string, GameObject>();
        for (int i = 0; i < clothingCategories.Count; i++)
        {
            hitboxKeys.Add(clothingCategories[i], hitboxes[i]);
        }

    }

    public void HitboxOn()
    {
        if (theCloset.currentCategory != "Earrings")
        {
            hitboxKeys[theCloset.currentCategory].SetActive(true);
        }
        else
        {
            //If we have earrings on, we need to figure out which doll is active.
            if (FindObjectOfType<JewelryDollSkin>().ActiveDoll() == "Elizabeth")
            {
                hitboxKeys["Elizabeth Ears"].SetActive(true);
            }
            else
            {
                hitboxKeys["Jane Ears"].SetActive(true);
            }
        }
    }

    public void HitboxOff()
    {
        foreach (KeyValuePair<string, GameObject> pair in hitboxKeys)
        {
            pair.Value.SetActive(false);
        }
    }
}

