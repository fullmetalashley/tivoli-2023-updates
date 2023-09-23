using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Control the caption settings for the tableau.
public class TableauCaptionControls : MonoBehaviour
{
    public GameObject captionModal;

    public GameObject finalImageContent;

    public List<GameObject> finalSaveButtons;

    //A list of all individual tableaus that will change the headers as necessary.
    public List<TableauCaptions> tableauCaptions;

    //The current index of our tableau.
    public int currentTableau;

    //This turns the modal on.
    //Called when the "Create tableau" button is pressed.
    public void ToggleModal(int index)
    {
        //This is only called by the tableaus themselves, so let's turn off all of the objects to start.
        //Turn all other buttons and headers off.
        for (int i = 0; i < tableauCaptions.Count; i++)
        {
            tableauCaptions[i].ToggleButtons(false);
                tableauCaptions[i].ToggleHeaders(false);
                tableauCaptions[i].quote.enabled = false;        
        }

        currentTableau = index;

        captionModal.SetActive(true);

        //Set the appropriate buttons on.
        tableauCaptions[index].ToggleButtons(true);
    }

    //Open the modal back up if we have hit the Back button from the save.
    public void ToggleModalBackOn()
    {
        //This is only called by the tableaus themselves, so let's turn off all of the objects to start.
        //Turn all other buttons and headers off.
        for (int i = 0; i < tableauCaptions.Count; i++)
        {
            tableauCaptions[i].ToggleButtons(false);
            tableauCaptions[i].ToggleHeaders(false);
            tableauCaptions[i].quote.enabled = false;
        }

        captionModal.SetActive(true);

        //Turn off the final save content.
        ToggleFinalSaveButtons(false);
        finalImageContent.SetActive(false);

        //Set the appropriate buttons on.
        tableauCaptions[currentTableau].ToggleButtons(true);
    }

    //Reset all tableau captions and turn them all off.
    public void ContentReset()
    {
        //Turn all other buttons and headers off.
        for (int i = 0; i < tableauCaptions.Count; i++)
        {

            tableauCaptions[i].ToggleButtons(false);
            tableauCaptions[i].ToggleHeaders(false);
            tableauCaptions[i].quote.enabled = false;
        }
    }

    //This turns the modal OFF.
    //Called when the back button on the Create Tableau dialog is pressed.
    public void ToggleModalOff()
    {
        //Turn all other buttons and headers off.
        for (int i = 0; i < tableauCaptions.Count; i++)
        {

            tableauCaptions[i].ToggleButtons(false);
            tableauCaptions[i].ToggleHeaders(false);
            tableauCaptions[i].quote.enabled = false;

        }

        FindObjectOfType<TableauManager>().firstPreviewDialog.SetActive(true);

        tableauCaptions[currentTableau].ToggleButtons(false); //Turn buttons on and off as necessary.

        captionModal.SetActive(false);
    }

    //This sets the next UI text to be the caption.
    //This is called by one of the individual buttons, which should set the right quote to inactive.
    public void ActivateCaption(int index)
    {
        captionModal.SetActive(false);
        finalImageContent.SetActive(true);

        //Also turn off the first preview dialog.
        FindObjectOfType<TableauManager>().firstPreviewDialog.SetActive(false);

        //We have the index of that button, so set that tableau caption on.
        tableauCaptions[currentTableau].tableauHeaders[index].enabled = true;
        tableauCaptions[currentTableau].quote.enabled = true;

        ToggleFinalSaveButtons(true);
    }

    //Turns the final imagery on / off.
    public void ToggleFinalSaveButtons(bool status)
    {
        for (int i = 0; i < finalSaveButtons.Count; i++)
        {
            finalSaveButtons[i].SetActive(status);
        }
    }
}
