using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelrySpreadController : MonoBehaviour
{
    public List<GameObject> hairItems;
    public List<GameObject> earringItems;
    public List<GameObject> necklaceItems;

    public JewelryDatabase jewelryLists;

    public JewelryLoader jewelryLoad;

    public GameObject itemActivation;


    // Start is called before the first frame update
    void Start()
    {
        jewelryLists = FindObjectOfType<JewelryDatabase>();
        jewelryLoad = FindObjectOfType<JewelryLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleItems()
    {
        jewelryLoad.PopulationDisplay();
        for (int i = 0; i < hairItems.Count; i++)
        {
            hairItems[i].GetComponent<JewelryMovement>().StartTransition();
        }
        for (int j = 0; j < earringItems.Count; j++)
        {
            earringItems[j].GetComponent<JewelryMovement>().StartTransition();
        }
        for (int k = 0; k < necklaceItems.Count; k++)
        {
            necklaceItems[k].GetComponent<JewelryMovement>().StartTransition();
        }
    }


}
