using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressingToolTipControl : MonoBehaviour
{
    public GameObject toolTipModal;

    //This arrow is index 3, mirror is 4.
    public GameObject tableauArrow;
    public GameObject mirrorArrow;

    private ToolTipManager tipControls;

    private DataManager theData;

    void Start()
    {
        tipControls = FindObjectOfType<ToolTipManager>();
        theData = FindObjectOfType<DataManager>();

       /*Turning off for GGC.

            if (tipControls.CheckTipStatus("Dressing Tip"))
            {
                toolTipModal.SetActive(false);
            }
            else
            {
                toolTipModal.SetActive(true);
            }




            if (!tipControls.tutorialArrowStatus[3])
            {
 //               tableauArrow.SetActive(false);
            }
            else
            {
  //              tableauArrow.SetActive(true);
            }
            if (!tipControls.tutorialArrowStatus[4])
            {
//                mirrorArrow.SetActive(false);
            }
            else
            {
  //              mirrorArrow.SetActive(true);
            }
        
            if (theData.dataExists)
        {
  //          tableauArrow.SetActive(false);
  //          mirrorArrow.SetActive(false);
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

    public void ToggleArrow(int arrowIndex)
    {
        if (arrowIndex == 3)
        {
            tableauArrow.SetActive(false);

        }
        else
        {
            mirrorArrow.SetActive(false);
        }
        tipControls.tutorialArrowStatus[arrowIndex] = false;
    }
}
