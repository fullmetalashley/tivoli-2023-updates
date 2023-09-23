using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public List<string> clothingCategories;
    public List<GameObject> hitboxes;
    public Dictionary<string, GameObject> hitboxKeys;

    private ClosetControls theCloset;
    // Start is called before the first frame update
    void Start()
    {
        theCloset = FindObjectOfType<ClosetControls>();

        hitboxKeys = new Dictionary<string, GameObject>();
        for (int i = 0; i < clothingCategories.Count; i++)
        {
            hitboxKeys.Add(clothingCategories[i], hitboxes[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitboxOn()
    {
            hitboxKeys[theCloset.subcategory].SetActive(true);
    }

    public void HitboxOff()
    {
        foreach(KeyValuePair<string, GameObject> pair in hitboxKeys)
        {
            pair.Value.SetActive(false);
        }
    }
}
