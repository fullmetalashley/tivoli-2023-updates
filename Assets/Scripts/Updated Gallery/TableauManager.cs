using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script purpose: Controls the UI elements of the tableaus in the gallery scene.
public class TableauManager : MonoBehaviour
{
    //--------------------------------------------------------------
    //SCRIPT REFS
    private DataManager playerData;
    private TableauDatabase tableauList;

    //--------------------------------------------------------------
    //TABLEAU IMAGERY / DETAILS
    public List<Image> tableauImages;
    public List<Transform> eDollPositions;
    public List<Transform> jDollPositions;
    public List<float> dollSizes;   //Used to set scales of each doll.

    public float defaultScale;

    //--------------------------------------------------------------
    //UI ELEMENTS
    //The modal that controls the preview.
    public GameObject firstPreviewDialog;
    public GameObject previewModal;
    public GameObject nav;

    //TABLEAU REFS
    public List<TableauControl> tableauControls;

    //--------------------------------------------------------------
    //TABLEAU SPECIFICS
    public Image finalTableau;

    public GameObject elizabethDoll;
    public GameObject janeDoll;

    public GameObject imageSavedToast;

    public int clickedIndex;

    public List<GameObject> buttons;

    public GameObject backButton;

    //Tableau index.
    public int index;


    // Start is called before the first frame update
    void Start()
    {
        playerData = FindObjectOfType<DataManager>();
        tableauList = FindObjectOfType<TableauDatabase>();


        //Set up the list of available tableaus based on the tableau database.
        for (int i = 0; i < tableauList.activeTableaus.Count; i++)
        {
            tableauImages[i].sprite = tableauList.activeTableaus[i].tableau;
            tableauImages[i].GetComponent<Button>().enabled = true;
            tableauImages[i].GetComponent<Button>().interactable = true;
        }

        //Disables all other tableaus that cannot be accessed yet.
        for (int i = tableauList.activeTableaus.Count; i < (tableauList.activeTableaus.Count + tableauList.inactiveTableaus.Count); i++)
        {
            tableauImages[i].GetComponent<Button>().enabled = false;
        }
    }       

    //Used by the tableau buttons.
    //Toggles the initial preview on based on the index.
    public void TogglePreviewModal(int passedIndex)
    {
        index = passedIndex;
        finalTableau.sprite = tableauImages[index].sprite;
        
        previewModal.SetActive(!previewModal.activeSelf);

        elizabethDoll.transform.localScale *= dollSizes[index];
        janeDoll.transform.localScale *= dollSizes[index];

        //Set the doll positions.
        elizabethDoll.transform.position = eDollPositions[index].position;
        janeDoll.transform.position = jDollPositions[index].position;

        if (previewModal.activeSelf)
        {
            clickedIndex = index;
            nav.SetActive(false);
        }
        else
        {
            nav.SetActive(true);
        }
    }

    //Used by the FIRST back button, same function but with no index.
    //Returns us to the gallery, with all options turned OFF.
    public void TogglePreviewModal()
    {
        //First, turn off all content.
        FindObjectOfType<TableauCaptionControls>().ContentReset();

        //Otherwise, reset them to the default scale.
        elizabethDoll.transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
        janeDoll.transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);

        //Turn off the first preview modal.
        previewModal.SetActive(false);

        //Turn the navigation back on.
        nav.SetActive(true);
    }

    //Turn on caption selection.
    //Called by the "Create tableau" button.
    public void ToggleCaptionSelect()
    {
        //Turn off the first round of selection buttons.
        firstPreviewDialog.SetActive(false);

        //Tell the caption controls to initialize based on our current tableau index.
        FindObjectOfType<TableauCaptionControls>().ToggleModal(index);
    }

    //----------------------------------------------------

//The image has been saved so we can turn things back on.
    public void ImageSaved()
    {
        if (!imageSavedToast.GetComponent<MiniToolTipControl>().running) {
            imageSavedToast.GetComponent<MiniToolTipControl>().StartAnimation();

        }
   

        //Turn on the first image preview content again.
        firstPreviewDialog.SetActive(true);

    }

    //Called by the save button.
    public void TakeScreenshot()
    {
        //Turn off the final caption buttons.
        FindObjectOfType<TableauCaptionControls>().ToggleFinalSaveButtons(false);

        if (imageSavedToast.GetComponent<MiniToolTipControl>().running)
        {
            imageSavedToast.GetComponent<MiniToolTipControl>().EarlyShutDown();
        }

        FindObjectOfType<ScreenshotTableaus>().SaveScreen();
    }

    public void InitiateDelay()
    {
        StartCoroutine(DelayPhoto(3f));
    }

    IEnumerator DelayPhoto(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ImageSaved();
        FindObjectOfType<ScreenshotTableaus>().AddToList();
    }
}
