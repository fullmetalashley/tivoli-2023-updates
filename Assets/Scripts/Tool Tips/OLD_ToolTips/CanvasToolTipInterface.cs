using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*NOTE: As of 1/29/2020, I am disabling this.  Our tutorial system needs to be reevaluated anyways.*/

public class CanvasToolTipInterface : MonoBehaviour
{
    public GameObject toolTipModal;
    public GameObject writingTipModal;


    private ToolTipManager tipControls;
    private DataManager playerData;

    public List<GameObject> tutorialArrows;
    public List<bool> tutorialArrowStatus;

    // Start is called before the first frame update
    void Start()
    {
        tipControls = FindObjectOfType<ToolTipManager>();
        playerData = FindObjectOfType<DataManager>();
        /*Turning off for GGC.
        if (!playerData.dataExists)
        {
            if (tipControls.CheckTipStatus("Tip One"))
            {
                toolTipModal.SetActive(false);
            }
            else
            {
                toolTipModal.SetActive(true);
            }

            if (tipControls.CheckTipStatus("Tip Two"))
            {
                writingTipModal.SetActive(false);
            }
            else
            {
                writingTipModal.SetActive(true);
            }

            for (int i = 0; i < tutorialArrows.Count; i++)
            {
                if (!tipControls.tutorialArrowStatus[i])
                {
                    tutorialArrows[i].SetActive(false);
                }
                else
                {
                    tutorialArrows[i].SetActive(true);
                }
            }
        }
        else
        {
            writingTipModal.SetActive(false);
            toolTipModal.SetActive(false);
            for (int i = 0; i < tutorialArrows.Count; i++)
            {
                tutorialArrows[i].SetActive(false);
            }
        }
*/
    }

    public void DataExists()
    {
        if (playerData.dataExists)
        {
            writingTipModal.SetActive(false);
            toolTipModal.SetActive(false);
            for (int i = 0; i < tutorialArrows.Count; i++)
            {
                tutorialArrows[i].SetActive(false);
            }
        }
    }

    public void TipRead(string tipName)
    {
        tipControls.TipRead(tipName);

        if (tipName == "Tip One")
        {
            toolTipModal.SetActive(false);
        }else if (tipName == "Tip Two")
        {
            writingTipModal.SetActive(false);
        }
    }

    public void ToggleArrow(int arrowIndex)
    {
 //       tutorialArrows[arrowIndex].SetActive(false);
  //      tipControls.tutorialArrowStatus[arrowIndex] = false;
    }
}
