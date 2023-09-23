using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelryToolTipControls : MonoBehaviour
{
    public GameObject toolTipModal;

    private ToolTipManager tipControls;

    void Start()
    {
        tipControls = FindObjectOfType<ToolTipManager>();
/*Turning off for GGC.
        if (tipControls.CheckTipStatus("Jewelry Tip"))
        {
            toolTipModal.SetActive(false);
        }
        else
        {
            toolTipModal.SetActive(true);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TipRead(string tipName)
    {
        tipControls.TipRead(tipName);


        toolTipModal.SetActive(false);

    }
}
