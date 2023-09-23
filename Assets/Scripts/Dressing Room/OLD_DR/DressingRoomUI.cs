using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DressingRoomUI : MonoBehaviour
{

    public GameObject elizabethDoll;
    public GameObject janeDoll;
    public GameObject dollSwap;

    public GameObject dressingRoom;
    public GameObject tableauPreview;

    public GameObject background;

    public GameObject tableauSelectionScreen;

    public GameObject finalTableauScreen;

    public List<Image> highlights;
    public List<Image> tableaus;

    public int selectedTableau;

    public Button tableauPreviewButton;
    public Button finalizeButton;

    private DollManager dollControls;
    private TableauDatabase tableauList;
    private ScreenshotTableaus screenshotControl;

    public GameObject tableauSlider;
    public GameObject optionsWindow;

    public GameObject pageNav;

    //The list of scales used to determine how the dolls shrink during the tableau preview.
    public List<float> tableauPreviewSizes;

    //The list of scales used to determine the sizing of the dolls when the tableau is active as a screenshot.
    public List<float> tableauFinalSizes;

    public Image finalTableau;
    public Image previewTableau;

    public Sprite unknownSprite;

    private FadeControls theFade;
    public GameObject theFadeObject;

    private DataManager playerData;

    public GameObject returnToDRButton;
    public GameObject regularBackButton;

    public GameObject screenTableauButton;
    public GameObject mainMenuButton;


    public Transform inactiveDollStartPoint;
    public Transform activeDollStartPoint;

    //The positions the dolls go to when they're in their active tableaus.
    public Transform inactiveDollTableauPoint;
    public Transform activedollTableauPoint;

    //The positions the dolls go to for the final tableau image.  Listed in reference to the corresponding tableau index.
    public List<Transform> activeDollFinalTableauPoints;
    public List<Transform> inactiveDollFinalTableauPoints;

    //The positions the dolls go to for the preview tableau image.  Listed in reference to the corresponding tableau index.
    public List<Transform> activeDollPreviewTableauPoints;
    public List<Transform> inactiveDollPreviewTableauPoints;

    private void Start()
    {
        dollControls = FindObjectOfType<DollManager>();
        tableauList = FindObjectOfType<TableauDatabase>();
        theFade = FindObjectOfType<FadeControls>();
        playerData = FindObjectOfType<DataManager>();
        screenshotControl = FindObjectOfType<ScreenshotTableaus>();
        selectedTableau = 0;


        finalTableau.sprite = tableauList.activeTableaus[selectedTableau].tableau;

        tableauPreviewButton.interactable = false;
        finalizeButton.interactable = false;
        
        
    }

 /*   public void PopulateTableaus()
    {
        for (int i = 0; i < tableauList.activeTableaus.Count; i++)
        {
            tableaus[i].sprite = tableauList.activeTableaus[i].tableau;
            tableaus[i].GetComponent<Button>().interactable = true;

        }

        for (int m = tableauList.activeTableaus.Count; m < tableaus.Count; m++)
        {
            tableaus[m].sprite = unknownSprite;
            tableaus[m].GetComponent<Button>().interactable = false;
        }
    }

    //Turn the TableauPreview on and off.
    public void ToggleTableauPreview()
    {
        elizabethDoll.SetActive(!elizabethDoll.activeSelf);
        janeDoll.SetActive(!janeDoll.activeSelf);
        tableauPreview.SetActive(!tableauPreview.activeSelf);
        
        if (tableauPreview.activeSelf)
        {
            //The preview is on, so set the dolls to the actual tableau positions / sizes.
            dollControls.inactiveDollObject.transform.localScale *= 1f / dollControls.inactiveScale;

            dollControls.inactiveDollObject.transform.localScale *= tableauPreviewSizes[selectedTableau];
            dollControls.activeDollObject.transform.localScale *= tableauPreviewSizes[selectedTableau];



            dollControls.inactiveDollObject.transform.position = inactiveDollPreviewTableauPoints[selectedTableau].position;
            dollControls.activeDollObject.transform.position = activeDollPreviewTableauPoints[selectedTableau].position;

            
        }
        else
        {
            //The preivew is off, so set the dolls to the regular dressing positions / sizes.

            dollControls.inactiveDollObject.transform.localScale *= 1f / tableauPreviewSizes[selectedTableau];
            dollControls.activeDollObject.transform.localScale *= 1f / tableauPreviewSizes[selectedTableau];


            dollControls.inactiveDollObject.transform.position = inactiveDollStartPoint.position;
            dollControls.activeDollObject.transform.position = activeDollStartPoint.position;


            dollControls.inactiveDollObject.transform.localScale *= dollControls.inactiveScale;

        }

    }

    public void ToggleSmallPreviewDolls()
    {
        //What if we just skin the clothing onto these dolls?  Essentially, they're duplicates.

    }

    

    public void TableauController()
    {
        //Automatically toggle the tableau selection screen.
        tableauSelectionScreen.SetActive(!tableauSelectionScreen.activeSelf);
        dollSwap.SetActive(!dollSwap.activeSelf);

        //Also set animator to close.
        pageNav.GetComponent<NavController>().ToggleSlide();

        //This manages what to do with the selection screen.  Dolls are turned off and the tableau selection is defaulted back.
        if (tableauSelectionScreen.activeSelf)
        {
            janeDoll.SetActive(false);
            elizabethDoll.SetActive(false);
            if (selectedTableau != 0)
            {
                SelectTableau(selectedTableau);
            }
        }
        else
        {
            janeDoll.SetActive(true);
            elizabethDoll.SetActive(true);
        }

        if (tableauPreview.activeSelf)
        {
            tableauPreview.SetActive(false);

            dollControls.inactiveDollObject.transform.localScale *= 1f / tableauPreviewSizes[selectedTableau];
            dollControls.activeDollObject.transform.localScale *= 1f / tableauPreviewSizes[selectedTableau];


            dollControls.inactiveDollObject.transform.position = inactiveDollStartPoint.position;
            dollControls.activeDollObject.transform.position = activeDollStartPoint.position;


            dollControls.inactiveDollObject.transform.localScale *= dollControls.inactiveScale;
        }

        if (finalTableauScreen.activeSelf)
        {
            TurnOffTableau();
            tableauPreview.SetActive(false);
            tableauSelectionScreen.SetActive(false);
        }
    }


    public void SelectTableau(int index)
    {
        tableauPreviewButton.interactable = true;
        finalizeButton.interactable = true;

        if (index < tableauList.activeTableaus.Count)
        {
            for (int i = 0; i < highlights.Count; i++)
            {
                if (i != index)
                {
                    //Highlights are turned off if they're not at the right button.
                    highlights[i].enabled = false;
                }
                else
                {
                        highlights[i].enabled = true;
                        selectedTableau = index;
                }
            }
        }
        else
        {
            selectedTableau = 0;
        }

        previewTableau.sprite = tableauList.activeTableaus[selectedTableau].tableau;
        finalTableau.sprite = tableauList.activeTableaus[selectedTableau].tableau;
        SaveTableau();
    }

    public void TurnOnTableau()
    {
        if (pageNav.GetComponent<NavController>().slideActive)
        {
            pageNav.GetComponent<NavController>().ToggleSlide();
        }


        theFadeObject.SetActive(true);
        theFade = theFadeObject.GetComponent<FadeControls>();
        theFade.FadeToBlack(0.5f);
        StartCoroutine(DelayTurnOn(1.5f));
    }

    IEnumerator DelayTurnOn(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        theFade.TurnOffFade(0.5f);
        dressingRoom.SetActive(false);
        background.SetActive(false);
        tableauSelectionScreen.SetActive(false);
        finalTableauScreen.SetActive(true);
        dollSwap.SetActive(false);
        elizabethDoll.SetActive(true);
        janeDoll.SetActive(true);

        tableauSlider.SetActive(false);



        dollControls.inactiveDollObject.transform.localScale *= 1f / dollControls.inactiveScale;

        dollControls.inactiveDollObject.transform.localScale *= tableauFinalSizes[selectedTableau];
        dollControls.activeDollObject.transform.localScale *= tableauFinalSizes[selectedTableau];

        dollControls.inactiveDollObject.transform.position = inactiveDollFinalTableauPoints[selectedTableau].position;
        dollControls.activeDollObject.transform.position = activeDollFinalTableauPoints[selectedTableau].position;

    }

    public void TurnOffTableau()
    {
        dressingRoom.SetActive(true);
        background.SetActive(true);
        finalTableauScreen.SetActive(false);
        dollSwap.SetActive(true);
        janeDoll.SetActive(true);
        elizabethDoll.SetActive(true);
        tableauSlider.SetActive(true);

        dollControls.inactiveDollObject.transform.localScale *= dollControls.inactiveScale;


        dollControls.inactiveDollObject.transform.localScale *= 1f / tableauFinalSizes[selectedTableau];
        dollControls.activeDollObject.transform.localScale *= 1f / tableauFinalSizes[selectedTableau];



        dollControls.inactiveDollObject.transform.position = inactiveDollStartPoint.position;
        dollControls.activeDollObject.transform.position = activeDollStartPoint.position;
    }

    public void SaveTableau()
    {
        playerData.tableauSaved = true;
        playerData.tableauIndex = selectedTableau;
    }

    public void ScreenshotTableau()
    {
        //Turn off mouse if it exists
        mainMenuButton.SetActive(false);    //Turn off main menu button
        returnToDRButton.SetActive(false);  //Turn off return to DR button
        pageNav.SetActive(false);
        optionsWindow.SetActive(false);
        playerData.tableauDates.Add(new System.DateTime(playerData.currentPlayedInGame.Year, playerData.currentPlayedInGame.Month, playerData.currentPlayedInGame.Day));
        //TAKE SCREENSHOT
        StartCoroutine(ScreenshotDelay(.01f));
        StartCoroutine(TurnButtonsOnDelay(1.5f));

    }



    IEnumerator ScreenshotDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        screenshotControl.SaveScreen();
    }

    IEnumerator TurnButtonsOnDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TurnTableauButtonsBackOn();
    }

    public void TurnTableauButtonsBackOn()
    {
        Debug.Log("Turning buttons back on.");
        //Turn all the above back on
        mainMenuButton.SetActive(true);    //Turn on main menu button
        pageNav.SetActive(true);
        optionsWindow.SetActive(true);
        screenshotControl.AddToList();
    }
    */
}
